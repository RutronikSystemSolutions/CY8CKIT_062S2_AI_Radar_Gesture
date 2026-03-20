namespace RadarSensorGesture
{
    partial class ThresholdConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThresholdConfigurationForm));
            thresholdTopLabel = new Label();
            thresholdTopTextBox = new TextBox();
            thresholdBotTextBox = new TextBox();
            thresholdBotLabel = new Label();
            setButton = new Button();
            SuspendLayout();
            // 
            // thresholdTopLabel
            // 
            thresholdTopLabel.AutoSize = true;
            thresholdTopLabel.Location = new Point(12, 15);
            thresholdTopLabel.Name = "thresholdTopLabel";
            thresholdTopLabel.Size = new Size(84, 15);
            thresholdTopLabel.TabIndex = 0;
            thresholdTopLabel.Text = "Threshold top:";
            // 
            // thresholdTopTextBox
            // 
            thresholdTopTextBox.Location = new Point(102, 12);
            thresholdTopTextBox.Name = "thresholdTopTextBox";
            thresholdTopTextBox.Size = new Size(100, 23);
            thresholdTopTextBox.TabIndex = 1;
            // 
            // thresholdBotTextBox
            // 
            thresholdBotTextBox.Location = new Point(102, 41);
            thresholdBotTextBox.Name = "thresholdBotTextBox";
            thresholdBotTextBox.Size = new Size(100, 23);
            thresholdBotTextBox.TabIndex = 3;
            // 
            // thresholdBotLabel
            // 
            thresholdBotLabel.AutoSize = true;
            thresholdBotLabel.Location = new Point(12, 44);
            thresholdBotLabel.Name = "thresholdBotLabel";
            thresholdBotLabel.Size = new Size(84, 15);
            thresholdBotLabel.TabIndex = 2;
            thresholdBotLabel.Text = "Threshold bot:";
            // 
            // setButton
            // 
            setButton.Location = new Point(208, 12);
            setButton.Name = "setButton";
            setButton.Size = new Size(75, 52);
            setButton.TabIndex = 4;
            setButton.Text = "Set";
            setButton.UseVisualStyleBackColor = true;
            setButton.Click += setButton_Click;
            // 
            // ThresholdConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(290, 71);
            Controls.Add(setButton);
            Controls.Add(thresholdBotTextBox);
            Controls.Add(thresholdBotLabel);
            Controls.Add(thresholdTopTextBox);
            Controls.Add(thresholdTopLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ThresholdConfigurationForm";
            Text = "Threshold configuration";
            Load += ThresholdConfigurationForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label thresholdTopLabel;
        private TextBox thresholdTopTextBox;
        private TextBox thresholdBotTextBox;
        private Label thresholdBotLabel;
        private Button setButton;
    }
}