using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    internal class Player
    {
        public float x, y, size;

        public Vector normal = new Vector(0, 0);
        public Vector inputs;

        bool touchingGround = false;

        public float rise, run;
        public PointF middleOfPlayerGroundPoint;

        public Vector velocity = new Vector(0, 0);

        public double angle;

        public bool leftKey, rightKey, downKey, upKey, spaceKey;

        public Image image;

        public bool start = true;

        public Player(int x, int y, Image image)
        {
            this.x = x;
            this.y = y;

            size = 30;
            velocity.y = 0;
            this.image = image;
        }

        public void Move(List<Hill>hillSegments)
        {
            #region tangent calculations
            Hill hillSegment = new Hill(0, 0, 0, false, new Point(0,0));
            foreach (Hill h in hillSegments)
            {
                if (h.curve[h.curve.Count -1].X < x + h.length && h.curve[h.curve.Count -1].X > x)
                {
                    hillSegment = h;
                    break;
                }
            }

            //find tangent of ground below player
            float x1 = x + size / 2;
            float x2 = x1 + 1;
            float y1 = 0;
            float y2 = 0;

            for(int i = 0; i < hillSegment.curve.Count; i++)
            {
                if (Convert.ToInt32(hillSegment.curve[i].X) == Convert.ToInt32(x1))
                {
                    y1 = hillSegment.curve[i].Y;
                }
                else if(Convert.ToInt32(hillSegment.curve[i].X) == Convert.ToInt32(x2))
                {
                    y2 = hillSegment.curve[i].Y;
                }
            }

            rise = (y2 - y1);
            run = (x2 - x1);

            middleOfPlayerGroundPoint = new PointF(x1, y1);

            float tangent = rise / run;
            float perpTangent = -run / rise;
            #endregion

            #region angle calculations
            //find angle player needs to rotate to be flat on ground
            angle = -Math.Atan(tangent);
            GameScreen.rotateAngle = angle * (-180 / Math.PI);
            #endregion

            #region touching ground
            float groundHeight = middleOfPlayerGroundPoint.Y;

            if (y + size >= groundHeight)
            {
                touchingGround = true;

                image = Properties.Resources._101;
            }
            else
            {
                touchingGround = false;
                image = Properties.Resources._144;
            }
            #endregion

            #region user inputs
            inputs = new Vector(0, 0);

            if (upKey)
            {
                inputs.y+=3;
            }
            if (downKey)
            {
                inputs.y-=3;
            }
            if(spaceKey && start)
            {
                inputs.x = 20;
                start = false;
            }
            //if (leftKey)
            //{
            //    inputs.x--;
            //}
            //if (rightKey)
            //{
            //    if(velocity.x < 50)
            //    {
            //        inputs.x++;
            //    }
            //}
            #endregion

            #region gravity calculations
            float gY = 3.1f;
            float gX = 0;
            
            Vector gravity = new Vector (gX,  -gY);
            #endregion

            if (touchingGround)
            {
                #region normal calculations
                float nY = -gravity.y - inputs.y - velocity.y;

                float nX;
                if (rise > 0)
                {
                    nX = (nY + inputs.y) * (float)Math.Tan(angle);
                }
                else
                {
                    nY += velocity.x * (float)Math.Tan(angle);
                    nX = (nY + inputs.y) * (float)Math.Tan(angle);
                }

                //float nX = 0;
                normal = new Vector (-nX, nY);
                #endregion
            }
            else
            {
                normal = new Vector(0, 0);
            }

            #region friction calculations
            //Vector friction = new Vector(normal.y, -normal.x).Multiply(1f);
            //if (velocity.x > 0)
            //{
            //    friction.x = -Math.Abs(velocity.x);
            //}
            //else
            //{
            //    friction.x = Math.Abs(velocity.x);
            //}
            Vector friction = new Vector (0, 0);
            if (touchingGround)
            {
                if (velocity.x > 0)
                {
                    friction = new Vector(-velocity.x + 0.000001f, 0);
                }
                else if (velocity.x < 0)
                {
                    friction = new Vector(-velocity.x - 0.000001f, 0);
                }
            }
            else
            {
                friction = new Vector(0, 0);
            }
            #endregion

            #region velocity calculations
            velocity = velocity.Add(gravity).Add(normal).Add(inputs);//.Add(friction);

            #endregion

            #region movement
            y -= velocity.y;
            #endregion
        }

        public void Jump()
        {
            if (touchingGround)
            {
                velocity.y = 10;
            }

            touchingGround = false;
        }
    }
}
