using Engine.Graphics;
using Engine;
using System.Collections.Generic;

namespace Game
{
    public class mProgessWidget : CanvasWidget
    {
        TexturedBatch2D texturedBatch2D;
        public bool needDraw = true;
        public float prog = 0;
        public int ptype = 0;
        public int _renderType=0;
        public Texture2D mtexture;
        public Color _base = Color.White;
        public Color _nbase = Color.White;
        public mProgessWidget()
        {

        }
        public Texture2D Texture {
            set { mtexture = value; texturedBatch2D = WidgetsManager.PrimitivesRenderer2D.TexturedBatch(Texture); }
            get { return mtexture; }
        }
        public void setColor(Color base_,Color nbase_) {
            _base = base_;
            _nbase = nbase_;
        
        }
        public LayoutDirection Direction {
            set;
            get;
        }
        public float Progess
        {
            set { prog = value; }
            get { return prog; }
        }
        public float Scale = 1f;
        public override void Draw()
        {
            if (Texture == null) return;
            Matrix m = GlobalTransform;
            Vector2 v1, v3, v33;
                v1 = Vector2.Zero;
                v3 = new Vector2(Texture.Width * Scale, Texture.Height * Scale);//底层100%宽
                v33 = new Vector2(Texture.Width * Progess * Scale, Texture.Height * Scale);
                Vector2.Transform(ref v1, ref m, out Vector2 result);
                Vector2.Transform(ref v3, ref m, out Vector2 result3);
                Vector2.Transform(ref v33, ref m, out Vector2 result33);
                texturedBatch2D.QueueQuad(result, result3, 0f, Vector2.Zero, Vector2.One, _base);//底层图
                texturedBatch2D.QueueQuad(result, result33, 0f, Vector2.Zero, new Vector2(Progess, 1f), _nbase);            
        }
        public override void MeasureOverride(Vector2 parentAvailableSize)
        {
            IsDrawRequired = needDraw;
        }
    }
}
