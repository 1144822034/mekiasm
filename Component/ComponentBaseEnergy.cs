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
    public class ComponentBaseEnergy: ComponentEnergyMachine, IUpdateable
    {//基础能量立方
        public int UpdateOrder => 50;        
      
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {           
           base.Load(valuesDictionary, idToEntityMap);
        }

        


        public override void Dispose()
        {
            base.Dispose();
           
        }
        public void Update(float dt)
        {
            getEnergy(32);
        }
    }
}
