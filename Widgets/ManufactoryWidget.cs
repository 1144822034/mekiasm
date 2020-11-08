using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Mekiasm
{
    public class ManufactoryWidget : BaseMachineWiget
    {
        public ComponentManufactory componentManufactory;
        public LabelWidget labelWidget = new LabelWidget() { Size = new Engine.Vector2(100, 32), FontScale = 0.5f };
        public ManufactoryWidget(ComponentManufactory componentManufactory_)
        {
            title.Text = "制造厂";
            componentManufactory = componentManufactory_;
            InventorySlotWidget inventorySlota = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64) };
            InventorySlotWidget inventorySlotb = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64), Margin = new Engine.Vector2(10, 0) };
            inventorySlota.AssignInventorySlot(componentManufactory, 0);
            inventorySlotb.AssignInventorySlot(componentManufactory, 1);
            stackLeft.Children.Add(inventorySlota);
            stackRight.Children.Add(inventorySlotb);
            stackBottom.Children.Add(labelWidget);
        }

        public override void Update()
        {
           labelWidget.Text = "电量:" + componentManufactory.quantity ;
            EnergybarWidget.Value = (float)componentManufactory.quantity / (float)componentManufactory.maxquantity;
             barWidget.Value = ((float)componentManufactory.per) / 100;
        }

    }
}

