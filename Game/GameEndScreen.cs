using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class GameEndScreen : UserControl
    {
        public GameEndScreen()
        {
            InitializeComponent();
            //display final score
            scoreLabel.Text = $"Your score was {Form1.distanceTravelled}!";
        }

        private void mainButton_Click(object sender, EventArgs e)
        {
            //back to main screen
            Form1.ChangeScreen(this, new MainScreen());
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            //leave application
            Application.Exit();
        }
    }
}
