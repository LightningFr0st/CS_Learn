using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Rectangl : DisplayObject
    {
        protected int rectX1, rectY1, rectX2, rectY2;

        public Rectangl(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int R = 0, int G = 0, int B = 0) :
            base((topLeftX + bottomRightX) / 2, (topLeftY + bottomRightY) / 2, bottomRightX - topLeftY, bottomRightY - topLeftY)
        {
            rectX1 = topLeftX;
            rectY1 = topLeftY;
            rectX2 = bottomRightX;
            rectY2 = bottomRightY;
        }

        protected override void UpdateFrame()
        {
            int[] coordsX = { rectX1, rectX2, rectX1, rectX2 };
            int[] coordsY = { rectY1, rectY1, rectY2, rectY2 };

            SetFrameFromPoints(x1_anch, y1_anch, coordsX, coordsY);
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);
            Rectangle rect = new Rectangle(rectX1, rectY1, rectX2 - rectX1, rectY2 - rectY1);
            g.FillRectangle(fillBrush, rect);
            g.DrawRectangle(new Pen(strokeBrush, strokeThick), rect);

            g.Restore(prevState);
        }
    }
}
