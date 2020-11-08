using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;
using Game;
//粉碎机
namespace Mekiasm
{
    public class MekSmeltWidget : BaseMachineWiget
    {
        public ComponentSmelt componentSmelt;
        public LabelWidget labelWidget = new LabelWidget() { Size = new Engine.Vector2(100, 32), FontScale = 0.5f };
        public MekSmeltWidget(ComponentSmelt componentSmelt_)
        {
            title.Text = "充能冶炼炉";
            componentSmelt = componentSmelt_;
            InventorySlotWidget inventorySlota = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64) };
            InventorySlotWidget inventorySlotb = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64), Margin = new Engine.Vector2(10, 0) };
            inventorySlota.AssignInventorySlot(componentSmelt, 0);
            inventorySlotb.AssignInventorySlot(componentSmelt, 1);
            stackLeft.Children.Add(inventorySlota);
            stackRight.Children.Add(inventorySlotb);
            stackBottom.Children.Add(labelWidget);
        }

        public override void Update()
        {
            labelWidget.Text = "电量:" + componentSmelt.quantity;
            EnergybarWidget.Value = (float)componentSmelt.quantity / (float)componentSmelt.maxquantity;
            barWidget.Value = ((float)componentSmelt.per) / 100;

        }

    }
}
