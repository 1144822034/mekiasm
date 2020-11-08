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
    public class BaseMachineWiget:CanvasWidget
    {
        public RectangleWidget rectangleWidget = new RectangleWidget() { OutlineColor=Color.White,FillColor=Color.Gray};
        public StackPanelWidget stackMain = new StackPanelWidget() {Direction=LayoutDirection.Horizontal,HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Center };
        public StackPanelWidget stackLeft = new StackPanelWidget() { Direction=LayoutDirection.Vertical,VerticalAlignment=WidgetAlignment.Center};
        public StackPanelWidget stackRight = new StackPanelWidget() { Direction = LayoutDirection.Vertical,VerticalAlignment=WidgetAlignment.Center };
        public StackPanelWidget stackBottom = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Far };
        public StackPanelWidget stackTop = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Near };
        public LabelWidget title = new LabelWidget() { Size=new Vector2(200,32),FontScale=0.7f};
        public ValueBarWidget barWidget = new ValueBarWidget() { BarsCount = 8, LayoutDirection = LayoutDirection.Horizontal, LitBarColor = Color.White, TextureLinearFilter = true, BarSize = new Vector2(12, 12), VerticalAlignment = WidgetAlignment.Center };
        public Subtexture customSubtexture;
        public ValueBarWidget EnergybarWidget = new ValueBarWidget() { Margin = new Vector2(15, 20), BarsCount = 100, LayoutDirection = LayoutDirection.Vertical, LitBarColor = Color.White, BarSize = new Vector2(8, 1), FlipDirection = true };

        public BaseMachineWiget() {
            this.Size = new Vector2(300,300);
            customSubtexture =new Subtexture( ContentManager.Get<Texture2D>("Mekiasm/Textures/ProcessCell"),Vector2.Zero,Vector2.One);
            barWidget.BarSubtexture = TextureAtlasManager.GetSubtexture("Textures/Atlas/ProgressBar");
            EnergybarWidget.BarSubtexture = customSubtexture;
            Children.Add(rectangleWidget);
            Children.Add(stackMain);
            Children.Add(stackTop);
            Children.Add(stackBottom);
            stackTop.Children.Add(title);
            stackMain.Children.Add(EnergybarWidget);
            stackMain.Children.Add(stackLeft);
            stackMain.Children.Add(barWidget);
            stackMain.Children.Add(stackRight);       
        }


    }
}
