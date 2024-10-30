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
        public int rectX1, rectY1, rectX2, rectY2;


        public int rectX1_s, rectY1_s, rectX2_s, rectY2_s;

        public Rectangl(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int R = 0, int G = 0, int B = 0) :
            base((topLeftX + bottomRightX) / 2, (topLeftY + bottomRightY) / 2, bottomRightX - topLeftY, bottomRightY - topLeftY)
        {
            rectX1_s = topLeftX;
            rectY1_s = topLeftY;
            rectX2_s = bottomRightX;
            rectY2_s = bottomRightY;

            rectX1 = rectX1_s;
            rectY1 = rectY1_s;
            rectX2 = rectX2_s;
            rectY2 = rectY2_s;
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
            Rectangle rect = new Rectangle((int)rectX1, (int)(rectY1), (int)(rectX2 - rectX1), (int)(rectY2 - rectY1));
            g.FillRectangle(fillBrush, rect);
            g.DrawRectangle(new Pen(strokeBrush, strokeThick), rect);

            g.Restore(prevState);
        }

        protected override void RenewPoints(int dx, int dy) { 
            rectX1 += dx; rectY1 += dy;
            rectX2 += dx; rectY2 += dy;
            rectX1_s = rectX1;
            rectX2_s = rectX2;
            rectY1_s = rectY1;
            rectY2_s = rectY2;
        }

        protected override void ShiftPoints(int dx, int dy)
        {
            rectX1 = rectX1_s + dx;
            rectY1 = rectY1_s + dy;
            rectX2 = rectX2_s + dx;
            rectY2 = rectY2_s + dy;
        }
    }
}
