using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class MainScreen : UserControl
    {
        public MainScreen()
        {
            InitializeComponent();

            //center screen
            this.Location = new Point(ClientSize.Width / 2 - this.Width / 2, ClientSize.Height / 2 - this.Height / 2);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //open gamescreen
            Form1.ChangeScreen(this, new GameScreen());
        }
    }
}
