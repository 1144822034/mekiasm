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
    public class ItemElectricBlock : Block,IElectricElementBlock,IPaintableBlock
    {
        public const int Index = 1003;
        public ItemElectricBlock()
        {
            
        }
        public virtual Item GetItem(ref int value)
        {
            int id = Terrain.ExtractData(value);
            id = ILibrary.getItemId(value);
            return MekiasmInit.items_electric.Where(p => p.itemid == id).FirstOrDefault();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            GetItem(ref value).DrawBlock(primitivesRenderer, value, color, size, ref matrix, environmentData);
        }
        public override float GetIconViewScale(int value, DrawBlockEnvironmentData environmentData)
        {
            return base.GetIconViewScale(value, environmentData);
        }
        public override BoundingBox[] GetCustomInteractionBoxes(SubsystemTerrain terrain, int value)
        {
            return base.GetCustomInteractionBoxes(terrain, value);
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
            return GetItem(ref value).IsFaceTransparent(subsystemTerrain,face,value); ;
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            int data = Terrain.ExtractData(value);
            switch (data) {
                case 40:return false;
                case 41:return false;
                default:return GetItem(ref value).IsInteractive(subsystemTerrain,value);
            }
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            GetItem(ref oldValue).GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
        }
        public override string GetCategory(int value)
        {
            return "通用机械-机器";
        }
        
        public override BlockPlacementData GetDigValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, int toolValue, TerrainRaycastResult raycastResult)
        {
            return GetItem(ref value).GetDigValue(subsystemTerrain, componentMiner, value, toolValue, raycastResult);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            int value2 = value;
            return GetItem(ref value2).GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            int[] arr = new int[MekiasmInit.items_electric.Count];
            for (int i = 0; i < MekiasmInit.items_electric.Count; i++)
            {
                arr[i] = Terrain.MakeBlockValue(1003, 0, MekiasmInit.items_electric.Array[i].itemid);
            }
            return arr;
        }

        public ElectricElement CreateElectricElement(SubsystemElectricity subsystemElectricity, int value, int x, int y, int z)
        {//创建电子元件元素
            return GetItem(ref value).CreateElectricElement(subsystemElectricity,value,x,y,z);
        }
        public ElectricConnectorType? GetConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z)
        {//设置输入输出模式
            return GetItem(ref value).GetConnectorType(terrain,value,face,connectorFace,x,y,z);
        }

        public int GetConnectionMask(int value)
        {
            return GetItem(ref value).GetConnectionMask(value); ;
        }

        public int? GetPaintColor(int value)
        {
            return 0;
        }
        public int Paint(SubsystemTerrain subsystemTerrain, int value, int? color)
        {
            return 0;
        }
    }
}
