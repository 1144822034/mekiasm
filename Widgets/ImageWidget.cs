using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Game;
namespace Mekiasm
{
    public class ImageWidget :Widget
    {
        public Vector2 textcora = new Vector2(1, 1);

        public Texture2D Texture_;
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
        public Vector2 startPoint=Vector2.Zero;
        public Vector2 endPoint=Vector2.One;
        public ImageWidget()
        {
            IsHitTestVisible = false;
            IsDrawRequired = true;
        }
        public void setTexcoord(Vector2 start,Vector2 end) {
            startPoint = start;
            endPoint = end;
        }
        public override void Draw()
        {
            if (Texture != null)
            {
                TexturedBatch2D texturedBatch2D = WidgetsManager.PrimitivesRenderer2D.TexturedBatch(Texture, useAlphaTest: false, 0, DepthStencilState.None, null, BlendState.NonPremultiplied, SamplerState.PointWrap);
                int count = texturedBatch2D.TriangleVertices.Count;
                texturedBatch2D.QueueQuad(Vector2.Zero,base.DesiredSize, 0f, startPoint, endPoint, base.GlobalColorTransform);
                texturedBatch2D.TransformTriangles(base.GlobalTransform, count);                
            }
        }
    }
}
  