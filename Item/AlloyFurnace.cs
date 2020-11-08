using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    //合金炉
    public class AlloyFurnace:Item
    {
        public AlloyFurnace(int d,string n) : base(d,n) { }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 80, 81, 81, 81, 81, 81 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return ILibrary.GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }
    }
}
