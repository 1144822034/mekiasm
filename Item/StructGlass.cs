using Engine;
using Engine.Graphics;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class StructGlass : Item
    {
        public StructGlass(int itemid_, string displayName_) : base(itemid_, displayName_)
        {
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, color, environmentData);
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            showDebris = true;
            int dd = ILibrary.getPrimaryValue(oldValue);
            dropValues.Add(new BlockDropValue() { Value = dd, Count = 1 });
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }
        public override string GetCategory(int value)
        {
            return "通用机械-机器";
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return 39;
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(block, value, x, y, z, Color.White, SubsystemItemBlockBase.GTV(x, z, geometry).OpaqueSubsetsByFace);
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return true;
        }
    }
}
