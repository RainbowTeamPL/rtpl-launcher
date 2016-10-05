namespace LauncherUpdate
{
    partial class Updater
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BuildNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BuildNumberLabel
            // 
            this.BuildNumberLabel.AutoSize = true;
            this.BuildNumberLabel.Location = new System.Drawing.Point(12, 9);
            this.BuildNumberLabel.Name = "BuildNumberLabel";
            this.BuildNumberLabel.Size = new System.Drawing.Size(46, 13);
            this.BuildNumberLabel.TabIndex = 0;
            this.BuildNumberLabel.Text = "#BUILD";
            // 
            // Updater
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(205, 26);
            this.ControlBox = false;
            this.Controls.Add(this.BuildNumberLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Updater";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Updater_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BuildNumberLabel;
    }
}

