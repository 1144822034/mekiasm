using System;
using System.Collections.Generic;
using System.Globalization;
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
	public abstract class ComponentFluidStore : Component, IInventory
	{
		public int mode = 0;
		public int quantity = 0;
		public int maxquantity = 0;
		public SubsystemTimeOfDay subsystemTimeOfDay;
		public SubsystemBlockEntities subsystemBlockEntities;

		public class Slot
		{
			public int Value;

			public int Count;
		}

		public List<Slot> m_slots = new List<Slot>();

		public Game.Random m_random = new Game.Random();

		Project IInventory.Project => base.Project;

		public virtual int SlotsCount => m_slots.Count;

		public virtual int ActiveSlotIndex
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}
		public float CalculateVolume(float distance, float minDistance, float rolloffFactor = 2f)
		{
			if (distance > minDistance)
			{
				return minDistance / (minDistance + MathUtils.Max(rolloffFactor * (distance - minDistance), 0f));
			}
			return 1f;
		}
		public static int FindAcquireSlotForItem(IInventory inventory, int value)
		{
			for (int i = 0; i < inventory.SlotsCount; i++)
			{
				if (inventory.GetSlotCount(i) > 0 && inventory.GetSlotValue(i) == value && inventory.GetSlotCount(i) < inventory.GetSlotCapacity(i, value))
				{
					return i;
				}
			}
			for (int j = 0; j < inventory.SlotsCount; j++)
			{
				if (inventory.GetSlotCount(j) == 0 && inventory.GetSlotCapacity(j, value) > 0)
				{
					return j;
				}
			}
			return -1;
		}

		public static int AcquireItems(IInventory inventory, int value, int count)
		{
			while (count > 0)
			{
				int num = FindAcquireSlotForItem(inventory, value);
				if (num < 0)
				{
					break;
				}
				inventory.AddSlotItems(num, value, 1);
				count--;
			}
			return count;
		}
		public override void Dispose()
		{
			base.Dispose();
		}
		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			subsystemTimeOfDay = Project.FindSubsystem<SubsystemTimeOfDay>();
			subsystemBlockEntities = Project.FindSubsystem<SubsystemBlockEntities>();
			quantity = valuesDictionary.GetValue<int>("Quantity");
			maxquantity = valuesDictionary.GetValue<int>("MaxQuantity");
			mode = valuesDictionary.GetValue<int>("Mode");
			int value = valuesDictionary.GetValue<int>("SlotsCount");
			for (int i = 0; i < value; i++)
			{
				m_slots.Add(new Slot());
			}
			ValuesDictionary value2 = valuesDictionary.GetValue<ValuesDictionary>("Slots");
			for (int j = 0; j < m_slots.Count; j++)
			{
				ValuesDictionary value3 = value2.GetValue<ValuesDictionary>("Slot" + j.ToString(CultureInfo.InvariantCulture), null);
				if (value3 != null)
				{
					Slot slot = m_slots[j];
					slot.Value = value3.GetValue<int>("Contents");
					slot.Count = value3.GetValue<int>("Count");
				}
			}
		}

		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			ValuesDictionary valuesDictionary2 = new ValuesDictionary();
			valuesDictionary.SetValue("Slots", valuesDictionary2);
			valuesDictionary.SetValue("Quantity", quantity);
			valuesDictionary.SetValue("MaxQuantity", maxquantity);
			valuesDictionary.SetValue("Mode", mode);
			for (int i = 0; i < m_slots.Count; i++)
			{
				Slot slot = m_slots[i];
				if (slot.Count > 0)
				{
					ValuesDictionary valuesDictionary3 = new ValuesDictionary();
					valuesDictionary2.SetValue("Slot" + i.ToString(CultureInfo.InvariantCulture), valuesDictionary3);
					valuesDictionary3.SetValue("Contents", slot.Value);
					valuesDictionary3.SetValue("Count", slot.Count);
				}
			}
		}

		public virtual int GetSlotValue(int slotIndex)
		{
			if (slotIndex >= 0 && slotIndex < m_slots.Count)
			{
				if (m_slots[slotIndex].Count <= 0)
				{
					return 0;
				}
				return m_slots[slotIndex].Value;
			}
			return 0;
		}

		public virtual int GetSlotCount(int slotIndex)
		{
			if (slotIndex >= 0 && slotIndex < m_slots.Count)
			{
				return m_slots[slotIndex].Count;
			}
			return 0;
		}

		public virtual int GetSlotCapacity(int slotIndex, int value)
		{
			if (slotIndex >= 0 && slotIndex < m_slots.Count)
			{
				return BlocksManager.Blocks[Terrain.ExtractContents(value)].MaxStacking;
			}
			return 0;
		}

		public virtual int GetSlotProcessCapacity(int slotIndex, int value)
		{
			int slotCount = GetSlotCount(slotIndex);
			int slotValue = GetSlotValue(slotIndex);
			if (slotCount > 0 && slotValue != 0)
			{
				SubsystemBlockBehavior[] blockBehaviors = base.Project.FindSubsystem<SubsystemBlockBehaviors>(throwOnError: true).GetBlockBehaviors(Terrain.ExtractContents(slotValue));
				for (int i = 0; i < blockBehaviors.Length; i++)
				{
					int processInventoryItemCapacity = blockBehaviors[i].GetProcessInventoryItemCapacity(this, slotIndex, value);
					if (processInventoryItemCapacity > 0)
					{
						return processInventoryItemCapacity;
					}
				}
			}
			return 0;
		}

		public virtual void AddSlotItems(int slotIndex, int value, int count)
		{
			if (count > 0 && slotIndex >= 0 && slotIndex < m_slots.Count)
			{
				Slot slot = m_slots[slotIndex];
				if ((GetSlotCount(slotIndex) != 0 && GetSlotValue(slotIndex) != value) || GetSlotCount(slotIndex) + count > GetSlotCapacity(slotIndex, value))
				{
					throw new InvalidOperationException("Cannot add slot items.");
				}
				slot.Value = value;
				slot.Count += count;
			}
		}

		public virtual void ProcessSlotItems(int slotIndex, int value, int count, int processCount, out int processedValue, out int processedCount)
		{
			int slotCount = GetSlotCount(slotIndex);
			int slotValue = GetSlotValue(slotIndex);
			if (slotCount > 0 && slotValue != 0)
			{
				SubsystemBlockBehavior[] blockBehaviors = base.Project.FindSubsystem<SubsystemBlockBehaviors>(throwOnError: true).GetBlockBehaviors(Terrain.ExtractContents(slotValue));
				foreach (SubsystemBlockBehavior subsystemBlockBehavior in blockBehaviors)
				{
					int processInventoryItemCapacity = subsystemBlockBehavior.GetProcessInventoryItemCapacity(this, slotIndex, value);
					if (processInventoryItemCapacity > 0)
					{
						subsystemBlockBehavior.ProcessInventoryItem(this, slotIndex, value, count, MathUtils.Min(processInventoryItemCapacity, processCount), out processedValue, out processedCount);
						return;
					}
				}
			}
			processedValue = value;
			processedCount = count;
		}

		public virtual int RemoveSlotItems(int slotIndex, int count)
		{
			if (slotIndex >= 0 && slotIndex < m_slots.Count)
			{
				Slot slot = m_slots[slotIndex];
				count = MathUtils.Min(count, GetSlotCount(slotIndex));
				slot.Count -= count;
				return count;
			}
			return 0;
		}

		public void DropAllItems(Vector3 position)
		{
			SubsystemPickables subsystemPickables = base.Project.FindSubsystem<SubsystemPickables>(throwOnError: true);
			for (int i = 0; i < SlotsCount; i++)
			{
				int slotCount = GetSlotCount(i);
				if (slotCount > 0)
				{
					int slotValue = GetSlotValue(i);
					int count = RemoveSlotItems(i, slotCount);
					Vector3 value = m_random.UniformFloat(5f, 10f) * Vector3.Normalize(new Vector3(m_random.UniformFloat(-1f, 1f), m_random.UniformFloat(1f, 2f), m_random.UniformFloat(-1f, 1f)));
					subsystemPickables.AddPickable(slotValue, count, position, value, null);
				}
			}


		}
	}
}
