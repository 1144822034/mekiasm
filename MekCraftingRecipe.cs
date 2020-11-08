using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    //合成谱
    public class MekCraftingRecipe
    {
        public struct MekRecipeItem {
            public string craftID;
            public int[] ingredians;
            public int[] needDevice;
            public bool requireHeat;
            public int heatLevel;
            public int ResultValue;
            public int ResultCount;
        }
        public static List<MekRecipeItem> recipes=new List<MekRecipeItem>();

        public static void addRecipe(string c,int result,int recnt,int[] d,int[] n,bool re,int h=0) {
            MekRecipeItem recipeItem = new MekRecipeItem { craftID = c, ingredians = d, needDevice = n, requireHeat = re,heatLevel=h,ResultValue=result,ResultCount=recnt };
            recipes.Add(recipeItem);
         }
        public static void initRecipes() {
            //粉碎机47
            //富集仓78
            //充能冶炼炉72
            //合金炉80
            //制造厂82
            //试用 鹅暖石->沙子
            //矿石打成粉
            //石砖变原料
            //植物变生物燃料
            addRecipe("沙子", 7, 1, new int[] { 3 }, new int[] { Terrain.MakeBlockValue(1003, 0, 47) }, false);//花岗岩
            addRecipe("沙子", 7, 2, new int[] { 4 }, new int[] { Terrain.MakeBlockValue(1003, 0, 47) }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { Terrain.MakeBlockValue(1003, 0, 47) }, false);//鹅暖石
            addRecipe("压缩碳", Terrain.MakeBlockValue(1006, 0, 501), 1, new int[] { 22 }, new int[] { Terrain.MakeBlockValue(1003, 0, 78) }, false);//小煤块
            addRecipe("石墨粉", Terrain.MakeBlockValue(1006, 0, 511), 1, new int[] { 22 }, new int[] { Terrain.MakeBlockValue(1003, 0, 82) }, false);//小煤块
            addRecipe("钢锭", Terrain.MakeBlockValue(1006, 0, 514), 1, new int[] { Terrain.MakeBlockValue(1003, 0, 512),40 }, new int[] { Terrain.MakeBlockValue(1003, 0, 80) }, false);//石墨锭，铁锭
            addRecipe("石墨粉", Terrain.MakeBlockValue(1006, 0, 511), 1, new int[] { Terrain.MakeBlockValue(1003, 0, 512) }, new int[] { Terrain.MakeBlockValue(1003, 0, 47) }, false);//石墨锭
            addRecipe("钢粉", Terrain.MakeBlockValue(1006, 0, 513), 1, new int[] { Terrain.MakeBlockValue(1003, 0, 514) }, new int[] { Terrain.MakeBlockValue(1003, 0, 47) }, false);//钢锭


            /*
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
            addRecipe("沙子", 7, 1, new int[] { 4 }, new int[] { 0 }, false);//砂岩
            addRecipe("沙子", 7, 1, new int[] { 5 }, new int[] { 0 }, false);//鹅暖石
    */
            initSmeltRecipe();
        }

        public static void initSmeltRecipe() {//增加熔炼合成谱
            addRecipe("小煤块", 22, 1, new int[] { 9 }, new int[] { Terrain.MakeBlockValue(1003, 0, 72) }, false);//橡木
            addRecipe("小煤块", 22, 1, new int[] { 10 }, new int[] { Terrain.MakeBlockValue(1003, 0, 72) }, false);//桦木
            addRecipe("小煤块", 22, 1, new int[] { 11 }, new int[] { Terrain.MakeBlockValue(1003, 0, 72) }, false);//云杉木
            addRecipe("石墨锭", Terrain.MakeBlockValue(1003, 0, 512), 1, new int[] { Terrain.MakeBlockValue(1003, 0, 511) }, new int[] { Terrain.MakeBlockValue(1003, 0, 72) }, false);//石墨粉
            addRecipe("钢锭", Terrain.MakeBlockValue(1003, 0, 514), 1, new int[] { Terrain.MakeBlockValue(1003, 0, 513) }, new int[] { Terrain.MakeBlockValue(1003, 0, 72) }, false);//石墨粉



        }

    }
}
