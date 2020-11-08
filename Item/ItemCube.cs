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
    public class ItemCube:Item
    {
        public ItemCube(int itemid_, string displayName_):base(itemid_,displayName_)
        {
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, Color.White, Color.White, environmentData);
        }
        public override void GenerateTerrainVertices(Block block,BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
             generator.GenerateCubeVertices(block, value, x, y, z, Color.White, SubsystemItemBlockBase.GTV(x, z, geometry).OpaqueSubsetsByFace);
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return false;
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        
        public override int GetFaceTextureSlot(int face, int value)
        {
            return Terrain.ExtractData(value);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }
       
    }
}
