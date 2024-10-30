using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Line : DisplayObject
    {
        protected int x1, x2, y1, y2;

        protected int x1_s, x2_s, y1_s, y2_s;

        public Line(int X1, int Y1, int X2, int Y2, int R = 0, int G = 0, int B = 0) :
            base((X1 + X2) / 2, (Y1 + Y2) / 2, Math.Abs(X1 - X2), Math.Abs(Y1 - Y2), R, G, B)
        {
            x1_s = X1; x2_s = X2; y1_s = Y1; y2_s = Y2;
            x1 = x1_s; x2 = x2_s; y1 = y1_s; y2 = y2_s;
        }

        protected override void UpdateFrame()
        {
            int[] pointsX = [ x1, x2 ];
            int[] pointsY = [ y1, y2 ];

            SetFrameFromPoints(x1_anch, y1_anch, pointsX, pointsY);
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);

            g.DrawLine(new Pen(fillBrush, strokeThick), new Point((int)x1, (int)y1), new Point((int)x2, (int)y2));

            g.Restore(prevState);
        }

        protected override void RenewPoints(int dx, int dy)
        {
            x1 += dx;
            y1 += dy;
            x2 += dx;
            y2 += dy;

            x1_s = x1;
            x2_s = x2;
            y1_s = y1;
            y2_s = y2;
        }

        protected override void ShiftPoints(int dx, int dy)
        {
            x1 = x1_s + dx;
            y1 = y1_s + dy;

            x2 = x2_s + dx;
            y2 = y2_s + dy;
        }
    }
}
