using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    public abstract class DisplayObject
    {
        //coordinates for a frame
        public int x1_frame;
        public int y1_frame;
        public int x2_frame;
        public int y2_frame;

        //rotation angle
        protected int angle;

        //anchor point
        protected int x1_anch;
        protected int y1_anch;

        //colors
        public Color fillColor;
        public Color strokeColor;
        public int strokeThick;

        public void SetAngle(int p_angle)
        {
            angle = p_angle;
            UpdateFrame();
        }

        public void SetThick(int p_thick)
        {
            strokeThick = p_thick;
        }

        public void SetFillColor(int red, int green, int blue)
        {
            fillColor = Color.FromArgb(red, green, blue);
        }

        public void SetStrokeColor(int red, int green, int blue)
        {
            strokeColor = Color.FromArgb(red, green, blue);
        }

        public DisplayObject(int anchX, int anchY, int width, int height, int R = 0, int G = 0, int B = 0)
        {
            x1_anch = anchX;
            y1_anch = anchY;
            x1_frame = anchX - width / 2;
            y1_frame = anchY - height / 2;
            x2_frame = x1_frame + width;
            y2_frame = y1_frame + height;
            angle = 0;
            fillColor = Color.FromArgb(R, G, B);
            strokeColor = Color.FromArgb(R, G, B);
        }

        protected Brush GetFillBrush()
        {
            return new SolidBrush(fillColor);
        }

        protected Brush GetStrokeBrush()
        {
            return new SolidBrush(strokeColor);
        }

        protected abstract void UpdateFrame();

        protected void SetFrameFromPoints(int rotPointX, int rotPointY, int[] pointsX, int[] pointsY)
        {

            int? minX = null, minY = null, maxX = null, maxY = null;

            int tempX, tempY;
            double angle = Vector.Rad(this.angle);
            for (int i = 0; i < pointsX.Length; i++)
            {
                (tempX, tempY) = Vector.getVector(rotPointX, rotPointY, pointsX[i], pointsY[i]);
                (tempX, tempY) = Vector.rotateVector(tempX, tempY, angle);
                minX = minX == null ? tempX : (tempX < minX ? tempX : minX);
                maxX = maxX == null ? tempX : (tempX > maxX ? tempX : maxX);
                minY = minY == null ? tempY : (tempY < minY ? tempY : minY);
                maxY = maxY == null ? tempY : (tempY > maxY ? tempY : maxY);
            }

            x1_frame = rotPointX + (minX.HasValue ? minX.Value : 0);
            y1_frame = rotPointY + (minY.HasValue ? minY.Value : 0);
            x2_frame = rotPointX + (maxX.HasValue ? maxX.Value : 0);
            y2_frame = rotPointY + (maxY.HasValue ? maxY.Value : 0);
        }

        public abstract void Draw(Graphics G);

        protected GraphicsState MatrixRotate(Graphics g)
        {
            GraphicsState prevState = g.Save();

            Matrix m = new Matrix(g.TransformElements);
            m.RotateAt(angle, new PointF(x1_anch, y1_anch));
            g.Transform = m;
            return prevState;
        }

        //projection of acceleration
        public int a_x;
        public int a_y;

        //projection of velocity
        public int v_x;
        public int v_y;

        
    }
}
