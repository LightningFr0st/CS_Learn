using System.Drawing.Drawing2D;

namespace shapes
{
    internal class Drawfield : DisplayObject
    {
        // outer frame coordinates
        protected int fieldX1, fieldY1;
        protected int fieldX2, fieldY2;

        // client field coordinates
        protected int clientX1, clientY1;
        protected int clientX2, clientY2;


        // Constructor for field with default filling
        public Drawfield(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int borderThickness, int R = 0, int G = 0, int B = 0) :
            base((topLeftX + bottomRightX) / 2, (topLeftY + bottomRightY) / 2,
                bottomRightX - topLeftX, bottomRightY - topLeftY, R, G, B)
        {
            fieldX1 = topLeftX;
            fieldY1 = topLeftY;
            fieldX2 = bottomRightX;
            fieldY2 = bottomRightY;

            clientX1 = fieldX1 + borderThickness;
            clientY1 = fieldY1 + borderThickness;
            clientX2 = fieldX2 - borderThickness;
            clientY2 = fieldY2 - borderThickness;
        }


        protected override void UpdateFrame()
        {
            // form the point collection 
            int[] coordsX = [ fieldX1, fieldX2, fieldX1, fieldX2 ];
            int[] coordsY = [ fieldY1, fieldY1, fieldY2, fieldY2 ];

            // pass it to the method
            SetFrameFromPoints(x1_anch, y1_anch, coordsX, coordsY);
        }

        public override void Draw(Graphics g)
        {
            Brush strokeBrush = GetStrokeBrush();
            Brush fillBrush = GetFillBrush();

            GraphicsState prevState = MatrixRotate(g);
            g.TranslateTransform(fieldX1, fieldY1);

            // Drawing the outer frame
            Rectangle rectOuter = new Rectangle(0, 0, fieldX2 - fieldX1, fieldY2 - fieldY1);
            g.FillRectangle(strokeBrush, rectOuter);
            g.DrawRectangle(new Pen(strokeBrush), rectOuter);

            // Drawing the client area            
            g.TranslateTransform(clientX1 - x1_frame, clientY1 - y1_frame);
            Rectangle rectClient = new Rectangle(0, 0, clientX2 - clientX1, clientY2 - clientY1);
            g.FillRectangle(fillBrush, rectClient);
            g.DrawRectangle(new Pen(fillBrush), rectClient);

            g.Restore(prevState);
        }
    }
}
