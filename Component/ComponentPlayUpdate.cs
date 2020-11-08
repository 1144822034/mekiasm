using Engine;
using Game;
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;
using Engine.Graphics;
using System;

namespace Mekiasm
{
    class ComponentPlayUpdate:Component,IUpdateable,IDrawable
    {
        public ComponentMiner componentMiner;
        public NewMutiBlockCheck newMuti = new NewMutiBlockCheck();
        public List<Point3> points = new List<Point3>();
        public Point3 digPoint;
        public bool transmitFLag = false;
        public SubsystemMovingBlocks subsystemMovingBlocks;
        public SubsystemItemElectricBehavior electricBlockBehavior;
        public SubsystemTerrain subsystemTerrain;
        public int digValue;
        public Shader shader;
        public static Terrain terrain;
        public static Dictionary<Point3, TerrainGeometrySubsets> Data ;
        public bool digFlag = false;
        public List<int> liansuoID = new List<int>() { 16, 148, 41, 101, 39, 41, 112, 72, 100, 9, 10, 11 };
        public int UpdateOrder => 45;
        public int[] DrawOrders => new int[] { 10};
        public static Dictionary<Point3, Texture2D> duliBlocks = new Dictionary<Point3, Texture2D>();

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            componentMiner = Entity.FindComponent<ComponentMiner>();
            subsystemTerrain = Entity.Project.FindSubsystem<SubsystemTerrain>();
            subsystemMovingBlocks = Entity.Project.FindSubsystem<SubsystemMovingBlocks>();
            electricBlockBehavior = Entity.Project.FindSubsystem<SubsystemItemElectricBehavior>();
            terrain = subsystemTerrain.Terrain;
            Data = new Dictionary<Point3, TerrainGeometrySubsets>();
            shader= ContentManager.Get<Shader>("Shaders/Transparent");
        }
        public void Update(float dt)
        {
            if (componentMiner == null) return;
            if (componentMiner.DigProgress > 0.6f && !digFlag)
            {
                digFlag = true;
                digValue = subsystemTerrain.Terrain.GetCellContents(componentMiner.DigCellFace.Value.X, componentMiner.DigCellFace.Value.Y, componentMiner.DigCellFace.Value.Z);
                digPoint = componentMiner.DigCellFace.Value.Point;
                if (liansuoID.Contains(digValue))
                {
                    newMuti.scanBlocks(subsystemTerrain.Terrain, digPoint, new List<int>() { digValue });
                    points = newMuti.cache;
                }
            }
            else if (digFlag)
            {
                if (componentMiner.DigCellFace == null)
                {
                    if (liansuoID.Contains(digValue) && subsystemTerrain.Terrain.GetCellContents(digPoint.X, digPoint.Y, digPoint.Z) == 0)
                    { //煤矿测试
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (points[i] == digPoint) continue;
                            subsystemTerrain.DestroyCell(BlocksManager.Blocks[digValue].RequiredToolLevel, points[i].X, points[i].Y, points[i].Z, 0, false, false);
                        }
                    }
                    digPoint = Point3.Zero;
                    digValue = 0;
                    digFlag = false;
                }
            }
        }
        public static TerrainGeometrySubsets GTV(int x,int y, int z, TerrainGeometrySubsets geometry)
        {
            try
            {
            Point3 point = new Point3(x,y,z);
            TerrainChunk chunkAtCell = terrain.GetChunkAtCell(x, z);
            if (chunkAtCell == null)
            {
                return geometry;
            }
            if (!Data.TryGetValue(point, out geometry))
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
                    Data.Add(point, geometry);
                return geometry;
            }
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
            return geometry;
        }
        public void Draw(Camera camera, int drawOrder)
        { 
                foreach (KeyValuePair<Point3,TerrainGeometrySubsets> item in Data)
                {
                    TerrainGeometrySubsets value = item.Value;
                    if (value != null)
                    {
                        if (!duliBlocks.TryGetValue(item.Key, out Texture2D texture)) continue;
                        Display.BlendState = BlendState.Opaque;
                        Display.DepthStencilState = DepthStencilState.Default;
                        Display.RasterizerState = RasterizerState.CullCounterClockwiseScissor;
                        SubsystemSky subsystemSky = subsystemMovingBlocks.m_subsystemSky;
                        shader.GetParameter("u_texture").SetValue(texture);
                        shader.GetParameter("u_samplerState").SetValue(SamplerState.PointClamp);
                        shader.GetParameter("u_fogColor").SetValue(new Vector3(subsystemSky.ViewFogColor));
                        shader.GetParameter("u_fogStartInvLength").SetValue(new Vector2(subsystemSky.ViewFogRange.X, 1f / (subsystemSky.ViewFogRange.Y - subsystemSky.ViewFogRange.X)));
                        shader.GetParameter("u_worldViewProjectionMatrix").SetValue(camera.ViewProjectionMatrix);
                        shader.GetParameter("u_viewPosition").SetValue(camera.ViewPosition);
                        Display.DrawUserIndexed(PrimitiveType.TriangleList, shader, TerrainVertex.VertexDeclaration, value.SubsetOpaque.Vertices.Array, 0, value.SubsetOpaque.Vertices.Count, value.SubsetOpaque.Indices.Array, 0, value.SubsetOpaque.Indices.Count);
                    }
                }
           
            }
    }
}
