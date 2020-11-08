using Engine;
using Game;
using Engine.Graphics;
//粉碎机
namespace Mekiasm
{
    public class CrusherWidget:BaseMachineWiget
    {
        public ComponentCrusher componentCrusher;
        public LabelWidget labelWidget = new LabelWidget() { Size = new Engine.Vector2(100, 32), FontScale = 0.5f };
        public StackPanelWidget stackPanel = new StackPanelWidget() { Direction=LayoutDirection.Horizontal};
        public CrusherWidget(ComponentCrusher componentCrusher_) {
            title.Text = "粉碎机";
            componentCrusher = componentCrusher_;
            InventorySlotWidget inventorySlota = new InventorySlotWidget() { Size=new Engine.Vector2(64,64)};
            InventorySlotWidget inventorySlotb = new InventorySlotWidget() { Size = new Engine.Vector2(64, 64),Margin=new Engine.Vector2(10,0) };
            inventorySlota.AssignInventorySlot(componentCrusher, 0);
            inventorySlotb.AssignInventorySlot(componentCrusher, 1);
            stackPanel.Children.Add(inventorySlota);
            stackLeft.Children.Add(stackPanel);
            stackRight.Children.Add(inventorySlotb);
            stackBottom.Children.Add(labelWidget);            
        }
        
        public override void Update()
        {
            labelWidget.Text = "电量:"+componentCrusher.quantity;
            EnergybarWidget.Value = (float)componentCrusher.quantity / (float)componentCrusher.maxquantity;
            barWidget.Value = ((float)componentCrusher.per)/100;
        }

    }
}
