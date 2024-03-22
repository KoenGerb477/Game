namespace Game
{
    partial class GameScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.troubleshoot1 = new System.Windows.Forms.Label();
            this.troubleshoot2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 20;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // troubleshoot1
            // 
            this.troubleshoot1.AutoSize = true;
            this.troubleshoot1.ForeColor = System.Drawing.Color.Black;
            this.troubleshoot1.Location = new System.Drawing.Point(76, 47);
            this.troubleshoot1.Name = "troubleshoot1";
            this.troubleshoot1.Size = new System.Drawing.Size(51, 20);
            this.troubleshoot1.TabIndex = 0;
            this.troubleshoot1.Text = "label1";
            // 
            // troubleshoot2
            // 
            this.troubleshoot2.AutoSize = true;
            this.troubleshoot2.Location = new System.Drawing.Point(216, 47);
            this.troubleshoot2.Name = "troubleshoot2";
            this.troubleshoot2.Size = new System.Drawing.Size(51, 20);
            this.troubleshoot2.TabIndex = 1;
            this.troubleshoot2.Text = "label2";
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.troubleshoot2);
            this.Controls.Add(this.troubleshoot1);
            this.DoubleBuffered = true;
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(1800, 1100);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.GameScreen_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label troubleshoot1;
        private System.Windows.Forms.Label troubleshoot2;
    }
}
