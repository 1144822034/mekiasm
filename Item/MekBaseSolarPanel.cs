using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    class MekBaseSolarPanel : Item
    {
        public BlockMesh mainMesh = new BlockMesh();
        public Texture2D texture;

        public List<BoundingBox> boundingBoxes = new List<BoundingBox>();
        public List<string> eles = new List<string>() { "TYN"};
        public MekBaseSolarPanel(int id, string str) : base(id, str)
        {
            Model model = ContentManager.Get<Model>("Mekiasm/Models/TYN");
            texture = ContentManager.Get<Texture2D>("Mekiasm/Textures/solarpanel");
            for (int i = 0; i < eles.Count; i++)
            {
                BlockMesh blockMesh = new BlockMesh();
                Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[i]).ParentBone);
                blockMesh.AppendModelMeshPart(model.FindMesh(eles[i]).MeshParts[0], matrix, false, false, false, false, Color.White);
                mainMesh.AppendBlockMesh(blockMesh);
            }
            BlockMesh mesh = mainMesh;
            mesh.TransformPositions(Matrix.CreateScale(1f) * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f));
            boundingBoxes.Add(mesh.CalculateBoundingBox());
        }

        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return boundingBoxes.ToArray();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, mainMesh, texture, Color.White, size, ref matrix, environmentData);
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            Point3 p = new Point3(x, y, z);
            if (!ComponentPlayUpdate.duliBlocks.ContainsKey(p))
            {
                ComponentPlayUpdate.duliBlocks.Add(p, texture);
            }
            generator.GenerateMeshVertices(block, x, y, z, mainMesh, Color.White, Matrix.CreateScale(1f) , ComponentPlayUpdate.GTV(x, y, z, geometry).SubsetTransparent);
        }
    }
}
