using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    public class FusionElectromagnet:Item
    {//电磁铁
        public FusionElectromagnet(int d, string n) : base(d,n) { }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 90, 90, 90, 90, 83, 83 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return ILibrary.GetPlacementValue(subsystemTerrain, componentMiner, value, raycastResult);
        }
    }
}
