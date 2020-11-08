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
    public class EnrichWidget : BaseMachineWiget
    {
        public ComponentEnrich componentEnrich;
        public LabelWidget labelWidget = new LabelWidget(){ Size = new Engine.Vector2(100, 32), FontScale = 0.5f };
        public EnrichWidget(ComponentEnrich componentEnrich_)
        {
            title.Text = "富集仓";
            componentEnrich = componentEnrich_;
            InventorySlotWidget inventorySlota = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64) };
            InventorySlotWidget inventorySlotb = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64), Margin = new Engine.Vector2(10, 0) };
            inventorySlota.AssignInventorySlot(componentEnrich, 0);
            inventorySlotb.AssignInventorySlot(componentEnrich, 1);
            stackLeft.Children.Add(inventorySlota);
            stackRight.Children.Add(inventorySlotb);
            stackBottom.Children.Add(labelWidget);
        }

        public override void Update()
        {
            labelWidget.Text = "电量:" + componentEnrich.quantity ;
            EnergybarWidget.Value = (float)componentEnrich.quantity / (float)componentEnrich.maxquantity;
            barWidget.Value = ((float)componentEnrich.per) / 100;
        }

    }
}
