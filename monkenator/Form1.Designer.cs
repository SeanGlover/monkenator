namespace monkenator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PausePlay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PausePlay)).BeginInit();
            this.SuspendLayout();
            // 
            // PausePlay
            // 
            this.PausePlay.BackColor = System.Drawing.Color.Transparent;
            this.PausePlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PausePlay.BackgroundImage")));
            this.PausePlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PausePlay.Location = new System.Drawing.Point(12, 12);
            this.PausePlay.Name = "PausePlay";
            this.PausePlay.Size = new System.Drawing.Size(267, 228);
            this.PausePlay.TabIndex = 0;
            this.PausePlay.TabStop = false;
            this.PausePlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PausePlay_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 703);
            this.Controls.Add(this.PausePlay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PausePlay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PausePlay;
    }
}

