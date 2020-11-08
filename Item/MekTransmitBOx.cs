using Game;
namespace Mekiasm
{
    class MekTransmitBox:ItemCube
    {
        public MekTransmitBox(int d,string n):base(d,n) { }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return false;
        }
    }
}
