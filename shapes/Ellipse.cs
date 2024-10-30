using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Ellipse : DisplayObject
    {
        protected int radiusX, radiusY;

        protected int centerX, centerY;

        public Ellipse(int p_centerX, int p_centerY, int p_radiusX, int p_radiusY, int R = 0, int G = 0, int B = 0) :
            base(p_centerX, p_centerY, p_radiusX * 2, p_radiusY * 2, R, G, B)
        {
            centerX = p_centerX;
            centerY = p_centerY;
            radiusX = p_radiusX;
            radiusY = p_radiusY;
        }

        protected override void UpdateFrame()
        {
            double angle = Vector.Rad(this.angle);

            int shiftX = (int)Math.Sqrt(
                Math.Pow(radiusX * Math.Cos(angle), 2) +
                Math.Pow(radiusY * Math.Sin(angle), 2)
                );
            int shiftY = (int)Math.Sqrt(
                Math.Pow(radiusX * Math.Sin(angle), 2) +
                Math.Pow(radiusY * Math.Cos(angle), 2)
                );

            x1_frame = centerX - shiftX;
            y1_frame = centerY - shiftY;
            x2_frame = x1_frame + shiftX * 2;
            y2_frame = y1_frame + shiftY * 2;
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);
            Rectangle ellipse = new Rectangle(centerX - radiusX, centerY - radiusY, radiusX * 2, radiusY * 2);
            g.FillEllipse(fillBrush, ellipse);
            g.DrawEllipse(new Pen(strokeBrush, strokeThick), ellipse);

            //g.Restore(prevState);
        }
    }
}
