using Engine;
using Game;
namespace Mekiasm
{
    public class AlloyWidget:BaseMachineWiget
    {

        public ComponentAlloy componentAlloy;
        public LabelWidget labelWidget = new LabelWidget() { Size = new Vector2(100, 32), FontScale = 0.5f };
        public AlloyWidget(ComponentAlloy componentAlloy_)
        {
            title.Text = "合金炉";
            componentAlloy = componentAlloy_;
            InventorySlotWidget inventorySlota = new InventorySlotWidget() { Size = new Vector2(48, 48) };
            InventorySlotWidget inventorySlotb = new InventorySlotWidget() { Size = new Vector2(48, 48), Margin = new Vector2(0, 10) };
            InventorySlotWidget inventorySlotc = new InventorySlotWidget() { Size = new Vector2(48, 48), Margin = new Vector2(10, 0) };
            inventorySlota.AssignInventorySlot(componentAlloy, 0);
            inventorySlotb.AssignInventorySlot(componentAlloy, 1);
            inventorySlotc.AssignInventorySlot(componentAlloy, 2);
            stackLeft.Children.Add(inventorySlota);
            stackLeft.Children.Add(inventorySlotb);
            stackRight.Children.Add(inventorySlotc);
            stackBottom.Children.Add(labelWidget);
        }

        public override void Update()
        {
            labelWidget.Text = "电量:" + componentAlloy.quantity;
            EnergybarWidget.Value = (float)componentAlloy.quantity / (float)componentAlloy.maxquantity;
            barWidget.Value = ((float)componentAlloy.per) / 100;
        }
    }
}
