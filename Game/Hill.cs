using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    internal class Hill
    {
        float startHeight;
        float endHeight;
        public float length;
        bool firstHill;
        public List<PointF> curve = new List<PointF>();

        public PointF finalPoint;
        public PointF finalPointPrev;

        public Hill(float startHeight, float endHeight, float length, bool firstHill, PointF finalPointPrev)
        {
            this.startHeight = startHeight;
            this.endHeight = endHeight;
            this.length = length;
            this.firstHill = firstHill;

            this.finalPointPrev = finalPointPrev;
            
            MakeCurve();
        }

        public void MakeCurve()
        {
            //first hill is unique and only do once
            if (firstHill)
            {
                //points at bottom left and right of screen
                curve.Add(new Point(GameScreen.width, GameScreen.height));
                curve.Add(new Point(0, GameScreen.height));

                //slope starts at 0
                float slope = 0;
                float rateOfSlopeChange = (1 / ((length - 200) / 2)) * ((endHeight - startHeight) / 170);

                //make points all along length of screen
                for (int i = 0; i < length; i++)
                {
                    //first 200 pixels are a flat line
                    if (i < 200)
                    {
                        curve.Add(new PointF(i, startHeight));
                    }
                    //curve begins
                    else
                    {
                        curve.Add(new PointF(i, curve[i - 1].Y + slope));
                        slope += rateOfSlopeChange;

                        //halfway through the curve reverse acceleration so it flattens out
                        if (i == ((length - 200) / 2) + 200)
                        {
                            rateOfSlopeChange = -rateOfSlopeChange;
                        }
                    }
                }
            }
            //the rest of the hill segments are randomized
            else
            {
                //points at the bottom right and left points of the screen
                curve.Add(new PointF(finalPointPrev.X + length, GameScreen.height));
                curve.Add(new PointF(finalPointPrev.X, GameScreen.height));

                //use cosine curve and calculations to start at a specific height and end at a height, with a random length
                for (float i = 0; i < length; i += 1f)
                {
                    float x = i + finalPointPrev.X;
                    float y = -((endHeight - startHeight)/2) * (float)(Math.Cos(i / (length / Math.PI))) + (-((endHeight - startHeight) / 2)) + endHeight;

                    curve.Add(new PointF(x, y));
                }
            }
        }

        public void Draw(Graphics g)
        {
            //draw hill
            g.FillPolygon(new SolidBrush(Color.DarkBlue), curve.ToArray());
        }

        public PointF Move(float velocity)
        {
            //store list of points in a temporary list of points with a changed x value based on player velocity
            List<PointF> curveTranslator = new List<PointF>();

            for (int i = 0; i < curve.Count; i++)
            {
                curveTranslator.Add(new PointF(curve[i].X - velocity, curve[i].Y));
            }

            //set curve to new curve and return its final point
            curve.Clear();
            curve = curveTranslator;
            return finalPoint = new PointF(curve[curve.Count - 1].X, curve[curve.Count - 1].Y);
        }
    }
}
