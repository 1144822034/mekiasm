using Game;
namespace Mekiasm
{
    class MekCombiner:Item
    {
        public MekCombiner(int d,string n) : base(d,n) { }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 107, 93, 94, 92, 109, 77 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
    }
}
