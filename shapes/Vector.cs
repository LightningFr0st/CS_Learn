using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    internal class Vector
    {
        public int x;
        public int y;

        public Vector(int p_x, int p_y)
        {
            x = p_x;
            y = p_y;
        }

        public static double Rad(double degrees) => degrees * Math.PI / 180;

        public static (int x, int y) getVector
            (int centerX, int centerY, int targetX, int targetY)
        {
            int vectorX = targetX - centerX;
            int vectorY = targetY - centerY;
            return (vectorX, vectorY);
        }

        public static (int x, int y) rotateVector
            (int vectorX, int vectorY, double angleRad)
        {
            int rotatedX = (int)(vectorX * Math.Cos(angleRad) - vectorY * Math.Sin(angleRad));
            int rotatedY = (int)(vectorX * Math.Sin(angleRad) + vectorY * Math.Cos(angleRad));
            return (rotatedX, rotatedY);
        }
    }
}
