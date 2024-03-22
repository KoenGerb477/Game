using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Game
{
    public partial class GameScreen : UserControl
    {
        Player player;
        public static int height;
        public static int width;

        public static double rotateAngle = 0;

        List<PointF> finalPointHillSegments = new List<PointF>();
        List<Hill> hillSegments = new List<Hill>();

        int hillDirectionCounter = 0;

        int distanceTraveled = 0;

        public GameScreen()
        {
            InitializeComponent();

            player = new Player(50, 200, Properties.Resources._147);

            height = this.Height;
            width = this.Width;

            Hill firstHillSegment = new Hill(300, this.Height - 100, this.Width, true, new Point(0,0));
            hillSegments.Add(firstHillSegment);
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Hill hill in hillSegments)
            {
                hill.Draw(e.Graphics);
            }

            e.Graphics.TranslateTransform(player.x + player.size / 2, player.y);
            e.Graphics.RotateTransform((float)rotateAngle);
            e.Graphics.DrawImage(player.image, 0, 0, player.size + 20, player.size);
            e.Graphics.ResetTransform();

            if (player.start)
            {
                e.Graphics.DrawString($"Press Space to Begin", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(50, 150));
            }

            e.Graphics.DrawString($"{distanceTraveled}", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(this.Width - 100, 75));

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            player.Move(hillSegments);


            foreach (Hill h in hillSegments)
            {
                h.Move(player.velocity.x);

                finalPointHillSegments.Add(h.curve[h.curve.Count - 1]);
            }

            for (int i = 0; i < finalPointHillSegments.Count; i++)
            {
                if (finalPointHillSegments[i].X < -this.Width)
                {
                    finalPointHillSegments.RemoveAt(i);
                    hillSegments.RemoveAt(i);
                }
            }

            if (finalPointHillSegments[finalPointHillSegments.Count - 1].X < this.Width - 1)
            {
                Random random = new Random();

                PointF finalPoint = finalPointHillSegments[finalPointHillSegments.Count - 1];
                float startHeight = finalPoint.Y;
                float endHeight;
                float length = random.Next(this.Width / 2, this.Width);
                
                if (hillDirectionCounter == 0)
                {
                    hillDirectionCounter++;

                    endHeight = random.Next(100, Convert.ToInt32(startHeight));
                }
                else
                {
                    hillDirectionCounter--;

                    endHeight = random.Next(Convert.ToInt32(startHeight), this.Height - 100);
                }

                hillSegments.Add(new Hill(startHeight, endHeight, length, false, finalPoint));
            }
            //this.Height - 100, random.Next(100, this.Height - 100)

            finalPointHillSegments.Clear();

            distanceTraveled += Convert.ToInt32(player.velocity.x / 10);

            Refresh();
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.leftKey = true;
                    break;
                case Keys.Right:
                    player.rightKey = true;
                    break;
                case Keys.Up:
                    player.upKey = true;
                    break;
                case Keys.Down:
                    player.downKey = true;
                    break;
                case Keys.Space:
                    player.spaceKey = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.leftKey = false;
                    break;
                case Keys.Right:
                    player.rightKey = false;
                    break;
                case Keys.Up:
                    player.upKey = false;
                    break;
                case Keys.Down:
                    player.downKey = false;
                    break;
                case Keys.Space:
                    player.spaceKey = false;
                    break;
            }
        }
    }
}
