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
using System.Threading;
using System.Media;


namespace Game
{
    public partial class GameScreen : UserControl
    {
        //make player
        Player player;

        //store height and width of screen publicly
        public static int height;
        public static int width;

        //angle penguin is drawn at
        public static double rotateAngle = 0;

        //final points of hill segments (point at the far right of hill segment)
        List<PointF> finalPointHillSegments = new List<PointF>();

        //list of hill segments
        List<Hill> hillSegments = new List<Hill>();

        //variable to alternate uphill or downhill
        int hillDirectionCounter = 0;

        public GameScreen()
        {
            InitializeComponent();

            //make player at start position with image
            player = new Player(50, 200, Properties.Resources._147);

            //size up the screen and store public
            this.Width = ClientSize.Width;
            this.Height = ClientSize.Height;
            height = this.Height;
            width = this.Width;

            //make first hill segment (it is different than the rest of them)
            Hill firstHillSegment = new Hill(300, this.Height - 100, this.Width, true, new Point(0,0));
            hillSegments.Add(firstHillSegment);

            //reset distance travelled
            Form1.distanceTravelled = 0;
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw hill segments
            foreach (Hill hill in hillSegments)
            {
                hill.Draw(e.Graphics);
            }

            //draw player
            e.Graphics.TranslateTransform(player.x + player.size / 2, player.y);
            e.Graphics.RotateTransform((float)rotateAngle);
            e.Graphics.DrawImage(player.image, 0, 0, player.size + 20, player.size);
            e.Graphics.ResetTransform();

            //display instructions before starting
            if (player.start)
            {
                e.Graphics.DrawString($"Press Space to Begin", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(50, 150));

                e.Graphics.DrawRectangle(new Pen(Color.Black, 5), 380, 130, 650, 110);
                e.Graphics.DrawString($"Press up arrow key to make the penguin lighter.", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(400, 150));
                e.Graphics.DrawString($"Press down arrow key to make the penguin heavier.", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(400, 175));
                e.Graphics.DrawString($"Hint: Press the down arrow key on downhills and the up arrow key on uphills.", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(400, 200));

            }

            //display distance travelled
            e.Graphics.DrawString($"{Form1.distanceTravelled}", new Font(new FontFamily("Arial"), 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(this.Width - 100, 75));

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player, passing hill segments so player knows where ground is
            player.Move(hillSegments);

            //move hill segments based on players x velocity
            foreach (Hill h in hillSegments)
            {
                h.Move(player.velocity.x);

                //find the final point of hills and store it
                finalPointHillSegments.Add(h.curve[h.curve.Count - 1]);
            }

            //if the final points of any hill is off screen to the left, hill segment is removed
            for (int i = 0; i < finalPointHillSegments.Count; i++)
            {
                if (finalPointHillSegments[i].X < 0)
                {
                    finalPointHillSegments.RemoveAt(i);
                    hillSegments.RemoveAt(i);
                }
            }

            //final point of last hill segment is now on screen, so make a new hill segment
            if (finalPointHillSegments[finalPointHillSegments.Count - 1].X < this.Width - 1)
            {
                Random random = new Random();

                PointF finalPoint = finalPointHillSegments[finalPointHillSegments.Count - 1];
                float startHeight = finalPoint.Y;
                float endHeight;
                float length = random.Next(this.Width / 2, this.Width);
                
                //if previous hill was uphill do downhill and vice versa
                if (hillDirectionCounter == 0)
                {
                    hillDirectionCounter++;

                    //end height is greater than start height
                    endHeight = random.Next(100, Convert.ToInt32(startHeight));
                }
                else
                {
                    hillDirectionCounter--;

                    //end height is less than start height
                    endHeight = random.Next(Convert.ToInt32(startHeight), this.Height - 100);
                }

                //make hill with random start height, end height, length. Give it final point of last hill segment
                hillSegments.Add(new Hill(startHeight, endHeight, length, false, finalPoint));
            }

            finalPointHillSegments.Clear();

            //add onto score based on velocity of player
            Form1.distanceTravelled += Convert.ToInt32(player.velocity.x / 10);

            Refresh();

            //game over if go backwards too far
            if (hillSegments[0].curve[2].X > 0)
            {
                gameTimer.Stop();
                Thread.Sleep(1000);
                Form1.ChangeScreen(this, new GameEndScreen());
            }

        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //key pressing and whatnot
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
                //leave if escape
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
        }
    }
}
