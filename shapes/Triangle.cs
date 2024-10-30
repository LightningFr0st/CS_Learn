using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Triangle : DisplayObject
    {
        protected int[] coordsX, coordsY;

        public Triangle(int point1X, int point1Y, int point2X, int point2Y, int point3X, int point3Y, int R = 0, int G = 0, int B = 0) :
            this(SetBaseProperties(point1X, point1Y, point2X, point2Y, point3X, point3Y), R, G, B)
        {
            coordsX = [point1X, point2X, point3X];
            coordsY = [point1Y, point2Y, point3Y];
        }
        public Triangle(int topLeftX, int topLeftY, int width, int height, int R = 0, int G = 0, int B = 0) :
            this(topLeftX, topLeftY + height, topLeftX + width / 2, topLeftY, topLeftX + width, topLeftY + height, R, G, B)
        { }

        private Triangle(Tuple<int, int, int, int> baseProperties, int R = 0, int G = 0, int B = 0) :
            base(baseProperties.Item1, baseProperties.Item2, baseProperties.Item3, baseProperties.Item4, R, G, B)
        { }

        // Finding 
        private static Tuple<int, int, int, int> SetBaseProperties(int point1X, int point1Y, int point2X, int point2Y, int point3X, int point3Y)
        {
            int minX = Math.Min(point1X, Math.Min(point2X, point3X));
            int maxX = Math.Max(point1X, Math.Max(point2X, point3X));
            int minY = Math.Min(point1Y, Math.Min(point2Y, point3Y));
            int maxY = Math.Max(point1Y, Math.Max(point2Y, point3Y));

            return Tuple.Create((minX + maxX) / 2, (minY + maxY) / 2, maxX - minX, maxY - minY);
        }

        protected override void UpdateFrame()
        {
            SetFrameFromPoints(x1_anch, y1_anch, coordsX, coordsY);
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);
            Point[] polygonPoints = new Point[] {
                new Point(coordsX[0],coordsY[0]),
                new Point(coordsX[1],coordsY[1]),
                new Point(coordsX[2],coordsY[2])
            };
            g.FillPolygon(fillBrush, polygonPoints);
            g.DrawPolygon(new Pen(strokeBrush, strokeThick), polygonPoints);

            g.Restore(prevState);
        }
    }
}
