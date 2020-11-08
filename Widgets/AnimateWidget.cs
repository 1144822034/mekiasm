using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
{
    public class AnimateWidget : Widget
    {
        public static Vector2 textcora = new Vector2(1, 1);
        public Vector2 texcoorddata;
        public Texture2D Texture_;
        public double lastupdate;
        public List<Vector2> drawPoint_start = new List<Vector2>();
        public List<Vector2> drawPoint_end = new List<Vector2>();
        public float framedur = 0;
        public int drawpos = 0;
        public Texture2D Texture
        {
            get
            {
                return Texture_;
            }
            set
            {
                Texture_ = value;
            }
        }

        public AnimateWidget(Vector2 data,float dura)//水平个数，垂直个数
        {
            texcoorddata = data;
            IsHitTestVisible = false;
            IsDrawRequired = true;
            DesiredSize = new Vector2(64,64);
            framedur = dura;
            float x = 1f / texcoorddata.X;//水平单位
            float y = 1f / texcoorddata.Y;//垂直单位

            for (int i = 0; i < texcoorddata.X; i++)
            {
                for (int j = 0; j < texcoorddata.Y; j++)
                {
                    drawPoint_start.Add(new Vector2(j * x, i * y));
                    drawPoint_end.Add( new Vector2((j + 1) * x, (i + 1) * y));                }
            }
                }
        public override void Draw()
        {
            if (Texture != null)
            {
               
                if (Engine.Time.RealTime - lastupdate > framedur)
                {
                    ++drawpos; if (drawpos >= drawPoint_end.Count) drawpos = 0;
                    lastupdate = Engine.Time.RealTime;

                }
                    TexturedBatch2D texturedBatch2D = WidgetsManager.PrimitivesRenderer2D.TexturedBatch(Texture, useAlphaTest: false, 0, DepthStencilState.None, null, BlendState.NonPremultiplied, SamplerState.PointWrap);
                    int count = texturedBatch2D.TriangleVertices.Count;
                    texturedBatch2D.QueueQuad(Vector2.Zero, base.ActualSize, 0f, drawPoint_start[drawpos], drawPoint_end[drawpos], base.GlobalColorTransform);
                    texturedBatch2D.TransformTriangles(base.GlobalTransform, count);                
            }
        }
    }
}
