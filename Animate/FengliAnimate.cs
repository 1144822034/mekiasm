using Engine;
using Engine.Graphics;
using Game;

namespace Mekiasm
{
    public class FengliAnimate
    {
        public TexturedBatch3D texturedBatch3D;
        public float f = 0.01f;
        public PrimitivesRenderer3D primitives = new PrimitivesRenderer3D();
        public float angle=0.01f;
        public BlockMesh blockMesh = new BlockMesh();
        public string[] eles = new string[] { "ShanYe"};
        public FengliAnimate() {
            Model model = ContentManager.Get<Model>("Mekiasm/Models/风力发电机");
            Texture2D texture = ContentManager.Get<Texture2D>("Mekiasm/Textures/风力发电机");
            for (int i = 0; i < eles.Length; i++)
            {
                Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(eles[i]).ParentBone);
                blockMesh.AppendModelMeshPart(model.FindMesh(eles[i]).MeshParts[0], matrix, false, false, false, false, Color.White);
            }
            texturedBatch3D = primitives.TexturedBatch(texture,false,0,DepthStencilState.DepthWrite,RasterizerState.CullCounterClockwiseScissor,BlendState.Opaque);
        }
        public void Draw(Camera camera,Vector3 point) {
			int indecies = blockMesh.Indices.Count / 3;
			for (int i = 0; i < indecies; i ++)
			{
				BlockMeshVertex blockMeshVertex = blockMesh.Vertices.Array[blockMesh.Indices.Array[i * 3]];
				BlockMeshVertex blockMeshVertex2 = blockMesh.Vertices.Array[blockMesh.Indices.Array[i * 3 + 1]];
				BlockMeshVertex blockMeshVertex3 = blockMesh.Vertices.Array[blockMesh.Indices.Array[i * 3 + 2]];
				Vector3 p = blockMeshVertex.Position + point;
				Vector3 p2 = blockMeshVertex2.Position + point;
				Vector3 p3 = blockMeshVertex3.Position + point;
				texturedBatch3D.QueueTriangle(p, p2, p3, blockMeshVertex.TextureCoordinates, blockMeshVertex2.TextureCoordinates, blockMeshVertex3.TextureCoordinates,Color.White);
			}
            primitives.Flush(camera.ViewProjectionMatrix);
            blockMesh.TransformPositions(Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 2f),f));
        }
    }
}
