using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{
    public class ComponentCoalGenerator : ComponentEnergyMachine, IUpdateable
    {
        public int UpdateOrder => 50;
        public int fuelTime = 0;
        public int fullfuelTime = 0;
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            fuelTime = valuesDictionary.GetValue<int>("fuelTime");
            fullfuelTime = valuesDictionary.GetValue<int>("fullfuelTime");
            
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
            valuesDictionary.SetValue<int>("fuelTime",fuelTime);
            valuesDictionary.SetValue<int>("fullfuelTime",fullfuelTime);
        }
        public override int GetSlotCapacity(int slotIndex, int value)
        {          
            int id = Terrain.ExtractContents(value);
            if (id == 22 || id == 150) { return BlocksManager.Blocks[id].MaxStacking; }
            return 0;
        }
        public void Update(float dt)
        {
            if (m_slots[0].Count > 0)
            {
                int blockid = Terrain.ExtractContents(m_slots[0].Value);
                if (fuelTime <= 0)
                {
                    if (blockid == 22)
                    {
                        //22小煤块
                        //150煤方块
                        this.fuelTime += 600;//设定600
                        fullfuelTime = 600;
                    }
                    else if (blockid == 150)
                    {
                       
                        this.fuelTime += 5400;//煤方块燃料时间设定5400
                        fullfuelTime = 5400;
                    }
                    RemoveSlotItems(0, 1);//移除一个
                }

            }
            if (fuelTime > 0) { //燃料时间>0
                if (quantity < maxquantity) {
                    quantity += 10;
                    --fuelTime;
                }
            }
        }
    }
}

