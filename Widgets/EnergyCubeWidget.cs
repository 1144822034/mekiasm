using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Graphics;
using Game;
namespace Mekiasm
{
     public class EnergyCubeWidget:BaseEnergyWidget
    {
        public ComponentEnergyMachine cptBaseEgy;
        public EnergyCubeWidget(ComponentEnergyMachine cpt_) {
            cptBaseEgy = cpt_;
        }
        public override void Update()
        {
            barWidget.Value = (((float)cptBaseEgy.quantity / (float)cptBaseEgy.maxquantity));
            labelWidget1.Text = $"储存：{cptBaseEgy.quantity} eu";
            labelWidget2.Text = $"总量：{cptBaseEgy.maxquantity} eu";
            labelWidget3.Text = $"模式：{cptBaseEgy.mode}";
        }
    }
}
