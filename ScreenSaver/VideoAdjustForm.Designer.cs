namespace ScreenSaver
{
    partial class VideoAdjustForm
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
            this.lblBrightness = new System.Windows.Forms.Label();
            this.trkBrightness = new System.Windows.Forms.TrackBar();
            this.lblContrast = new System.Windows.Forms.Label();
            this.trkContrast = new System.Windows.Forms.TrackBar();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.trkSaturation = new System.Windows.Forms.TrackBar();
            this.lblHue = new System.Windows.Forms.Label();
            this.trkHue = new System.Windows.Forms.TrackBar();
            this.lblGamma = new System.Windows.Forms.Label();
            this.trkGamma = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trkSpeed = new System.Windows.Forms.TrackBar();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Location = new System.Drawing.Point(12, 12);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(56, 13);
            this.lblBrightness.TabIndex = 0;
            this.lblBrightness.Text = "Brightness";
            // 
            // trkBrightness
            // 
            this.trkBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkBrightness.Location = new System.Drawing.Point(8, 28);
            this.trkBrightness.Maximum = 200;
            this.trkBrightness.Minimum = 0;
            this.trkBrightness.Name = "trkBrightness";
            this.trkBrightness.Size = new System.Drawing.Size(268, 45);
            this.trkBrightness.TabIndex = 1;
            this.trkBrightness.TickFrequency = 20;
            this.trkBrightness.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // lblContrast
            // 
            this.lblContrast.AutoSize = true;
            this.lblContrast.Location = new System.Drawing.Point(12, 74);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(56, 13);
            this.lblContrast.TabIndex = 2;
            this.lblContrast.Text = "Contrast";
            // 
            // trkContrast
            // 
            this.trkContrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkContrast.Location = new System.Drawing.Point(8, 90);
            this.trkContrast.Maximum = 200;
            this.trkContrast.Minimum = 0;
            this.trkContrast.Name = "trkContrast";
            this.trkContrast.Size = new System.Drawing.Size(268, 45);
            this.trkContrast.TabIndex = 3;
            this.trkContrast.TickFrequency = 20;
            this.trkContrast.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Location = new System.Drawing.Point(12, 136);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(56, 13);
            this.lblSaturation.TabIndex = 4;
            this.lblSaturation.Text = "Saturation";
            // 
            // trkSaturation
            // 
            this.trkSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkSaturation.Location = new System.Drawing.Point(8, 152);
            this.trkSaturation.Maximum = 300;
            this.trkSaturation.Minimum = 0;
            this.trkSaturation.Name = "trkSaturation";
            this.trkSaturation.Size = new System.Drawing.Size(268, 45);
            this.trkSaturation.TabIndex = 5;
            this.trkSaturation.TickFrequency = 30;
            this.trkSaturation.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Location = new System.Drawing.Point(12, 198);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(56, 13);
            this.lblHue.TabIndex = 6;
            this.lblHue.Text = "Hue";
            // 
            // trkHue
            // 
            this.trkHue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkHue.Location = new System.Drawing.Point(8, 214);
            this.trkHue.Maximum = 180;
            this.trkHue.Minimum = -180;
            this.trkHue.Name = "trkHue";
            this.trkHue.Size = new System.Drawing.Size(268, 45);
            this.trkHue.TabIndex = 7;
            this.trkHue.TickFrequency = 30;
            this.trkHue.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // lblGamma
            // 
            this.lblGamma.AutoSize = true;
            this.lblGamma.Location = new System.Drawing.Point(12, 260);
            this.lblGamma.Name = "lblGamma";
            this.lblGamma.Size = new System.Drawing.Size(56, 13);
            this.lblGamma.TabIndex = 8;
            this.lblGamma.Text = "Gamma";
            // 
            // trkGamma
            // 
            this.trkGamma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkGamma.Location = new System.Drawing.Point(8, 276);
            this.trkGamma.Maximum = 300;
            this.trkGamma.Minimum = 1;
            this.trkGamma.Name = "trkGamma";
            this.trkGamma.Size = new System.Drawing.Size(268, 45);
            this.trkGamma.TabIndex = 9;
            this.trkGamma.TickFrequency = 30;
            this.trkGamma.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(12, 322);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(56, 13);
            this.lblSpeed.TabIndex = 10;
            this.lblSpeed.Text = "Speed";
            // 
            // trkSpeed
            // 
            this.trkSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkSpeed.Location = new System.Drawing.Point(8, 338);
            this.trkSpeed.Maximum = 400;
            this.trkSpeed.Minimum = 25;
            this.trkSpeed.Name = "trkSpeed";
            this.trkSpeed.Size = new System.Drawing.Size(268, 45);
            this.trkSpeed.TabIndex = 11;
            this.trkSpeed.TickFrequency = 25;
            this.trkSpeed.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(201, 388);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // VideoAdjustForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 423);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.trkBrightness);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.trkContrast);
            this.Controls.Add(this.lblContrast);
            this.Controls.Add(this.trkSaturation);
            this.Controls.Add(this.lblSaturation);
            this.Controls.Add(this.trkHue);
            this.Controls.Add(this.lblHue);
            this.Controls.Add(this.trkGamma);
            this.Controls.Add(this.lblGamma);
            this.Controls.Add(this.trkSpeed);
            this.Controls.Add(this.lblSpeed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoAdjustForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Video adjustments";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VideoAdjustForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.TrackBar trkBrightness;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.TrackBar trkContrast;
        private System.Windows.Forms.Label lblSaturation;
        private System.Windows.Forms.TrackBar trkSaturation;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.TrackBar trkHue;
        private System.Windows.Forms.Label lblGamma;
        private System.Windows.Forms.TrackBar trkGamma;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trkSpeed;
        private System.Windows.Forms.Button btnReset;
    }
}
