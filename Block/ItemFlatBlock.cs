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
    public class ItemFlatBlock : Block
    {
        public const int Index = 1006;
        public ItemFlatBlock()
        {
        }
        public virtual Item GetItem(ref int value)
        {
            int id = Terrain.ExtractData(value);
            if (Terrain.ExtractContents(value) != 1006) return null;
            return MekiasmInit.items_flat.Where(p => p.itemid == id).FirstOrDefault();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            GetItem(ref value).DrawBlock(primitivesRenderer, value, color, size, ref matrix, environmentData);
        }
        public override float GetIconViewScale(int value, DrawBlockEnvironmentData environmentData)
        {
            return base.GetIconViewScale(value, environmentData);
        }    
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return GetItem(ref value).GetCustomCollisionBoxes(terrain, value);
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            GetItem(ref value).GenerateTerrainVertices(this, generator, geometry, value, x, y, z);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return GetItem(ref value).GetDisplayName(subsystemTerrain, value);
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return GetItem(ref value).GetFaceTextureSlot(face, value);
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return GetItem(ref value).IsFaceTransparent(subsystemTerrain, face, value);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return GetItem(ref value).IsInteractive(subsystemTerrain, value);
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            GetItem(ref oldValue).GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
        }
        public override string GetCategory(int value)
        {
            return "Items";
        }
        public override BlockPlacementData GetDigValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, int toolValue, TerrainRaycastResult raycastResult)
        {
            return GetItem(ref value).GetDigValue(subsystemTerrain, componentMiner, value, toolValue, raycastResult);
        }
        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return GetItem(ref value).CreateDebrisParticleSystem(subsystemTerrain, position, value, strength);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return GetItem(ref value).GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            int[] arr = new int[MekiasmInit.items_flat.Count];
            for (int i = 0; i < MekiasmInit.items_flat.Count; i++)
            {
                arr[i] = Terrain.MakeBlockValue(1006, 0, MekiasmInit.items_flat.Array[i].itemid);
            }
            return arr;
        }

    }
}

