namespace RDIndexManager
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
            this.BrowseTextBox = new System.Windows.Forms.TextBox();
            this.BrowseFolderBtn = new System.Windows.Forms.Button();
            this.MakeBtn = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BrowseTextBox
            // 
            this.BrowseTextBox.Location = new System.Drawing.Point(12, 12);
            this.BrowseTextBox.Name = "BrowseTextBox";
            this.BrowseTextBox.Size = new System.Drawing.Size(795, 20);
            this.BrowseTextBox.TabIndex = 0;
            // 
            // BrowseFolderBtn
            // 
            this.BrowseFolderBtn.Location = new System.Drawing.Point(813, 10);
            this.BrowseFolderBtn.Name = "BrowseFolderBtn";
            this.BrowseFolderBtn.Size = new System.Drawing.Size(75, 23);
            this.BrowseFolderBtn.TabIndex = 1;
            this.BrowseFolderBtn.Text = "Browse";
            this.BrowseFolderBtn.UseVisualStyleBackColor = true;
            this.BrowseFolderBtn.Click += new System.EventHandler(this.BrowseFolderBtn_Click);
            // 
            // MakeBtn
            // 
            this.MakeBtn.Location = new System.Drawing.Point(12, 38);
            this.MakeBtn.Name = "MakeBtn";
            this.MakeBtn.Size = new System.Drawing.Size(876, 23);
            this.MakeBtn.TabIndex = 2;
            this.MakeBtn.Text = "Make";
            this.MakeBtn.UseVisualStyleBackColor = true;
            this.MakeBtn.Click += new System.EventHandler(this.MakeBtn_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(12, 67);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(876, 182);
            this.LogTextBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 261);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.MakeBtn);
            this.Controls.Add(this.BrowseFolderBtn);
            this.Controls.Add(this.BrowseTextBox);
            this.Name = "Form1";
            this.Text = "RDIndex Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BrowseTextBox;
        private System.Windows.Forms.Button BrowseFolderBtn;
        private System.Windows.Forms.Button MakeBtn;
        private System.Windows.Forms.TextBox LogTextBox;
    }
}

