using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    class MekAdvanceSolarPanel : Item
    {
        public BlockMesh mainMesh = new BlockMesh();
        public Texture2D texture;
        public List<BoundingBox> boundingBoxes = new List<BoundingBox>();
        public List<string> eles = new List<string>() { "parta", "partb"};
        public MekAdvanceSolarPanel(int id, string str) : base(id, str)
        {
            Model model = ContentManager.Get<Model>("Mekiasm/Models/AdvanceSolarPlane");
            texture = ContentManager.Get<Texture2D>("Mekiasm/Textures/AdvanceSolarPlane");
            for (int i = 0; i < eles.Count; i++)
            {
                BlockMesh blockMesh = new BlockMesh();
                Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[i]).ParentBone);
                blockMesh.AppendModelMeshPart(model.FindMesh(eles[i]).MeshParts[0], matrix* Matrix.CreateScale(0.5f)* Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
                mainMesh.AppendBlockMesh(blockMesh);
                boundingBoxes.Add(blockMesh.CalculateBoundingBox());

            }
        }
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return boundingBoxes.ToArray();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, mainMesh, texture, Color.White, 0.5f*size, ref matrix, environmentData);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return false;
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            Point3 p = new Point3(x, y, z);
            if (!ComponentPlayUpdate.duliBlocks.ContainsKey(p))
            {
                ComponentPlayUpdate.duliBlocks.Add(p, texture);
            }
            generator.GenerateMeshVertices(block, x, y, z, mainMesh, Color.White, Matrix.Identity, ComponentPlayUpdate.GTV(x, y, z, geometry).SubsetTransparent);
        }
    }
}
