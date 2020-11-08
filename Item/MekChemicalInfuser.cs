using Game;
namespace Mekiasm
{
    class MekChemicalInfuser:Item
    {
        public MekChemicalInfuser(int d,string n) : base(d,n) { }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 108, 93, 94, 92, 110, 77 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }


    }
}
