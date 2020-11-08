using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Audio;
using Engine.Media;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{
    //粉碎机
    public class ComponentEnrich : ComponentEnergyMachine, IUpdateable
    {
        public int UpdateOrder => 50;
        public int per = 0;
        public float mm = 0;
        public float controltime = 0.1f;
        public int result;
        public int resultcount;
        public string musicname = "Mekiasm/sounds/enrichmentchamber";
        public bool flag = false;
        public int thisID = Terrain.MakeBlockValue(1003, 0, 78);
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            per = valuesDictionary.GetValue<int>("Per");

        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
            valuesDictionary.SetValue<int>("Per", per);
        }

        public void Update(float dt)
        {
            getEnergy(32);
            if (m_slots[0].Count > 0)
            {
                if (quantity > 0)
                {
                    if (per == 0 && !flag)
                    {
                        for (int i = 0; i < MekCraftingRecipe.recipes.Count; i++)
                        {
                            if (MekCraftingRecipe.recipes[i].needDevice.Count() > 0) if (MekCraftingRecipe.recipes[i].needDevice[0] != thisID) continue;
                            if (MekCraftingRecipe.recipes[i].ingredians.Count() == 1)
                            {
                                if (MekCraftingRecipe.recipes[i].ingredians[0] == m_slots[0].Value)
                                {
                                    if ((m_slots[1].Value == MekCraftingRecipe.recipes[i].ResultValue && (m_slots[1].Count + MekCraftingRecipe.recipes[i].ResultCount) <= GetSlotCapacity(1, MekCraftingRecipe.recipes[i].ResultValue)) || m_slots[1].Count == 0)
                                    {
                                        result = MekCraftingRecipe.recipes[i].ResultValue;
                                        resultcount = MekCraftingRecipe.recipes[i].ResultCount;
                                        flag = true; break;
                                    }
                                }
                            }
                        }
                    }
                    else if (flag)
                    {
                        playSound(musicname, thisPosition);
                        mm = MathUtils.Min(mm + controltime * dt, 1f);//计算进度
                        if (quantity > 1)
                        {
                            quantity -= 1;
                        }
                        else
                        {//没电了
                            pauseSound();
                            return;
                        }
                        per = (int)(mm * 100);
                        if (mm == 1f)
                        {
                            RemoveSlotItems(0, 1);//移除格子
                            AddSlotItems(1, result, resultcount);
                            mm = 0f;//重置进度
                            per = 0;
                            flag = false;
                        }
                    }
                }
            }
            else if (m_slots[0].Count == 0) { mm = 0; per = 0; pauseSound(); flag = false; }
        }
    }
}
