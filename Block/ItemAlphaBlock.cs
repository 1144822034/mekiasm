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
    public class ItemAlphaBlock:Block
    {
        public const int Index = 1007;
        public virtual Item GetItem(ref int value)
        {
            int id = Terrain.ExtractData(value);
            if (Terrain.ExtractContents(value) != 1007) return null;
            return MekiasmInit.items_alpha.Where(p => p.itemid == id).FirstOrDefault();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            GetItem(ref value).DrawBlock(primitivesRenderer, value, color, size, ref matrix, environmentData);
        }
        public override bool ShouldGenerateFace(SubsystemTerrain subsystemTerrain, int face, int value, int neighborValue)
        {
            return GetItem(ref value).ShouldGenerateFace(subsystemTerrain, face, value, neighborValue);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return GetItem(ref value).GetDisplayName(subsystemTerrain, value);
        }
        public override BlockPlacementData GetDigValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, int toolValue, TerrainRaycastResult raycastResult)
        {
            return GetItem(ref value).GetDigValue(subsystemTerrain, componentMiner, value, toolValue, raycastResult);
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            int value = oldValue;
           GetItem(ref value).GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            GetItem(ref value).GenerateTerrainVertices(this,generator, geometry, value, x, y, z);
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return GetItem(ref value).GetFaceTextureSlot(face,value);
        }
        public override string GetCategory(int value)
        {
            return "通用机械-机器";
        }
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return GetItem(ref value).GetCustomCollisionBoxes(terrain, value);
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            int[] arr = new int[MekiasmInit.items_alpha.Count];
            for (int i = 0; i < MekiasmInit.items_alpha.Count; i++)
            {
                arr[i] = Terrain.MakeBlockValue(1007, 0, MekiasmInit.items_alpha.Array[i].itemid);
            }
            return arr;
        }

    }
}
