namespace AknakeresoWF
{
    partial class BoardForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartButton = new System.Windows.Forms.Button();
            this.BoardDisplay = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(190, 587);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(169, 87);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Save Game";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BoardDisplay
            // 
            this.BoardDisplay.Location = new System.Drawing.Point(28, 37);
            this.BoardDisplay.Name = "BoardDisplay";
            this.BoardDisplay.Size = new System.Drawing.Size(511, 509);
            this.BoardDisplay.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(169, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Next player: FirstPlayer";
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 688);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoardDisplay);
            this.Controls.Add(this.StartButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BoardForm";
            this.Text = "Aknakereso";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button StartButton;
        private Panel BoardDisplay;
        private Label label1;
    }
}