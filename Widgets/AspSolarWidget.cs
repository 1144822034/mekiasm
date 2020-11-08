using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Engine.Graphics;

namespace Mekiasm
{
    public class AspSolarWidget:BaseEnergyWidget
    {
        public CptEgySolarGtor cptEgySolarGtor;
        public AspSolarWidget(CptEgySolarGtor cptEgySolar) {
            cptEgySolarGtor = cptEgySolar;
        }
        public override void Update()
        {
            barWidget.Value= (((float)cptEgySolarGtor.quantity / (float)cptEgySolarGtor.maxquantity));
            labelWidget1.Text = $"储存：{cptEgySolarGtor.quantity} eu";
            labelWidget2.Text = $"总量：{cptEgySolarGtor.maxquantity} eu";
            labelWidget3.Text = $"模式：{cptEgySolarGtor.mode}";

        }
    }
}
