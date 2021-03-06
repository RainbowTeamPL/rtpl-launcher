﻿namespace ProjectPonyvilleLauncher
{
    partial class Form1
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
            this.VersionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.InstallBtn = new System.Windows.Forms.Button();
            this.CurrAction = new System.Windows.Forms.Label();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.ChangelogTextBox = new System.Windows.Forms.TextBox();
            this.promoImage1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.promoImage1)).BeginInit();
            this.SuspendLayout();
            // 
            // VersionLabel
            // 
            this.VersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VersionLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.VersionLabel.Location = new System.Drawing.Point(851, 475);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(36, 13);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "#VER";
            this.VersionLabel.Click += new System.EventHandler(this.VersionLabel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "#GAME Launcher";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "WHAT\'S NEW:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(325, 470);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(432, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            // 
            // InstallBtn
            // 
            this.InstallBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallBtn.Location = new System.Drawing.Point(763, 470);
            this.InstallBtn.Name = "InstallBtn";
            this.InstallBtn.Size = new System.Drawing.Size(87, 23);
            this.InstallBtn.TabIndex = 7;
            this.InstallBtn.Text = "Install";
            this.InstallBtn.UseVisualStyleBackColor = true;
            this.InstallBtn.Click += new System.EventHandler(this.InstallBtn_Click);
            // 
            // CurrAction
            // 
            this.CurrAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrAction.AutoSize = true;
            this.CurrAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CurrAction.ForeColor = System.Drawing.SystemColors.Control;
            this.CurrAction.Location = new System.Drawing.Point(325, 452);
            this.CurrAction.Name = "CurrAction";
            this.CurrAction.Size = new System.Drawing.Size(54, 13);
            this.CurrAction.TabIndex = 8;
            this.CurrAction.Text = "#ACTION";
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.Location = new System.Drawing.Point(862, 9);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(25, 25);
            this.SettingsButton.TabIndex = 9;
            this.SettingsButton.Text = "۞";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // ChangelogTextBox
            // 
            this.ChangelogTextBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ChangelogTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChangelogTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ChangelogTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.ChangelogTextBox.Location = new System.Drawing.Point(15, 58);
            this.ChangelogTextBox.Multiline = true;
            this.ChangelogTextBox.Name = "ChangelogTextBox";
            this.ChangelogTextBox.ReadOnly = true;
            this.ChangelogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChangelogTextBox.Size = new System.Drawing.Size(309, 435);
            this.ChangelogTextBox.TabIndex = 10;
            this.ChangelogTextBox.TabStop = false;
            this.ChangelogTextBox.Text = "#CHANGELOG";
            // 
            // promoImage1
            // 
            this.promoImage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.promoImage1.Location = new System.Drawing.Point(337, 79);
            this.promoImage1.Name = "promoImage1";
            this.promoImage1.Size = new System.Drawing.Size(550, 370);
            this.promoImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.promoImage1.TabIndex = 11;
            this.promoImage1.TabStop = false;
            this.promoImage1.WaitOnLoad = true;
            this.promoImage1.Click += new System.EventHandler(this.promoImage1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(899, 505);
            this.Controls.Add(this.promoImage1);
            this.Controls.Add(this.ChangelogTextBox);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.CurrAction);
            this.Controls.Add(this.InstallBtn);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VersionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProjectPonyville Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.promoImage1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button InstallBtn;
        private System.Windows.Forms.Label CurrAction;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.TextBox ChangelogTextBox;
        private System.Windows.Forms.PictureBox promoImage1;
    }
}

