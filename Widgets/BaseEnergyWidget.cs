using Game;
using Engine;
using Engine.Graphics;

namespace Mekiasm
{
    public abstract class BaseEnergyWidget:CanvasWidget
    {
        public CanvasWidget canvasWidgetc = new CanvasWidget() { Size = new Vector2(300, 160) };
        public int percent = 0;
        public StackPanelWidget stackmain = new StackPanelWidget() {Direction=LayoutDirection.Horizontal,HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Center };
        public StackPanelWidget stackbg = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };
        public StackPanelWidget stackpro = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
        public StackPanelWidget stackPanelCenter= new StackPanelWidget() { Direction = LayoutDirection.Vertical ,VerticalAlignment=WidgetAlignment.Far,HorizontalAlignment=WidgetAlignment.Center};
        public StackPanelWidget stackPanelLeft = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
        public LabelWidget labelWidget1 = new LabelWidget() { Text = "格子1", Size = new Vector2(64, 18),FontScale=0.5f ,Color=Color.Green};
        public LabelWidget labelWidget2 = new LabelWidget() { Text = "进度条", Size = new Vector2(64, 18),FontScale=0.5f ,Color=Color.Green};
        public LabelWidget labelWidget3 = new LabelWidget() { Text = "格子2", Size = new Vector2(64, 18),FontScale=0.5f ,Color=Color.Green};
        public RectangleWidget rectangleWidget = new RectangleWidget() { FillColor=Color.Gray,OutlineColor=Color.White};
        public ValueBarWidget barWidget = new ValueBarWidget() {Margin = new Vector2(15, 20), BarsCount = 100, LayoutDirection = LayoutDirection.Vertical, LitBarColor = Color.White, BarSize = new Vector2(8, 1),FlipDirection=true};
        public Subtexture customSubtexture;

        public BaseEnergyWidget() {

            canvasWidgetc.Children.Add(rectangleWidget);
            customSubtexture = new Subtexture(ContentManager.Get<Texture2D>("Mekiasm/Textures/ProcessCell"), Vector2.Zero, Vector2.One);
            barWidget.BarSubtexture = customSubtexture;
            this.Children.Add(stackbg);
            this.Children.Add(stackmain);
            stackmain.Children.Add(canvasWidgetc);
            canvasWidgetc.Children.Add(stackPanelLeft);
            stackpro.Children.Add(barWidget);
            stackPanelLeft.Children.Add(stackpro);
            canvasWidgetc.Children.Add(stackPanelCenter);
            stackPanelCenter.Children.Add(labelWidget1);
            stackPanelCenter.Children.Add(labelWidget2);
            stackPanelCenter.Children.Add(labelWidget3);
        }
    
    }
}
