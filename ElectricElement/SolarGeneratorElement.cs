using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    public class SolarGeneratorElement:MyElectricElement
    {
        public SolarGeneratorElement(SubsystemElectricity subsystemElectricity,CellFace cellFace) : base(subsystemElectricity,cellFace) { }
	}
}
