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
            if (firstHill)
            {
                curve.Add(new Point(GameScreen.width, GameScreen.height));
                curve.Add(new Point(0, GameScreen.height));

                float slope = 0;
                float rateOfSlopeChange = (1 / ((length - 200) / 2)) * ((endHeight - startHeight) / 170);

                for (int i = 0; i < length; i++)
                {
                    if (i < 200)
                    {
                        curve.Add(new PointF(i, startHeight));
                    }
                    else
                    {
                        curve.Add(new PointF(i, curve[i - 1].Y + slope));
                        slope += rateOfSlopeChange;

                        if (i == ((length - 200) / 2) + 200)
                        {
                            rateOfSlopeChange = -rateOfSlopeChange;
                        }
                    }
                }
            }
            else
            {
                curve.Add(new PointF(finalPointPrev.X + length, GameScreen.height));
                curve.Add(new PointF(finalPointPrev.X, GameScreen.height));

                for (float i = 0; i < length; i += 1f)
                {
                    float x = i + finalPointPrev.X;
                    float y = -((endHeight - startHeight)/2) * (float)(Math.Cos(i / (length / Math.PI))) + (-((endHeight - startHeight) / 2)) + endHeight;

                    curve.Add(new PointF(x, y));
                }


                //curve.Add(new PointF(GameScreen.width, startHeight));

                //float slope = 0;
                //float rateOfSlopeChange = (1 / ((length - 200) / 2)) * ((endHeight - startHeight) / 170);

                //for (int i = 1; i < length; i++)
                //{
                //    curve.Add(new PointF(GameScreen.width + i, curve[i + 1].Y + slope));
                //    slope += rateOfSlopeChange;

                //    if (i == ((length - 200) / 2) + 200)
                //    {
                //        rateOfSlopeChange = -rateOfSlopeChange;
                //    }
                //}
            }
        }

        public void Draw(Graphics g)
        {
            g.FillPolygon(new SolidBrush(Color.Black), curve.ToArray());
        }

        public PointF Move(float velocity)
        {
            List<PointF> curveTranslator = new List<PointF>();

            for (int i = 0; i < curve.Count; i++)
            {
                curveTranslator.Add(new PointF(curve[i].X - velocity, curve[i].Y));
            }

            curve.Clear();
            curve = curveTranslator;
            return finalPoint = new PointF(curve[curve.Count - 1].X, curve[curve.Count - 1].Y);
        }
    }
}
