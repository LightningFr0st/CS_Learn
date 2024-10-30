using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shapes
{
    public abstract class DisplayObject
    {

        public string tag;

        //coordinates for a frame
        public int x1_frame;
        public int y1_frame;
        public int x2_frame;
        public int y2_frame;

        //rotation angle
        protected int angle;

        //anchor point
        public int x1_anch;
        public int y1_anch;


        public int x1_frame_s;
        public int y1_frame_s;
        public int x2_frame_s;
        public int y2_frame_s;


        public int x1_anch_s;
        public int y1_anch_s;


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
            x1_anch_s = anchX;
            y1_anch_s = anchY;
            x1_frame_s = anchX - width / 2;
            y1_frame_s = anchY - height / 2;
            x2_frame_s = x1_frame_s + width;
            y2_frame_s = y1_frame_s + height;


            x1_anch = x1_anch_s;
            y1_anch = y1_anch_s;
            x1_frame = x1_frame_s;
            y1_frame = y1_frame_s;
            x2_frame = x2_frame_s;
            y2_frame = y2_frame_s;

            v_x = 0;
            v_y = 0;

            a_x = 0;
            a_y = 0;

            angle = 0;

            time_created = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            fillColor = Color.FromArgb(R, G, B);
            strokeColor = Color.FromArgb(0, 0, 0);
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
        //movement data
        public double movangl;
        public double v;
        public double a;
        public long time_created;
        //Movement projection
        public double v_x;
        public double v_y;
        public double v_x_s;
        public double v_y_s;
        public double a_x;
        public double a_y;

        public void setMovement(double p_v, double p_a, double angle)
        {
            v = p_v;
            a = p_a;
            movangl = (angle * Math.PI) / 180;

            v_x_s = (v * Math.Cos(movangl));
            v_y_s = -(v * Math.Sin(movangl));

            a_x = (a * Math.Cos(movangl));
            a_y = -(a * Math.Sin(movangl));
        }
        public bool IsInBounds(int fieldX1, int fieldY1, int fieldX2, int fieldY2)
        {
            return
                x1_anch > fieldX1 &&
                y1_anch > fieldY1 &&
                x1_anch < fieldX2 &&
                y1_anch < fieldY2;
        }
        public void move(double det)
        {
            long curtime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            double dt = ((double)curtime - time_created) / 2000;

            int dx = (int)Math.Round(dt * ((a_x * dt) + v_x));
            int dy = (int)Math.Round(dt * ((a_y * dt) + v_y));

            if (2 * dt < det)
            {
                v_x = v_x_s + (a_x * dt);
                v_y = v_y_s + (a_y * dt);

                x1_anch = x1_anch_s + dx;
                y1_anch = y1_anch_s + dy;

                x1_frame = x1_frame_s + dx;
                y1_frame = y1_frame_s + dy;
                x2_frame = x2_frame_s + dx;
                y2_frame = y2_frame_s + dy;

                ShiftPoints(dx, dy);
            }
                
        }
        public void RenewPos(int dx, int dy)
        {
            x1_anch += dx;
            y1_anch += dy;
            x1_frame += dx;
            y1_frame += dy;
            x2_frame += dx;
            y2_frame += dy;

            x1_anch_s = x1_anch;
            y1_anch_s = y1_anch;
            x1_frame_s = x1_frame;
            y1_frame_s = y1_frame;
            x2_frame_s = x2_frame;
            y2_frame_s = y2_frame;

            RenewPoints(dx, dy);

            time_created = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        protected abstract void RenewPoints(int dx, int dy);
        protected abstract void ShiftPoints(int dx, int dy);
    }
}
