using Game;
using Engine;
using Engine.Graphics;
using TemplatesDatabase;
using System;

namespace Mekiasm
{
    class SubsystemAtomicDecomposerBehavior : SubsystemBlockBehavior
    {
        public override int[] HandledBlocks => new int[] {1008 };
        public SubsystemCreatureSpawn subsystemCreatureSpawn;

        public int UpdateOrder => 999;

        public override void Load(ValuesDictionary valuesDictionary)
        {
            subsystemCreatureSpawn = Project.FindSubsystem<SubsystemCreatureSpawn>();
            base.Load(valuesDictionary);
        }
       
        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {
              subsystemCreatureSpawn.SpawnCreature("Duck",start+new Vector3(1,1,1),true);
              return false;
        }

        public void Update(float dt)
        {
        }
    }
}
