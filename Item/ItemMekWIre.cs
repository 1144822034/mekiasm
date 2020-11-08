using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    class ItemMekWire:Item
    {
        public BlockMesh mainMesh = new BlockMesh();
        public BlockMesh drawMesh = new BlockMesh();
        public Texture2D texture;
        public BlockMesh boundmesh = new BlockMesh();
        public Model model;
        public List<string> eles = new List<string>() {"Arm1", "Arm2", "Arm3", "Arm4", "Arm5", "Arm6", "Heart", };
        public static Point3[] Fixdirection = new Point3[6] {
        new Point3(1,0,0),new Point3(0,0,-1),new Point3(-1,0,0),new Point3(0,0,1),new Point3(0,1,0),new Point3(0,-1,0)
        };
        public ItemMekWire(int id,string str):base(id,str) {
            model = ContentManager.Get<Model>("Mekiasm/Models/Wire");
            texture = ContentManager.Get<Texture2D>("Mekiasm/Textures/Wire");
            BlockMesh blockMesh = new BlockMesh();
            Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[1]).ParentBone);
            blockMesh.AppendModelMeshPart(model.FindMesh(eles[1]).MeshParts[0], matrix, false, false, false, true, Color.White);
            mainMesh.AppendBlockMesh(blockMesh);
            mainMesh.TransformPositions(Matrix.CreateTranslation(0.5f, 0.5f, 0.5f));            
            drawMesh.AppendModelMeshPart(model.FindMesh(eles[6]).MeshParts[0], matrix, false, false, false, true, Color.White);
            drawMesh.TransformPositions(Matrix.CreateTranslation(0.5f, 0.5f, 0.5f));

        }
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return new BoundingBox[] { boundmesh.CalculateBoundingBox() };
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer,drawMesh,texture, Color.White,size+2f, ref matrix, environmentData);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return false;
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            BlockMesh drawMesh = new BlockMesh();
            Point3 p = new Point3(x,y,z);
            Terrain terrain = generator.Terrain;
            for (int i=0;i<6;i++) {
                Point3 pp = p + Fixdirection[i];
                int v = terrain.GetCellContents(pp.X,pp.Y,pp.Z);
                if (v == 1003) {
                    BlockMesh blockMesh = new BlockMesh();
                    Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[i]).ParentBone);
                    blockMesh.AppendModelMeshPart(model.FindMesh(eles[i]).MeshParts[0], matrix * Matrix.CreateScale(0.7f), false, false, false, false, Color.White);
                    drawMesh.AppendBlockMesh(blockMesh);
                }
            }
            BlockMesh blockMeshd = new BlockMesh();
            Matrix matrixd = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[6]).ParentBone);
            blockMeshd.AppendModelMeshPart(model.FindMesh(eles[6]).MeshParts[0], matrixd*Matrix.CreateScale(0.7f), false, false, false, false, Color.White);
            drawMesh.AppendBlockMesh(blockMeshd);
            drawMesh.TransformPositions(Matrix.CreateTranslation(0.5f, 0.5f, 0.5f));
            if (!ComponentPlayUpdate.duliBlocks.ContainsKey(p))
            {
                ComponentPlayUpdate.duliBlocks.Add(p, texture);
            }
            generator.GenerateMeshVertices(block,x,y,z,drawMesh,Color.White,Matrix.Identity, ComponentPlayUpdate.GTV(x,y,z,geometry).SubsetTransparent);
            boundmesh = drawMesh;
        }
    }
}
