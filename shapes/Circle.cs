using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Circle : Ellipse
    {
        public Circle(int centerX, int centerY, int radius, int R = 0, int G = 0, int B = 0) :
            base(centerX, centerY, radius, radius, R, G, B)
        { }
    }
}
