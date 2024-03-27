using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

        Vector previousVelocity = new Vector(0, 0);

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
            //find hill segment under player
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
            #endregion

            #region angle calculations
            //find angle player needs to rotate to be flat on ground
            angle = -Math.Atan(tangent);
            GameScreen.rotateAngle = angle * (-180 / Math.PI);
            #endregion

            #region touching ground
            float groundHeight = middleOfPlayerGroundPoint.Y;

            // if player is touching ground set bool to indicate that
            if (y + size >= groundHeight)
            {
                touchingGround = true;

                image = Properties.Resources. _101;

            }
            else
            {
                touchingGround = false;
            }

            //if player is above the ground give it a flying image
            if(y + size + 10 <= groundHeight)
            {
                image = Properties.Resources._30;// _144;
            }
            #endregion

            #region user inputs
            inputs = new Vector(0, 0);

            //store the values given by up and down keys to later adjust the velocity of player
            if (upKey)
            {
                inputs.y+=3;
            }
            if (downKey)
            {
                inputs.y-=3;
            }
            //give boost at the start of game
            if(spaceKey && start)
            {
                inputs.x = 20;

                SoundPlayer buzzer = new SoundPlayer(Properties.Resources.extremely_loud_incorrect_buzzer_0cDaG20);
                buzzer.Play();
                Thread.Sleep(1000);

                start = false;
            }
            #endregion

            #region gravity calculations
            float gY = 3.1f;
            float gX = 0;
            
            Vector gravity = new Vector (gX,  -gY);
            #endregion

            //normal force is used when touching ground
            if (touchingGround)
            {
                #region normal calculations
                //y component of normal force is equal to the y components of all forces going into the ground
                float nY = -gravity.y - inputs.y - velocity.y;
                
                float nX;

                //if ground is downhill
                if (rise > 0)
                {
                    //calculate the horizantal component of normal force
                    nX = (nY + inputs.y) * (float)Math.Tan(angle);
                }
                //if ground is uphill
                else
                {
                    //adjust and calculate x and y components of normal force
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

            #region velocity calculations
            //add all vectors together to find players velocity
            velocity = velocity.Add(gravity).Add(normal).Add(inputs);
            #endregion

            #region if speed skyrockets fix it
            //if accelerates extremely fast for no reason set velocity to the previous velocity
            if (velocity.x - previousVelocity.x > 75 || velocity.x + previousVelocity.x < -75)
            {
                velocity.x = previousVelocity.x;
            }
            previousVelocity = velocity;
            #endregion

            #region terminal velocity
            //set max y speed
            if (velocity.y > 50)
            {
                velocity.y = 50;
            }
            if (velocity.y < -50)
            {
                velocity.y = -50;
            }
            #endregion

            #region movement
            //move player vertically
            y -= velocity.y;
            //horizantal movement is shown through moving hills to the left
            #endregion
        }
    }
}
