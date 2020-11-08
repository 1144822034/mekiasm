using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{//太阳能发电的component
    public class CptEgySolarGtor : ComponentEnergyMachine, IUpdateable
    {
        public int UpdateOrder => 50;

        public void Update(float dt)
        {//白天自动加光
            if (subsystemTimeOfDay.TimeOfDay > 0.25f && subsystemTimeOfDay.TimeOfDay < 0.75f)
            {                
                if (quantity < maxquantity) quantity += 32;
            }
        }
    }
}
