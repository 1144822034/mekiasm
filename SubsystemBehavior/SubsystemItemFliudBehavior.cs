using Engine;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    public class SubsystemItemFliudBehavior : SubsystemFluidBlockBehavior,IUpdateable
    {
		public Game.Random m_random = new Game.Random();

		public float m_soundVolume;

		public override int[] HandledBlocks => new int[] {1009 };
        public SubsystemItemFliudBehavior():base(BlocksManager.FluidBlocks[1009],true) { }
		public int UpdateOrder => 0;
		public new void UpdateIsTop(int value, int x, int y, int z)
		{
			
			
		}
		public new void SpreadFluid()
		{
			
		}
		public void Update(float dt)
		{
			if (base.SubsystemTime.PeriodicGameTimeEvent(0.25, 0.0))
				SpreadFluid();
			if (base.SubsystemTime.PeriodicGameTimeEvent(1.0, 0.25))
			{
				float num = float.MaxValue;
				foreach (Vector3 listenerPosition in base.SubsystemAudio.ListenerPositions)
				{
					float? num2 = CalculateDistanceToFluid(listenerPosition, 8, flowingFluidOnly: true);
					if (num2.HasValue && num2.Value < num)
						num = num2.Value;
				}
				m_soundVolume = 0.5f * base.SubsystemAudio.CalculateVolume(num, 2f, 3.5f);
			}
			base.SubsystemAmbientSounds.WaterSoundVolume = MathUtils.Max(base.SubsystemAmbientSounds.WaterSoundVolume, m_soundVolume);
		}
	}
}
