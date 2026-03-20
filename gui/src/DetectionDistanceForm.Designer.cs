namespace RadarSensorGesture
{
    partial class DetectionDistanceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetectionDistanceForm));
            maximumDistanceLabel = new Label();
            maxDistanceTextBox = new TextBox();
            setButton = new Button();
            SuspendLayout();
            // 
            // maximumDistanceLabel
            // 
            maximumDistanceLabel.AutoSize = true;
            maximumDistanceLabel.Location = new Point(12, 15);
            maximumDistanceLabel.Name = "maximumDistanceLabel";
            maximumDistanceLabel.Size = new Size(112, 15);
            maximumDistanceLabel.TabIndex = 0;
            maximumDistanceLabel.Text = "Maximum Distance:";
            // 
            // maxDistanceTextBox
            // 
            maxDistanceTextBox.Location = new Point(130, 12);
            maxDistanceTextBox.Name = "maxDistanceTextBox";
            maxDistanceTextBox.Size = new Size(100, 23);
            maxDistanceTextBox.TabIndex = 1;
            // 
            // setButton
            // 
            setButton.Location = new Point(236, 11);
            setButton.Name = "setButton";
            setButton.Size = new Size(75, 23);
            setButton.TabIndex = 2;
            setButton.Text = "Set";
            setButton.UseVisualStyleBackColor = true;
            setButton.Click += setButton_Click;
            // 
            // DetectionDistanceForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(319, 42);
            Controls.Add(setButton);
            Controls.Add(maxDistanceTextBox);
            Controls.Add(maximumDistanceLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DetectionDistanceForm";
            Text = "Detection Distance Configuration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label maximumDistanceLabel;
        private TextBox maxDistanceTextBox;
        private Button setButton;
    }
}