using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class ItemCrusher:Item
    {
        public ItemCrusher(int id_,string name_) : base(id_,name_) { }        
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 63, 79, 47, 79, 95, 95 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return ILibrary.GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }
    }
}
