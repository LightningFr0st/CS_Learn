using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Square : Rectangl
    {
        public Square(int topLeftX, int topLeftY, int size, int R = 0, int G = 0, int B = 0) :
            base(topLeftX, topLeftY, topLeftX + size, topLeftY + size, R, G, B)
        { }
    }
}
