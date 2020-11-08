using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class ItemEnergyBase:Item
    {
        public ItemEnergyBase(int id_,string name_):base(id_,name_) { }
        public override int GetFaceTextureSlot(int face, int value)
        {
            switch (face) {
                case 4:return 64;
                case 5: return 64;
                default:return 65;
            }
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
    }
}
