namespace RadarSensorGesture.Views
{
    partial class DetectedActionView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            clickTableLayoutPanel = new TableLayoutPanel();
            clickTopButton = new Button();
            clickCenterButton = new Button();
            clickRightButton = new Button();
            clickLeftButton = new Button();
            clickDownButton = new Button();
            clickTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // clickTableLayoutPanel
            // 
            clickTableLayoutPanel.ColumnCount = 3;
            clickTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            clickTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            clickTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            clickTableLayoutPanel.Controls.Add(clickTopButton, 1, 0);
            clickTableLayoutPanel.Controls.Add(clickCenterButton, 1, 1);
            clickTableLayoutPanel.Controls.Add(clickRightButton, 2, 1);
            clickTableLayoutPanel.Controls.Add(clickLeftButton, 0, 1);
            clickTableLayoutPanel.Controls.Add(clickDownButton, 1, 2);
            clickTableLayoutPanel.Dock = DockStyle.Fill;
            clickTableLayoutPanel.Location = new Point(0, 0);
            clickTableLayoutPanel.Name = "clickTableLayoutPanel";
            clickTableLayoutPanel.RowCount = 3;
            clickTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            clickTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            clickTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            clickTableLayoutPanel.Size = new Size(450, 283);
            clickTableLayoutPanel.TabIndex = 0;
            // 
            // clickTopButton
            // 
            clickTopButton.Dock = DockStyle.Fill;
            clickTopButton.Location = new Point(151, 3);
            clickTopButton.Name = "clickTopButton";
            clickTopButton.Size = new Size(147, 87);
            clickTopButton.TabIndex = 1;
            clickTopButton.Text = "⬆";
            clickTopButton.UseVisualStyleBackColor = true;
            // 
            // clickCenterButton
            // 
            clickCenterButton.Dock = DockStyle.Fill;
            clickCenterButton.Location = new Point(151, 96);
            clickCenterButton.Name = "clickCenterButton";
            clickCenterButton.Size = new Size(147, 90);
            clickCenterButton.TabIndex = 1;
            clickCenterButton.Text = ".";
            clickCenterButton.UseVisualStyleBackColor = true;
            // 
            // clickRightButton
            // 
            clickRightButton.Dock = DockStyle.Fill;
            clickRightButton.Location = new Point(304, 96);
            clickRightButton.Name = "clickRightButton";
            clickRightButton.Size = new Size(143, 90);
            clickRightButton.TabIndex = 1;
            clickRightButton.Text = "➞";
            clickRightButton.UseVisualStyleBackColor = true;
            // 
            // clickLeftButton
            // 
            clickLeftButton.Dock = DockStyle.Fill;
            clickLeftButton.Location = new Point(3, 96);
            clickLeftButton.Name = "clickLeftButton";
            clickLeftButton.Size = new Size(142, 90);
            clickLeftButton.TabIndex = 1;
            clickLeftButton.Text = "⬅";
            clickLeftButton.UseVisualStyleBackColor = true;
            // 
            // clickDownButton
            // 
            clickDownButton.Dock = DockStyle.Fill;
            clickDownButton.Location = new Point(151, 192);
            clickDownButton.Name = "clickDownButton";
            clickDownButton.Size = new Size(147, 88);
            clickDownButton.TabIndex = 1;
            clickDownButton.Text = "⬇";
            clickDownButton.UseVisualStyleBackColor = true;
            // 
            // DetectedActionView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(clickTableLayoutPanel);
            Name = "DetectedActionView";
            Size = new Size(450, 283);
            clickTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel clickTableLayoutPanel;
        private Button clickTopButton;
        private Button clickDownButton;
        private Button clickRightButton;
        private Button clickLeftButton;
        private Button clickCenterButton;
    }
}
