using oop3.DisplayObjects;

namespace oop3.Utilities
{
    internal class ObjectGenerator
    {
        private static Random rand = new Random();

        private static readonly int WIDTH_MIN = 50;
        private static readonly int WIDTH_MAX = 150;
        private static readonly int HEIGHT_MIN = 50;
        private static readonly int HEIGHT_MAX = 150;
        public static int generateWidth() => rand.Next(WIDTH_MIN, WIDTH_MAX);
        public static int generateHeight() => rand.Next(HEIGHT_MIN, HEIGHT_MAX);
        public static int generateAngle() => (int)(rand.NextDouble() * 360);

        private static readonly int velXMin = -100;
        private static readonly int velXMax = 100;
        private static readonly int velYMin = -100;
        private static readonly int velYMax = 100;

        private static readonly int accXMin = -100;
        private static readonly int accXMax = 100;
        private static readonly int accYMin = -100;
        private static readonly int accYMax = 100;
        public static (int, int) generateVelocity() => (rand.Next(velXMin, velXMax), rand.Next(velYMin, velYMax));
        public static (int, int) generateAcceleration() => (rand.Next(accXMin, accXMax), rand.Next(accYMin, accYMax));


        private static int actualW = 0, actualH = 0;

        delegate DisplayObject genObject();
        private static readonly int amntOfObjects = 1;

        private static genObject[] genDelegates = new genObject[] { genRandomRect, genRandomSquare, genRandomEllipse, genRandomCircle, genRandomLine, genRandomTriangle };
        //private static genObject[] genDelegates = new genObject[] { genRandomRect };
        public static (GameField, DisplayObject[]) generateDrawField(int leftTopX, int leftTopY, int bottomRightX, int bottomRightY, int thickness, bool acceleration)
        {

            GameField CDrawField = new GameField(leftTopX, leftTopY, bottomRightX, bottomRightY, thickness);
            CDrawField.fillColor = Color.FromArgb(255, 255, 218);
            CDrawField.strokeColor = Color.FromArgb(128, 128, 128);
            //CDrawField.setRotationAngle(Math.PI / 6);

            InitializeGenerators(bottomRightX - leftTopX, bottomRightY - leftTopY, thickness);

            DisplayObject[] objects = genObjects(genDelegates, acceleration);

            return (CDrawField, objects);

        }
        private static DisplayObject[] genObjects(genObject[] generationArr, bool isAccelerated)
        {
            DisplayObject[] objects = new DisplayObject[generationArr.Length * amntOfObjects];
            DisplayObject obj;
            for (int i = 0; i < generationArr.Length; i++)
            {
                for (int j = 0; j < amntOfObjects; j++)
                {
                    // acquire current generation 
                    obj = generationArr[i]();
                    Color clr = getRandomColor();
                    obj.fillColor = Color.FromArgb(clr.R, clr.G, clr.B);
                    //field.AddObject(obj);
                    objects[i * amntOfObjects + j] = obj;
                    (int x, int y) = generateVelocity();
                    obj.velX = x;
                    obj.velY = y;

                    if (isAccelerated)
                    {
                        (int aX, int aY) = generateAcceleration();
                        obj.accX = aX;
                        obj.accY = aY;
                    }

                }
            }
            return objects;
        }

        public static void InitializeGenerators(int fieldWidth, int fieldHeight, int borderThick)
        {
            actualW = fieldWidth - 2 * borderThick;
            actualH = fieldHeight - 2 * borderThick;

        }

        public static DisplayObject genRandomRect()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateTopLeftRel(width, height);
            int topLeftX = p.X;
            int topLeftY = p.Y;
            RectangleObject ro = new RectangleObject(topLeftX, topLeftY, topLeftX + width, topLeftY + height);
            ro.SetRotationAngle(generateAngle());
            return ro;
        }

        public static DisplayObject genRandomSquare()
        {
            int width = generateWidth();
            Point p = generateTopLeftRel(width, width);
            int topLeftX = p.X;
            int topLeftY = p.Y;

            SquareObject so = new SquareObject(topLeftX, topLeftY, width);
            so.SetRotationAngle(generateAngle());
            return so;
        }
        public static DisplayObject genRandomEllipse()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateAnchorRel(width, height);
            int centerX = p.X;
            int centerY = p.Y;
            EllipseObject eo = new EllipseObject(centerX, centerY, width / 2, height / 2);
            eo.SetRotationAngle(generateAngle());
            return eo;
        }

        public static DisplayObject genRandomCircle()
        {
            int width = generateWidth();
            Point p = generateAnchorRel(width, width);
            int centerX = p.X;
            int centerY = p.Y;
            CircleObject co = new CircleObject(centerX, centerY, width / 2);
            co.SetRotationAngle(generateAngle());
            return co;
        }

        public static DisplayObject genRandomTriangle()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point p = generateTopLeftRel(width, height);
            int topLeftX = p.X;
            int topLeftY = p.Y;
            TriangleObject to = new TriangleObject(topLeftX, topLeftY, width, height);
            to.SetRotationAngle(generateAngle());
            return to;
        }
        public static DisplayObject genRandomLine()
        {
            int width = generateWidth();
            int height = generateHeight();
            Point anc = generateTopLeftRel(width, height);
            Point p1 = new Point(anc.X, anc.Y);
            Point p2 = new Point(anc.X + width, anc.Y + height);
            int x1 = anc.X;
            int y1 = anc.Y;
            int x2 = anc.X + width;
            int y2 = anc.Y + height;
            LineObject lo = new LineObject(x1, y1, x2, y2);
            lo.SetStrokeThickness(5);
            lo.SetRotationAngle(generateAngle());
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


        public static Point generateTopLeftRel(int width, int height)
        {
            Point topLeft = generateAnchorRel(width, height);
            topLeft.X -= width / 2;
            topLeft.Y -= height / 2;
            return topLeft;
        }

        public static Color getRandomColor()
        {
            return Color.FromArgb(255, (byte)rand.Next(), (byte)rand.Next(), (byte)rand.Next());
        }
    }
}
