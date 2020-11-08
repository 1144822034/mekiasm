using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Game;
namespace Mekiasm
{
    public class MekDynamicWidget:CanvasWidget
    {

        public ComponentMekDynamic mekDynamic;
        public RectangleWidget rectangleWidget = new RectangleWidget() { OutlineColor = Color.White, FillColor = Color.Gray };
        public StackPanelWidget stackMain = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };
        public StackPanelWidget stackLeft = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
        public StackPanelWidget stackRight = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
        public StackPanelWidget stackBottom = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Far };
        public StackPanelWidget stackTop = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Near };
        public LabelWidget title = new LabelWidget() { Size = new Vector2(200, 32), FontScale = 0.7f };
        public MekDynamicWidget(ComponentMekDynamic componentMekDynamic)
        {
            mekDynamic = componentMekDynamic;
            this.Size = new Vector2(300, 300);
            Children.Add(rectangleWidget);
            Children.Add(stackMain);
            Children.Add(stackTop);
            Children.Add(stackBottom);
            stackTop.Children.Add(title);
            stackMain.Children.Add(stackLeft);
            stackMain.Children.Add(stackRight);
            title.Text = "动态储罐";
            LabelWidget labelWidget = new LabelWidget() { Text="体积："+mekDynamic.maxquantity+"MB"};
            stackBottom.Children.Add(labelWidget);
        }
    }
}
