namespace ProjectPonyvilleLauncher
{
    partial class GameSelection
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
            this.PPGameBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PPGameBtn
            // 
            this.PPGameBtn.Location = new System.Drawing.Point(12, 12);
            this.PPGameBtn.Name = "PPGameBtn";
            this.PPGameBtn.Size = new System.Drawing.Size(250, 75);
            this.PPGameBtn.TabIndex = 0;
            this.PPGameBtn.Text = "ProjectPonyville";
            this.PPGameBtn.UseVisualStyleBackColor = true;
            this.PPGameBtn.Click += new System.EventHandler(this.PPGameBtn_Click);
            // 
            // GameSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 261);
            this.ControlBox = false;
            this.Controls.Add(this.PPGameBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "GameSelection";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Game...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.GameSelection_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PPGameBtn;
    }
}