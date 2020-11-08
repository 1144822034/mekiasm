using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{
    public class ComponentSingleChest:ComponentInventoryBase,IInventory
    {
        public override int SlotsCount => 1;
  
        public override int ActiveSlotIndex { get => 0; set => base.ActiveSlotIndex = value; }


        public override int GetSlotCapacity(int slotIndex, int value)
        {
            return 8092;
        }
        public override int GetSlotProcessCapacity(int slotIndex, int value)
        {
            return 8092;
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
          
        }
        public override void AddSlotItems(int slotIndex, int value, int count)
        {
            Slot slot = m_slots[slotIndex];
            slot.Value = value;
            slot.Count += count;
        }
        public void Droptems(Vector3 position,int dropcount)
        {
            SubsystemPickables subsystemPickables = base.Project.FindSubsystem<SubsystemPickables>(throwOnError: true);
            if (dropcount > SlotsCount) dropcount = SlotsCount;
            for (int i = 0; i < dropcount; i++)
            {   
                    int slotValue = GetSlotValue(i);
                    int count = RemoveSlotItems(i, dropcount);
                    Vector3 value = m_random.UniformFloat(5f, 10f) * Vector3.Normalize(new Vector3(m_random.UniformFloat(-1f, 1f), m_random.UniformFloat(1f, 2f), m_random.UniformFloat(-1f, 1f)));
                    subsystemPickables.AddPickable(slotValue, count, position, value, null);                
            }
        }
        public override void ProcessSlotItems(int slotIndex, int value, int count, int processCount, out int processedValue, out int processedCount)
        {
            base.ProcessSlotItems(slotIndex, value, count, processCount, out processedValue, out processedCount);
        }
        public override int RemoveSlotItems(int slotIndex, int count)
        {
            return base.RemoveSlotItems(slotIndex, count);
        }
        public override int GetSlotCount(int slotIndex)
        {
            return base.GetSlotCount(slotIndex);
        }
        public override int GetSlotValue(int slotIndex)
        {
            return base.GetSlotValue(slotIndex);
        }
       
        
    }
}
