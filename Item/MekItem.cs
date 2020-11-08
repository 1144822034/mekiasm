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
    public abstract class Item
    {//模拟block

        public int itemid;
        public string DisplayName;

        public Item() {
        }
        public Item(int id, string name) {
            itemid = id;
            DisplayName = name;
        }
        public virtual void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, Color.White, Color.White, environmentData);
        }
        public virtual void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(block, value, x, y, z, Color.White, SubsystemItemBlockBase.GTV(x, z, geometry).TransparentSubsetsByFace);
        }
        public virtual bool ShouldGenerateFace(SubsystemTerrain subsystemTerrain, int face, int value, int neighborValue)
        {
            return true;
        }
        public virtual ElectricElement CreateElectricElement(SubsystemElectricity subsystemElectricity, int value, int x, int y, int z){
            return null;
        }

        public virtual void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            showDebris = true;
            int dd = ILibrary.setDirection(oldValue,0);
            dd = ILibrary.getPrimaryValue(dd);
            dropValues.Add(new BlockDropValue() {Value= dd,Count=1 });
        }
        public virtual ElectricConnectorType? GetConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z)
        {//设置输入输出模式
            return ElectricConnectorType.InputOutput;
        }

        public virtual int GetConnectionMask(int value)
        {
            return int.MaxValue;
        }
        public virtual BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            BlockPlacementData result = default;
            result.Value = value;
            result.CellFace = raycastResult.CellFace;
            return result;
        }
        public virtual BlockPlacementData GetDigValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, int toolValue, TerrainRaycastResult raycastResult)
        {
            BlockPlacementData result = default;
            result.CellFace = raycastResult.CellFace;
            return result;
        }
        public virtual string GetCategory(int value)
        {
            return "Terrain";
        }

        public virtual bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {          
            return false;
        }
        public virtual BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, 1f, Color.White, GetFaceTextureSlot(4, value));
        }

        public virtual bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return false;
        }
        public virtual string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }
        public virtual int GetFaceTextureSlot(int face, int value)
        {
            return 0;
        }
        public virtual BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return Game.Block.m_defaultCollisionBoxes;
        }


    }
}

