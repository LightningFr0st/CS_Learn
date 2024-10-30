
using System;
using System.Drawing.Drawing2D;

namespace oop3.DisplayObjects
{
    internal class GameField : DisplayObject
    {
        // outer frame coordinates
        public int fieldX1, fieldY1;
        public int fieldX2, fieldY2;

        // client field coordinates
        public int clientX1, clientY1;
        public int clientX2, clientY2;


        // velocity value ranges
        private readonly int MinV_X = -100;
        private readonly int MaxV_X = 100;
        private readonly int MinV_Y = -100;
        private readonly int MaxV_Y = 100;

        // acceleration value ranges
        private readonly int MinA_X = -100;
        private readonly int MaxA_X = 100;
        private readonly int MinA_Y = -100;
        private readonly int MaxA_Y = 100;

        // gamefield objects storage
        private readonly int MaxObjects = 20;
        private DisplayObject[] objects;
        private int objCount;

        private Random random;
        public bool isAccelerated;


        // Initializer for all constructors of the DrawField
        private void InitializeFields(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int borderThickness)
        {

            fieldX1 = topLeftX;
            fieldY1 = topLeftY;
            fieldX2 = bottomRightX;
            fieldY2 = bottomRightY;

            clientX1 = fieldX1 + borderThickness;
            clientY1 = fieldY1 + borderThickness;
            clientX2 = fieldX2 - borderThickness;
            clientY2 = fieldY2 - borderThickness;

            objects = new DisplayObject[MaxObjects];
            objCount = 0;

            random = new Random();
            isAccelerated = false;
            clickHandler = () =>
            {
                Console.WriteLine("Gamefield clicked");
            };
        }


        // Constructor for field with fill color
        public GameField(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int borderThickness, Color? fill = null) :
            base((topLeftX + bottomRightX) / 2, (topLeftY + bottomRightY) / 2,
                bottomRightX - topLeftX, bottomRightY - topLeftY, fill)
        {
            InitializeFields(topLeftX, topLeftY, bottomRightX, bottomRightY, borderThickness);
        }

        // Constructor for field with fill texture
        public GameField(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int borderThickness, Bitmap bmp) :
            base((topLeftX + bottomRightX) / 2, (topLeftY + bottomRightY) / 2,
                bottomRightX - topLeftX, bottomRightY - topLeftY, bmp)
        {
            InitializeFields(topLeftX, topLeftY, bottomRightX, bottomRightY, borderThickness);
        }

        public override bool Click(int mouseX, int mouseY)
        {
            if (base.Click(mouseX, mouseY))
            {
                // transfer coordinates to relative to gamefield coordinates
                mouseX -= clientX1;
                mouseY -= clientY1;

                int i = objCount - 1;
                bool clickHandled = false;
                while (!clickHandled && i >= 0)
                {
                    clickHandled = objects[i].Click(mouseX, mouseY);
                    i--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? AddObject(DisplayObject obj)
        {
            if (objCount != MaxObjects)
            {
                objects[objCount] = obj;
                return objCount++;
            }
            return null;
        }

        public bool DeleteObject(int index)
        {
            if (index < objCount)
            {
                for (int i = index; i < objCount - 1; i++)
                {
                    objects[i] = objects[i + 1];
                }
                objCount--;
                return true;
            }
            return false;

        }
        public bool DeleteObject(DisplayObject obj)
        {
            int i = 0;
            bool flagFound = false;
            while (i < objCount && !flagFound)
            {
                flagFound = objects[i] == obj;
                i++;
            }
            if (flagFound)
            {
                return DeleteObject(i);
            }
            else
            {
                return false;
            }
        }

        public void MoveObjects(int deltaTime)
        {
            int shiftX, shiftY;
            int vX, vY;
            int aX, aY;
            DisplayObject obj;
            for (int i = 0; i < objCount; i++)
            {
                obj = objects[i];
                obj.Move(deltaTime);
                while (!obj.IsInBounds(0, 0, clientX2 - clientX1, clientY2 - clientY1))
                {
                    (shiftX, shiftY) = RandomGamefieldPoint();
                    shiftX -= obj.anchorX;
                    shiftY -= obj.anchorY;
                    obj.ShiftObject(shiftX, shiftY);

                    (vX, vY) = RandomVelocity();
                    obj.velX = vX;
                    obj.velY = vY;

                    if (isAccelerated)
                    {
                        (aX, aY) = RandomAcceleration();
                        obj.accX = aX;
                        obj.accY = aY;
                    }
                }

            }
        }

        protected override void UpdateFrame()
        {
            // form the point collection 
            int[] coordsX = [fieldX1, fieldX2, fieldX1, fieldX2];
            int[] coordsY = [fieldY1, fieldY1, fieldY2, fieldY2];

            // pass it to the method
            SetFrameFromPoints(anchorX, anchorY, coordsX, coordsY);
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);
            g.TranslateTransform(fieldX1, fieldY1);

            // Drawing the outer frame
            Rectangle rectOuter = new Rectangle(0, 0, (int)(fieldX2 - fieldX1), (int)(fieldY2 - fieldY1));
            g.FillRectangle(strokeBrush, rectOuter);
            g.DrawRectangle(new Pen(strokeBrush), rectOuter);

            // Drawing the client area            
            g.TranslateTransform(clientX1 - frameX1, clientY1 - frameY1);
            Rectangle rectClient = new Rectangle(0, 0, (int)(clientX2 - clientX1), (int)(clientY2 - clientY1));
            g.FillRectangle(fillBrush, rectClient);
            g.DrawRectangle(new Pen(fillBrush), rectClient);


            // drawing the objects inside the field
            for (int i = 0; i < objCount; i++)
            {
                objects[i].Draw(g);
            }

            g.Restore(prevState);
        }

        protected override void ShiftCoords(int deltaX, int deltaY)
        {
            fieldX1 += deltaX;
            fieldY1 += deltaY;
            fieldX2 += deltaX;
            fieldY2 += deltaY;

            clientX1 += deltaX;
            clientY1 += deltaY;
            clientX2 += deltaX;
            clientY2 += deltaY;
        }


        public (int, int) RandomGamefieldPoint()
        {
            return (random.Next(clientX2 - clientX1),
                    random.Next(clientY2 - clientY1));
        }
        public (int, int) RandomVelocity()
        {
            return (random.Next(MinV_X, MaxV_X), random.Next(MinV_Y, MaxV_Y));
        }

        public (int, int) RandomAcceleration()
        {
            return (random.Next(MinA_X, MaxA_X), random.Next(MinA_Y, MaxA_Y));
        }

    }
}
