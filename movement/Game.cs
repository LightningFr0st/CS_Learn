using shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace shapes
{
    internal class Game
    {
        private readonly int MinV = 100;
        private readonly int MaxV = 500;

        private readonly int MinA = 100;
        private readonly int MaxA = 500;

        private DisplayObject[] objects;
        private int objCount;
        private Drawfield gamefield;
        public Rectangl border;
        private Random random;
        private bool isAccelerated;

        public Game(Drawfield field, bool accelerated)
        {
            gamefield = field;
            border = new Rectangl(0, 0, 1920, 1080, 255, 0, 0);
            border.SetFillColor(235, 229, 213);
            border.SetStrokeColor(255, 255, 255);
            border.SetThick(15);
            objects = new DisplayObject[100];
            objCount = 0;
            random = new Random();
            isAccelerated = accelerated;
        }

        public void AddObject(DisplayObject obj)
        {
            objects[objCount++] = obj;
        }

        public void MoveObjects(double dt = 10000)
        {
            int shiftX, shiftY;
            DisplayObject obj;
            for (int i = 0; i < objCount; i++)
            {
                obj = objects[i];
                obj.move(dt);
                while (!obj.IsInBounds(gamefield.clientX1, gamefield.clientY1,
                    gamefield.clientX2, gamefield.clientY2))
                {
                    (shiftX, shiftY) = GetRandomPoint();    
                    shiftX -= obj.x1_anch;
                    shiftY -= obj.y1_anch;
                    obj.RenewPos(shiftX, shiftY);
                    double v = GetRandomVelocity();
                    double a = 0;
                    if (isAccelerated)
                    {
                        a = GetRandomAcceleration();
                    }

                    double angle = GetRandomAgnle(360);

                    obj.setMovement(v, a, angle);
                }
            }
        }

        public void DrawGame(Graphics g, int XX, int YY)
        {
            border.rectX2 = XX;
            border.rectY2 = YY;
            border.Draw(g);
            gamefield.Draw(g);
            for (int i = 0; i < objCount; i++)
            {
                objects[i].Draw(g);
            }
        }

        public (int, int) GetRandomPoint()
        {
            return ((random.Next(gamefield.clientX1, gamefield.clientX2)),
                    (random.Next(gamefield.clientY1, gamefield.clientY2)));
        }
        public double GetRandomVelocity()
        {
            return ((random.NextDouble() * (MaxV - MinV) + MinV));
        }

        public double GetRandomAcceleration()
        {
            return  ((random.NextDouble() * (MaxA - MinA) + MinA));
        }

        public double GetRandomAgnle(int MaxAngle)
        {
            return ((random.NextDouble() * MaxAngle));
        }
    }
}
