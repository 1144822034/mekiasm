using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Engine;
using Engine.Graphics;

namespace Mekiasm
{
    public class CoalGeneratorWidget:BaseEnergyWidget
    {
        public ComponentCoalGenerator generator;
        public CoalGeneratorWidget(ComponentCoalGenerator componentCoalGenerator) {
            generator = componentCoalGenerator;
            InventorySlotWidget inventory = new InventorySlotWidget() { Size=new Vector2(48,48)};
            inventory.AssignInventorySlot(componentCoalGenerator,0);
            stackPanelLeft.HorizontalAlignment = WidgetAlignment.Far;
            stackPanelLeft.Children.Add(inventory);
        }
        public override void Update()
        {
            barWidget.Value = (float)generator.quantity / (float)generator.maxquantity;
            labelWidget1.Text = $"储存电量:{generator.quantity}";
            labelWidget2.Text = $"最大电量:{generator.maxquantity}";
            labelWidget3.Text = $"燃料时间:{generator.fuelTime}";
        }

    }
}
