using Engine;
using Engine.Graphics;
using Game;
namespace Mekiasm
{
    //原子分解机
    public class AtomicDecomposerBlock:Block
    {
        public const int Index = 1008;
        public BlockMesh blockMesh=new BlockMesh();
        public Texture2D texture;
        public override void Initialize()
        {
            Model model = ContentManager.Get<Model>("Mekiasm/Models/AtomicDecomposer");
            Matrix bone = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Atom_main").ParentBone);
            texture = ContentManager.Get<Texture2D>("Mekiasm/Textures/AtmoicDecomposer");
            BlockMesh mesh = new BlockMesh();
            mesh.AppendModelMeshPart(model.FindMesh("Atom_main").MeshParts[0],bone,false,false,false,false,Color.White);
            blockMesh.AppendBlockMesh(mesh);
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, blockMesh, texture,Color.White, size,ref matrix,environmentData);
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }
    }
}
