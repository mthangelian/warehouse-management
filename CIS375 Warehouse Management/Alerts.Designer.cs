namespace CIS375_Warehouse_Management
{
    partial class Alerts
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
            this.lblThreshold = new System.Windows.Forms.Label();
            this.lblThresholdDisplay = new System.Windows.Forms.Label();
            this.lblNewThreshold = new System.Windows.Forms.Label();
            this.txtNewThreshold = new System.Windows.Forms.TextBox();
            this.btnSetThreshold = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(12, 31);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(94, 13);
            this.lblThreshold.TabIndex = 0;
            this.lblThreshold.Text = "Current Threshold:";
            // 
            // lblThresholdDisplay
            // 
            this.lblThresholdDisplay.AutoSize = true;
            this.lblThresholdDisplay.Location = new System.Drawing.Point(134, 31);
            this.lblThresholdDisplay.Name = "lblThresholdDisplay";
            this.lblThresholdDisplay.Size = new System.Drawing.Size(91, 13);
            this.lblThresholdDisplay.TabIndex = 1;
            this.lblThresholdDisplay.Text = "Current Threshold";
            // 
            // lblNewThreshold
            // 
            this.lblNewThreshold.AutoSize = true;
            this.lblNewThreshold.Location = new System.Drawing.Point(12, 68);
            this.lblNewThreshold.Name = "lblNewThreshold";
            this.lblNewThreshold.Size = new System.Drawing.Size(101, 13);
            this.lblNewThreshold.TabIndex = 2;
            this.lblNewThreshold.Text = "Set New Threshold:";
            // 
            // txtNewThreshold
            // 
            this.txtNewThreshold.Location = new System.Drawing.Point(129, 65);
            this.txtNewThreshold.Name = "txtNewThreshold";
            this.txtNewThreshold.Size = new System.Drawing.Size(100, 20);
            this.txtNewThreshold.TabIndex = 3;
            // 
            // btnSetThreshold
            // 
            this.btnSetThreshold.Location = new System.Drawing.Point(129, 101);
            this.btnSetThreshold.Name = "btnSetThreshold";
            this.btnSetThreshold.Size = new System.Drawing.Size(100, 23);
            this.btnSetThreshold.TabIndex = 4;
            this.btnSetThreshold.Text = "Set Threshold";
            this.btnSetThreshold.UseVisualStyleBackColor = true;
            this.btnSetThreshold.Click += new System.EventHandler(this.btnSetThreshold_Click);
            // 
            // Alerts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 147);
            this.Controls.Add(this.btnSetThreshold);
            this.Controls.Add(this.txtNewThreshold);
            this.Controls.Add(this.lblNewThreshold);
            this.Controls.Add(this.lblThresholdDisplay);
            this.Controls.Add(this.lblThreshold);
            this.Name = "Alerts";
            this.Text = "Alerts";
            this.Load += new System.EventHandler(this.Alerts_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblThresholdDisplay;
        private System.Windows.Forms.Label lblNewThreshold;
        private System.Windows.Forms.TextBox txtNewThreshold;
        private System.Windows.Forms.Button btnSetThreshold;
    }
}