using Engine;
using Engine.Graphics;
using Game;
using System.Runtime.Serialization;

namespace Mekiasm
{
    public class BuffView:CanvasWidget
    {
        public StackPanelWidget verLine = new StackPanelWidget() { Direction=LayoutDirection.Vertical,HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Center};
        public mProgessWidget mProgessWidget = new mProgessWidget() ;
        public LabelWidget labelWidget = new LabelWidget() { Size = new Vector2(32, 16), FontScale = 0.5f, HorizontalAlignment = WidgetAlignment.Center };
        public BuffView() {
            Size = new Vector2(32,48);
            mProgessWidget._base = new Color(125,125,125);
            Children.Add(verLine);
            verLine.Children.Add(mProgessWidget);
            verLine.Children.Add(labelWidget);
        }
        public void setData(ComponentBuff.Buff buff) {
            labelWidget.Text = buff.RemainTime+"s";
            mProgessWidget.Progess = (float) buff.RemainTime /(float) buff.TotalTime;
            mProgessWidget.Texture = findTexture(buff.bufftype);
        }
        public Texture2D findTexture(ComponentBuff.BuffTYpe buffTYpe) {
            return TextureAtlasManager.GetSubtexture("Mekiasm/Gui/Buff/Buff_"+(int)buffTYpe+ ".").Texture;               
        }
    }
}
