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
    public class PanelWidget:CanvasWidget
    {
        public ScrollPanelWidget scrollPanelWidget = new ScrollPanelWidget() { Direction = LayoutDirection.Vertical };
        public StackPanelWidget stackPanelWidget = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
        public ImageWidget rectangleWidget = new ImageWidget() {DesiredSize=new Vector2(700,400)  };
        public LabelWidget labelWidget = new LabelWidget() { Size=new Vector2(700,400),WordWrap=true};

        public PanelWidget() {
            MekiasmInit.imgres.TryGetValue("Machinebgtexture", out Texture2D bgtexture);
            rectangleWidget.Texture = bgtexture;
            Size = new Vector2(700,400);
            HorizontalAlignment = WidgetAlignment.Center;
            VerticalAlignment = WidgetAlignment.Center;
            Children.Add(rectangleWidget);
            Children.Add(scrollPanelWidget);
            scrollPanelWidget.Children.Add(stackPanelWidget);
            stackPanelWidget.Children.Add(labelWidget);
        }
        public void setText(string str) {
            labelWidget.Text = str;
        }
        public void add(Widget widget) {           
            stackPanelWidget.Children.Add(widget);
        }
        public void clear() {
            while (stackPanelWidget.Children.Count>0) {
                stackPanelWidget.Children.Remove(stackPanelWidget.Children[0]);
            }            
        }
    }
}
