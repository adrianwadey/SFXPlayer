namespace SFXPlayer {
    partial class ucVolume {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tbVolume = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(0, 0);
            this.tbVolume.Maximum = 100;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbVolume.Size = new System.Drawing.Size(45, 150);
            this.tbVolume.TabIndex = 0;
            this.tbVolume.TickFrequency = 10;
            this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
            this.tbVolume.Enter += new System.EventHandler(this.tbVolume_Enter);
            this.tbVolume.Leave += new System.EventHandler(this.tbVolume_Leave);
            // 
            // ucVolume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbVolume);
            this.Name = "ucVolume";
            this.Size = new System.Drawing.Size(49, 153);
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbVolume;
    }
}
