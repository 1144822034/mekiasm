using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    public class SubsystemMekPlantsBehavior:SubsystemBlockBehavior,IUpdateable
    {
        public override int[] HandledBlocks => new int[] { 1008};
        public static int MaxLevel = 5;
        public int UpdateOrder => 90;

        public static int setLevel(int value,int level) {
            int data = Terrain.ExtractData(value);
            int id = data / MaxLevel;
            return Terrain.ReplaceData(value,id*5+level);
        }
        public static int getLevel(int value) {
            return Terrain.ExtractData(value) % MaxLevel;
        }
        public static int getPlantID(int value) {
            return Terrain.ExtractData(value) / MaxLevel;
        }
        public void Update(float dt)
        {

        }
    }
}
