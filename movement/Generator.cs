using movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shapes
{
    internal class Generator
    {
        private static Random rand = new Random();

        private static readonly int WIDTH_MIN = 50;
        private static readonly int WIDTH_MAX = 150;
        private static readonly int HEIGHT_MIN = 50;
        private static readonly int HEIGHT_MAX = 150;
        public static int generateWidth() => rand.Next(WIDTH_MIN, WIDTH_MAX);
        public static int generateHeight() => rand.Next(HEIGHT_MIN, HEIGHT_MAX);
        public static int generateAngle() => (int)(rand.NextDouble() * 360);

        private static int MinV = 100;
        private static int MaxV = 500;

        private static int MinA = 100;
        private static int MaxA = 500;

        private static int fieldW = 0, fieldH = 0, borderT = 0;
        private static int actualW = 0, actualH = 0;
        private static int topLeftX = 0, topLeftY = 0;

        private static int offset = 15;
        

        delegate DisplayObject genObject();
        private static readonly int amntOfObjects = 1;

        private static genObject[] genDelegates = new genObject[] { genRandomRect, genRandomSquare, genRandomEllipse, genRandomCircle, genRandomLine, genRandomTriangle };
        public static (Drawfield, DisplayObject[]) generateDrawField(int leftTopX, int leftTopY, int bottomRightX, int bottomRightY, int thickness, bool acceleration, bool test = false)
        {

            Drawfield CDrawField = new Drawfield(leftTopX, leftTopY, bottomRightX, bottomRightY, thickness, 255, 255, 100);
            CDrawField.strokeColor = Color.FromArgb(0, 255, 0);

            InitializeGenerators(leftTopX, leftTopY, bottomRightX - leftTopX, bottomRightY - leftTopY, thickness);

            DisplayObject[] objects;

            if (test == false)
            {
                objects = genObjects(genDelegates, acceleration);
            }
            else
            {
                objects = testAcceleration(genDelegates, acceleration);
            }

            return (CDrawField, objects);

        }

        private static DisplayObject[] testAcceleration(genObject[] generationArr, bool isAccelerated)
        {
            DisplayObject[] objects = new DisplayObject[3];
            int width = generateWidth();
            int R = 20;
            Circle co1 = new Circle(topLeftX + borderT + R, topLeftY + fieldH - borderT - R, R);
            Circle co2 = new Circle(topLeftX + borderT + R, topLeftY + fieldH - borderT - R, R);
            Circle co3 = new Circle(topLeftX + borderT + R, topLeftY + fieldH - borderT - R, R);

            co1.SetFillColor(61, 204, 99);
            co2.SetFillColor(64, 40, 161);
            co3.SetFillColor(125, 125, 125);

            co1.setMovement(100, 100, 45);
            co2.setMovement(100, 100, 0);

            objects[0] = co1;
            objects[1] = co2;
            objects[2] = co3;

            return objects;
        }

        private static DisplayObject[] genObjects(genObject[] generationArr, bool isAccelerated)
        {
            DisplayObject[] objects = new DisplayObject[generationArr.Length * amntOfObjects + 1];
            DisplayObject obj;
            for (int i = 0; i < generationArr.Length; i++)
            {
                for (int j = 0; j < amntOfObjects; j++)
                {
                    // acquire current generation 
                    obj = generationArr[i]();
                    Color clr = getRandomColor();
                    obj.SetFillColor(clr.R, clr.G, clr.B);

                    //field.AddObject(obj);
                    objects[i * amntOfObjects + j + 1] = obj;
                    double v = GetRandomVelocity();
                    double a = 0;
                    if (isAccelerated)
                    {
                        a = GetRandomAcceleration();
                    }
                    double angle = GetRandomAgnle();
                    obj.setMovement(v, a, angle);
                }
            }
            return objects;
        }

        public static void InitializeGenerators(int fieldWidth, int fieldHeight, int borderThick)
        {
            fieldW = fieldWidth;
            fieldH = fieldHeight;
            borderT = borderThick;

            actualW = fieldW - 2 * borderT;
            actualH = fieldH - 2 * borderT;
        }
        public static void InitializeGenerators(int absTopLeftX, int absTopLeftY, int fieldWidth, int fieldHeight, int borderThick)
        {
            topLeftX = absTopLeftX;
            topLeftY = absTopLeftY;
            InitializeGenerators(fieldWidth, fieldHeight, borderThick);
        }

        public static DisplayObject genRandomRect()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateTopLeft(width, height);
            int topLeftX = (int)p.X;
            int topLeftY = (int)p.Y;
            Rectangl ro = new Rectangl(topLeftX, topLeftY, topLeftX + width, topLeftY + height);
            ro.SetAngle(generateAngle());
            return ro;
        }

        public static DisplayObject genRandomSquare()
        {
            int width = generateWidth();
            Point p = generateTopLeft(width, width);
            int topLeftX = (int)p.X;
            int topLeftY = (int)p.Y;

            Square so = new Square(topLeftX, topLeftY, width);
            so.SetAngle(generateAngle());
            return so;
        }
        public static DisplayObject genRandomEllipse()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateAnchor(width, height);
            int centerX = (int)p.X;
            int centerY = (int)p.Y;
            Ellipse eo = new Ellipse(centerX, centerY, width / 2, height / 2);
            eo.SetAngle(generateAngle());
            return eo;
        }

        public static DisplayObject genRandomCircle()
        {
            int width = generateWidth();
            Point p = generateAnchor(width, width);
            int centerX = (int)p.X;
            int centerY = (int)p.Y;
            Circle co = new Circle(centerX, centerY, width / 2);
            co.SetAngle(generateAngle());
            return co;
        }

        public static DisplayObject genRandomTriangle()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateTopLeft(width, height);
            int topLeftX = (int)p.X;
            int topLeftY = (int)p.Y;
            Triangle to = new Triangle(topLeftX, topLeftY, width, height);
            to.SetAngle(generateAngle());
            return to;
        }
        public static DisplayObject genRandomLine()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point anc = generateTopLeft(width, height);
            Point p1 = new Point(anc.X, anc.Y);
            Point p2 = new Point(anc.X + width, anc.Y + height);
            int x1 = (int)(anc.X);
            int y1 = (int)(anc.Y);
            int x2 = (int)(anc.X + width);
            int y2 = (int)(anc.Y + height);
            Line lo = new Line(x1, y1, x2, y2);
            lo.SetThick(5);
            lo.SetAngle(generateAngle());
            return lo;
        }
        public static Point generateAnchorRel(double width, double height)
        {
            double maxBump;
            maxBump = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) / 2;
            Point anchor = new Point();
            anchor.X = (int)Math.Floor(rand.NextDouble() * (actualW - 2 * maxBump) + maxBump);
            anchor.Y = (int)Math.Floor(rand.NextDouble() * (actualH - 2 * maxBump) + maxBump);
            return anchor;
        }

        public static Point generateAnchor(double width, double height)
        {
            Point p = generateAnchorRel(width, height);
            p.X += topLeftX;
            p.Y += topLeftY;
            return p;
        }
        public static Point generateTopLeftRel(int width, int height)
        {
            Point topLeft = generateAnchorRel(width, height);
            topLeft.X -= (width / 2);
            topLeft.Y -= (height / 2);
            return topLeft;
        }
        public static Point generateTopLeft(int width, int height)
        {
            Point topLeft = generateTopLeftRel(width, height);
            topLeft.X += topLeftX;
            topLeft.Y += topLeftY;
            return topLeft;
        }

        public static Color getRandomColor()
        {
            return Color.FromArgb(255, (byte)rand.Next(), (byte)rand.Next(), (byte)rand.Next());
        }

        public static double GetRandomVelocity()
        {
            return ((rand.NextDouble() * (MaxV - MinV) + MinV));
        }

        public static double GetRandomAcceleration()
        {
            return ((rand.NextDouble() * (MaxA - MinA) + MinA));
        }

        public static double GetRandomAgnle()
        {
            return ((rand.NextDouble() * 360));
        }
    }
}
