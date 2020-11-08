using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Graphics;
using Game;
namespace Mekiasm
{
    public class MekWire:Item
    {
        public MekWire(int d,string n):base(d,n) {
            }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return itemid;
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer,value,new Vector3(size),ref matrix,Color.White,Color.White,environmentData);
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(block,value,x,y,z,Color.White,SubsystemItemBlockBase.GTV(x,z,geometry).OpaqueSubsetsByFace);            
        }

    }
}
