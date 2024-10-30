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

        public Line(int X1, int Y1, int X2, int Y2, int R = 0, int G = 0, int B = 0) :
            base((X1 + X2) / 2, (Y1 + Y2) / 2, Math.Abs(X1 - X2), Math.Abs(Y1 - Y2), R, G, B)
        {
            x1 = X1; x2 = X2; y1 = Y1; y2 = Y2;
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

            g.DrawLine(new Pen(fillBrush, strokeThick), new Point(x1, y1), new Point(x2, y2));

            g.Restore(prevState);
        }
    }
}
