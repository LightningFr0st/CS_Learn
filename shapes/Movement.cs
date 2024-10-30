using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

const int LEFT = -1;
const int RIGHT = 1;
const int UP = -1;
const int DOWN = 1;

namespace shapes
{
    internal class Movement
    {
        Vector velocity;
        Vector accel_v;
        public int speed;
        public int accel;
        public bool accelerated = false;
        DisplayObject cur;


        public Movement(DisplayObject cur, int v_x, int v_y, int a_x = 0, int a_y = 0)
        {
            velocity = new Vector(v_x, v_y);
            Vector accel_v = new Vector(a_x, a_y);
        }
        protected void updatePos() 
        {
            if (!accelerated)
            {
                cur.x1_frame += velocity.x * speed;
                cur.y1_frame += velocity.y * speed;
                cur.x2_frame += velocity.x * speed;
                cur.y2_frame += velocity.y * speed;
            }
            
        }
    }
}
