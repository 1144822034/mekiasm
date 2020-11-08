using Engine;
using Engine.Graphics;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mekiasm
{
    public class SingleChestBlock:Block//储物柜
    {
        public const int Index = 1002;
        public SingleChestBlock() {
        }
        public Item GetItem(ref int value)
        {
            int id = ILibrary.getItemId(value);
            return MekiasmInit.items_chest.Where(p=>p.itemid==id).FirstOrDefault();
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int pos = ILibrary.GetDirection(value);//朝向
            int itemid = ILibrary.getItemId(value);
            switch (face)
                    {//返回基础的各个面
                        case 4: return 18+itemid;//顶面固定
                        case 5:return 17;//底面固定
                        default:
                    if (face== pos)  return (22 + itemid);  else return 17;
                    }         
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return true;
        }
        public override int GetEmittedLightAmount(int value)
        {
            return base.GetEmittedLightAmount(value);
        }
        public override int GetShadowStrength(int value)
        {
            return base.GetShadowStrength(value);
        }
        public override string GetCategory(int value)
        {
            return "通用机械-工具";
        }
        public override string GetDescription(int value)
        {
            int id = ILibrary.getItemId(value);
            switch (id) {
                case 0:return "拥有128容量的单物品储存柜";
                case 1: return "拥有256容量的单物品储存柜";
                case 2: return "拥有512容量的单物品储存柜";
                case 3: return "拥有1024容量的单物品储存柜";
                default:return "";
            }

        }
        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
        {
            return ILibrary.GetPlacementValue(subsystemTerrain,componentMiner,value,raycastResult);
        }
        public static CraftingRecipe makeCraft(string craft,string desc,int result,int count)
        {
            CraftingRecipe craftingRecipe = new CraftingRecipe();
            List<string> ins = new List<string>();
            string[] mm = craft.Split(new char[] { '|'},StringSplitOptions.RemoveEmptyEntries);
            if (mm.Length == 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    ins.Add(mm[i]);
                }
                craftingRecipe.Ingredients = ins.ToArray();
            }
            else {
                for (int i = 0; i < 9; i++)
                {
                    ins.Add("none");
                }
                craftingRecipe.Ingredients = ins.ToArray();
            }
            craftingRecipe.Description = desc;
            craftingRecipe.ResultValue = result;
            craftingRecipe.ResultCount = count;
            return craftingRecipe;
        }
        public override IEnumerable<CraftingRecipe> GetProceduralCraftingRecipes()
        {
            string basecraft = "cobblestone|cobblestone|cobblestone|cobblestone|chest|cobblestone|cobblestone|cobblestone|cobblestone";
            string pricraft = "chest|chest|chest|chest|SingleChestBlock:0|chest|chest|chest|chest";
            string advancecraft = "chest|chest|chest|chest|SingleChestBlock:1|chest|chest|chest|chest";
            string bestcraft = "chest|chest|chest|chest|SingleChestBlock:2|chest|chest|chest|chest";
            string desca = "用石头和箱子来制作基础储物柜";
            string descb = "用石头和基础储物柜来制作初级储物柜";
            string descc = "用箱子和初级储物柜来制作高级储物柜";
            string descd = "用箱子和高级储物柜来制作终极储物柜";
            List<CraftingRecipe> rcps = new List<CraftingRecipe>();
            rcps.Add(makeCraft(basecraft, desca,Terrain.MakeBlockValue(Index,0,0),1));
            rcps.Add(makeCraft(pricraft, descb, Terrain.MakeBlockValue(Index, 0, 1), 1));
            rcps.Add(makeCraft(advancecraft, descc, Terrain.MakeBlockValue(Index, 0, 2), 1));
            rcps.Add(makeCraft(bestcraft, descd, Terrain.MakeBlockValue(Index, 0, 3), 1));
            return rcps.ToArray();
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            GetItem(ref value).GenerateTerrainVertices(this, generator, geometry, value, x, y, z);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return GetItem(ref value).DisplayName;
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            showDebris = true;
            dropValues.Add(new BlockDropValue() { Value = ILibrary.setDirection(oldValue, 0), Count = 1 }) ;
        }
        public override BlockPlacementData GetDigValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, int toolValue, TerrainRaycastResult raycastResult)
        {
            BlockPlacementData result = default(BlockPlacementData);          
            result.CellFace = raycastResult.CellFace;
            return result;
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            GetItem(ref value).DrawBlock(primitivesRenderer, value, color, size, ref matrix, environmentData);
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            int[] arr = new int[MekiasmInit.items_chest.Count];
            for (int i = 0; i < MekiasmInit.items_chest.Count; i++)
            {
                arr[i] = Terrain.MakeBlockValue(1002, 0, MekiasmInit.items_chest.Array[i].itemid);
            }
            return arr;
        }
    }
}
