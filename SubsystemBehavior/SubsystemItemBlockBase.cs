using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Graphics;
using Game;
using TemplatesDatabase;

namespace Mekiasm
{
    //基类，用自带Texture画材质
    public class SubsystemItemBlockBase : SubsystemBlockBehavior,IDrawable
    {
        public static Terrain terrain;
        public static SubsystemMovingBlocks subsystemMovingBlocks;
        public SubsystemGameInfo subsystemGame;
        public ComponentPlayer componentPlayer;
        public static SubsystemTerrain subsystemTerrain;
        public override int[] HandledBlocks => new int[] { 1001};
        public int[] DrawOrders => new int[] {1};

        public SubsystemItemBlockBase() {
            TerrainUpdater.GenerateChunkVertices1 =ILibrary.GenerateChunkVertices;
            ILibrary.Data = new SortedMultiCollection<Point2, TerrainGeometrySubsets>(128,new XCompare());
        }
        public override void Load(ValuesDictionary valuesDictionary)
        {
            subsystemGame = Project.FindSubsystem<SubsystemGameInfo>(throwOnError: true);
            subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            subsystemMovingBlocks = Project.FindSubsystem<SubsystemMovingBlocks>(throwOnError: true);

            terrain = subsystemTerrain.Terrain;
        }
      
        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {

            componentPlayer = componentMiner.ComponentPlayer;
            return base.OnUse(start, direction, componentMiner);
        }
        public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
        {
            return base.OnInteract(raycastResult, componentMiner);
        }
        public void Draw(Camera camera, int drawOrder)
        {
            KeyValuePair<Point2, TerrainGeometrySubsets>[] array =ILibrary.Data.m_array;
            for (int i = 0; i <ILibrary.Data.Count; i++)
            {
                TerrainGeometrySubsets value = array[i].Value;
                if (value != null)
                {
                    Display.BlendState = BlendState.Opaque;//矿物调试可用
                    Display.DepthStencilState = DepthStencilState.Default;
                    Display.RasterizerState = RasterizerState.CullCounterClockwiseScissor;
                    Shader shader = subsystemMovingBlocks.m_shader;
                    SubsystemSky subsystemSky = subsystemMovingBlocks.m_subsystemSky;
                    shader.GetParameter("u_texture").SetValue(ILibrary.texture);
                    shader.GetParameter("u_samplerState").SetValue(SamplerState.PointClamp);
                    shader.GetParameter("u_fogColor").SetValue(new Vector3(subsystemSky.ViewFogColor));
                    shader.GetParameter("u_fogStartInvLength").SetValue(new Vector2(subsystemSky.ViewFogRange.X, 1f / (subsystemSky.ViewFogRange.Y - subsystemSky.ViewFogRange.X)));
                    shader.GetParameter("u_worldViewProjectionMatrix").SetValue(camera.ViewProjectionMatrix);
                    shader.GetParameter("u_viewPosition").SetValue(camera.ViewPosition);
                    Display.DrawUserIndexed(PrimitiveType.TriangleList, shader, TerrainVertex.VertexDeclaration, value.SubsetOpaque.Vertices.Array, 0, value.SubsetOpaque.Vertices.Count, value.SubsetOpaque.Indices.Array, 0, value.SubsetOpaque.Indices.Count);
                }
            }
        }
        public void generateMineralLikeCoal(TerrainChunk chunk,int value,int replacevalue,int minHeight,int maxHeight) { //生成矿物算法-类似煤的生成概率                                             
            int cx = chunk.Coords.X;
            int cy = chunk.Coords.Y;            
            List<TerrainBrush> terrainBrushes = new List<TerrainBrush>();
            Game.Random random = new Game.Random(17);
            for (int i = 0; i < 16; i++)
            {//煤块的生成概率
                TerrainBrush terrainBrush = new TerrainBrush();
                int num = random.UniformInt(4, 12);
                for (int j = 0; j < num; j++)
                {
                    Vector3 vector = 0.5f * Vector3.Normalize(new Vector3(random.UniformFloat(-1f, 1f), random.UniformFloat(-1f, 1f), random.UniformFloat(-1f, 1f)));
                    int num2 = random.UniformInt(3, 8);
                    Vector3 zero = Vector3.Zero;
                    for (int k = 0; k < num2; k++)
                    {
                        terrainBrush.AddBox((int)MathUtils.Floor(zero.X), (int)MathUtils.Floor(zero.Y), (int)MathUtils.Floor(zero.Z), 1, 1, 1, value);
                        zero += vector;
                    }
                }
                if (i == 0)
                    terrainBrush.AddCell(0, 0, 0, 150);
                terrainBrush.Compile();
                terrainBrushes.Add(terrainBrush);
            }
            for (int i = cx - 1; i <= cx + 1; i++)
            {
                for (int j = cy - 1; j <= cy + 1; j++)
                {
                    float num2 =CalculateMountainRangeFactor(i * 16, j * 16);
                    int num3 = (int)(5f + 2f * num2 * SimplexNoise.OctavedNoise(i, j, 0.33f, 1, 1f, 1f));
                    for (int l = 0; l < num3; l++)
                    {
                        int x2 = i * 16 + random.UniformInt(0, 15);
                        int y2 = random.UniformInt(minHeight, maxHeight);
                        int cz = j * 16 + random.UniformInt(0, 15);
                        terrainBrushes[random.UniformInt(0, terrainBrushes.Count - 1)].PaintFastSelective(chunk, x2, y2, cz, replacevalue);
                    }
                }
            }
                }
        public float CalculateMountainRangeFactor(float x, float z)
        {
            return 1f - MathUtils.Abs(2f * SimplexNoise.OctavedNoise(x , z , 0.0014f , 3, 1.91f, 0.75f) - 1f);
        }

        public static TerrainGeometrySubsets GTV(int x, int z, TerrainGeometrySubsets geometry)
        {
            TerrainChunk chunkAtCell = terrain.GetChunkAtCell(x, z);
            if (chunkAtCell == null)
            {
                return geometry;
            }
            if (!ILibrary.Data.TryGetValue(chunkAtCell.Coords, out geometry))
            {
                TerrainGeometrySubset terrainGeometrySubset = new TerrainGeometrySubset(new DynamicArray<TerrainVertex>(), new DynamicArray<ushort>());
                TerrainGeometrySubsets terrainGeometrySubsets = new TerrainGeometrySubsets();
                terrainGeometrySubsets.SubsetOpaque = terrainGeometrySubset;
                terrainGeometrySubsets.SubsetAlphaTest = terrainGeometrySubset;
                terrainGeometrySubsets.SubsetTransparent = terrainGeometrySubset;
                terrainGeometrySubsets.OpaqueSubsetsByFace = new TerrainGeometrySubset[6]
                {
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset
                };
                terrainGeometrySubsets.TransparentSubsetsByFace = new TerrainGeometrySubset[6]
                {
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset
                };
                terrainGeometrySubsets.AlphaTestSubsetsByFace = new TerrainGeometrySubset[6]
                {
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset,
                    terrainGeometrySubset
                };
                geometry = terrainGeometrySubsets;
                ILibrary.Data.Add(chunkAtCell.Coords, geometry);
            }
            return geometry;
        }
     
        public override void OnChunkInitialized(TerrainChunk chunk)
        {//控制矿物生成
         //value对照表
         //18水 7沙子 2泥土 5鹅卵石 3花岗岩
         //8草地 67玄武岩
            if (chunk.IsLoaded) return;
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 0), 3, 30, 50);//生成金矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 1), 67, 25, 55);//生成铀矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 2), 3, 20, 80);//生成锇矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 3), 67, 0, 30);//生成钚矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 4), 67, 0, 30);//生成锂矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 5), 67, 0, 20);//生成镁矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 6), 3, 25, 45);//生成硼矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 0), 3, 0, 55);//生成铅矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 0), 67, 0, 20);//生成钍矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 0), 3, 20, 80);//生成锡矿
            generateMineralLikeCoal(chunk, Terrain.MakeBlockValue(1001, 0, 0), 67, 0, 20);//生成红石矿
        }
    }
}
