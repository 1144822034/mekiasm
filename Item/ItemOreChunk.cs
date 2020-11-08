using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    public class ItemOreChunk:Item
    {
        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
        public static Color color_=Color.White;
        public ItemOreChunk(string name_,int id_,Color color,bool isIngot_) : base(id_,name_) {
            Model model = isIngot_ ? ContentManager.Get<Model>("Models/Ingots") : ContentManager.Get<Model>("Models/Chunk");
            Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.Meshes[0].ParentBone);
            m_standaloneBlockMesh.AppendModelMeshPart(model.Meshes[0].MeshParts[0], matrix, makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, color);
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer,m_standaloneBlockMesh, color_, 2f * size, ref matrix, environmentData);
        }
    }
}
