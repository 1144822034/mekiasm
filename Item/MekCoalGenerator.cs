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
    public class MekCoalGenerator : Item
    {
        public MekCoalGenerator(int itemid_, string displayName_) : base(itemid_, displayName_)
        {
        }      
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 27, 26, 26, 26, 26, 26 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return ILibrary.GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }        

        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }

    }
}
