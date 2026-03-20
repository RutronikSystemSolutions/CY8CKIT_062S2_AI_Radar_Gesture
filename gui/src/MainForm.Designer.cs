namespace RadarSensorGesture
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            comPortLabel = new Label();
            comPortComboBox = new ComboBox();
            comPortStatusTextBox = new TextBox();
            connectDisconnectButton = new Button();
            rawSignalView = new RadarSensorGesture.Views.RawSignalView();
            magnitudeOverTime = new RadarSensorGesture.Views.MagnitudeOverTime();
            gestureView = new RadarSensorGesture.Views.GestureView();
            phaseDifferenceView = new RadarSensorGesture.Views.PhaseDifferenceView();
            mainSplitContainer = new SplitContainer();
            tableLayoutPanel = new TableLayoutPanel();
            distanceOverTimeView = new RadarSensorGesture.Views.DistanceOverTimeView();
            processedSplitContainer = new SplitContainer();
            startRecordingButton = new Button();
            recordCountTextBox = new TextBox();
            stopRecordingButton = new Button();
            storeToFileButton = new Button();
            menuStrip = new MenuStrip();
            viewToolStripMenuItem = new ToolStripMenuItem();
            rawValuesToolStripMenuItem = new ToolStripMenuItem();
            phaseModeToolStripMenuItem = new ToolStripMenuItem();
            azimuthToolStripMenuItem = new ToolStripMenuItem();
            elevationToolStripMenuItem = new ToolStripMenuItem();
            holdonLongToolStripMenuItem = new ToolStripMenuItem();
            configurationToolStripMenuItem = new ToolStripMenuItem();
            thresholdToolStripMenuItem = new ToolStripMenuItem();
            distanceToolStripMenuItem = new ToolStripMenuItem();
            detectedActionView = new RadarSensorGesture.Views.DetectedActionView();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)processedSplitContainer).BeginInit();
            processedSplitContainer.Panel1.SuspendLayout();
            processedSplitContainer.Panel2.SuspendLayout();
            processedSplitContainer.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // comPortLabel
            // 
            comPortLabel.AutoSize = true;
            comPortLabel.Location = new Point(12, 30);
            comPortLabel.Name = "comPortLabel";
            comPortLabel.Size = new Size(63, 15);
            comPortLabel.TabIndex = 0;
            comPortLabel.Text = "COM port:";
            // 
            // comPortComboBox
            // 
            comPortComboBox.FormattingEnabled = true;
            comPortComboBox.Location = new Point(81, 27);
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(121, 23);
            comPortComboBox.TabIndex = 1;
            // 
            // comPortStatusTextBox
            // 
            comPortStatusTextBox.Location = new Point(208, 27);
            comPortStatusTextBox.Name = "comPortStatusTextBox";
            comPortStatusTextBox.ReadOnly = true;
            comPortStatusTextBox.Size = new Size(100, 23);
            comPortStatusTextBox.TabIndex = 2;
            // 
            // connectDisconnectButton
            // 
            connectDisconnectButton.Location = new Point(314, 26);
            connectDisconnectButton.Name = "connectDisconnectButton";
            connectDisconnectButton.Size = new Size(95, 23);
            connectDisconnectButton.TabIndex = 3;
            connectDisconnectButton.Text = "Connect";
            connectDisconnectButton.UseVisualStyleBackColor = true;
            connectDisconnectButton.Click += connectDisconnectButton_Click;
            // 
            // rawSignalView
            // 
            rawSignalView.BorderStyle = BorderStyle.FixedSingle;
            rawSignalView.Dock = DockStyle.Fill;
            rawSignalView.Location = new Point(3, 4);
            rawSignalView.Margin = new Padding(3, 4, 3, 4);
            rawSignalView.Name = "rawSignalView";
            rawSignalView.Size = new Size(292, 138);
            rawSignalView.TabIndex = 4;
            // 
            // magnitudeOverTime
            // 
            magnitudeOverTime.BorderStyle = BorderStyle.FixedSingle;
            magnitudeOverTime.Dock = DockStyle.Fill;
            magnitudeOverTime.Location = new Point(3, 150);
            magnitudeOverTime.Margin = new Padding(3, 4, 3, 4);
            magnitudeOverTime.Name = "magnitudeOverTime";
            magnitudeOverTime.Size = new Size(292, 138);
            magnitudeOverTime.TabIndex = 5;
            // 
            // gestureView
            // 
            gestureView.BorderStyle = BorderStyle.FixedSingle;
            gestureView.Dock = DockStyle.Fill;
            gestureView.Location = new Point(0, 0);
            gestureView.Margin = new Padding(3, 4, 3, 4);
            gestureView.Name = "gestureView";
            gestureView.Size = new Size(595, 288);
            gestureView.TabIndex = 6;
            // 
            // phaseDifferenceView
            // 
            phaseDifferenceView.BorderStyle = BorderStyle.FixedSingle;
            phaseDifferenceView.Dock = DockStyle.Fill;
            phaseDifferenceView.Location = new Point(3, 296);
            phaseDifferenceView.Margin = new Padding(3, 4, 3, 4);
            phaseDifferenceView.Name = "phaseDifferenceView";
            phaseDifferenceView.Size = new Size(292, 138);
            phaseDifferenceView.TabIndex = 7;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainSplitContainer.Location = new Point(12, 56);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(tableLayoutPanel);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(processedSplitContainer);
            mainSplitContainer.Size = new Size(897, 586);
            mainSplitContainer.SplitterDistance = 298;
            mainSplitContainer.TabIndex = 8;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(rawSignalView, 0, 0);
            tableLayoutPanel.Controls.Add(phaseDifferenceView, 0, 2);
            tableLayoutPanel.Controls.Add(magnitudeOverTime, 0, 1);
            tableLayoutPanel.Controls.Add(distanceOverTimeView, 0, 3);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel.Size = new Size(298, 586);
            tableLayoutPanel.TabIndex = 0;
            // 
            // distanceOverTimeView
            // 
            distanceOverTimeView.BorderStyle = BorderStyle.FixedSingle;
            distanceOverTimeView.Dock = DockStyle.Fill;
            distanceOverTimeView.Location = new Point(3, 441);
            distanceOverTimeView.Name = "distanceOverTimeView";
            distanceOverTimeView.Size = new Size(292, 142);
            distanceOverTimeView.TabIndex = 8;
            // 
            // processedSplitContainer
            // 
            processedSplitContainer.Dock = DockStyle.Fill;
            processedSplitContainer.Location = new Point(0, 0);
            processedSplitContainer.Name = "processedSplitContainer";
            processedSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // processedSplitContainer.Panel1
            // 
            processedSplitContainer.Panel1.Controls.Add(gestureView);
            // 
            // processedSplitContainer.Panel2
            // 
            processedSplitContainer.Panel2.Controls.Add(detectedActionView);
            processedSplitContainer.Size = new Size(595, 586);
            processedSplitContainer.SplitterDistance = 288;
            processedSplitContainer.TabIndex = 7;
            // 
            // startRecordingButton
            // 
            startRecordingButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            startRecordingButton.Location = new Point(537, 27);
            startRecordingButton.Margin = new Padding(3, 2, 3, 2);
            startRecordingButton.Name = "startRecordingButton";
            startRecordingButton.Size = new Size(82, 22);
            startRecordingButton.TabIndex = 9;
            startRecordingButton.Text = "Start";
            startRecordingButton.UseVisualStyleBackColor = true;
            startRecordingButton.Click += startRecordingButton_Click;
            // 
            // recordCountTextBox
            // 
            recordCountTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            recordCountTextBox.Location = new Point(711, 27);
            recordCountTextBox.Margin = new Padding(3, 2, 3, 2);
            recordCountTextBox.Name = "recordCountTextBox";
            recordCountTextBox.ReadOnly = true;
            recordCountTextBox.Size = new Size(110, 23);
            recordCountTextBox.TabIndex = 10;
            // 
            // stopRecordingButton
            // 
            stopRecordingButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            stopRecordingButton.Enabled = false;
            stopRecordingButton.Location = new Point(625, 26);
            stopRecordingButton.Margin = new Padding(3, 2, 3, 2);
            stopRecordingButton.Name = "stopRecordingButton";
            stopRecordingButton.Size = new Size(82, 22);
            stopRecordingButton.TabIndex = 11;
            stopRecordingButton.Text = "Stop";
            stopRecordingButton.UseVisualStyleBackColor = true;
            stopRecordingButton.Click += stopRecordingButton_Click;
            // 
            // storeToFileButton
            // 
            storeToFileButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            storeToFileButton.Enabled = false;
            storeToFileButton.Location = new Point(827, 26);
            storeToFileButton.Margin = new Padding(3, 2, 3, 2);
            storeToFileButton.Name = "storeToFileButton";
            storeToFileButton.Size = new Size(82, 22);
            storeToFileButton.TabIndex = 12;
            storeToFileButton.Text = "Store";
            storeToFileButton.UseVisualStyleBackColor = true;
            storeToFileButton.Click += storeToFileButton_Click;
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { viewToolStripMenuItem, configurationToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(921, 24);
            menuStrip.TabIndex = 13;
            menuStrip.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { rawValuesToolStripMenuItem, phaseModeToolStripMenuItem, azimuthToolStripMenuItem, elevationToolStripMenuItem, holdonLongToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // rawValuesToolStripMenuItem
            // 
            rawValuesToolStripMenuItem.Checked = true;
            rawValuesToolStripMenuItem.CheckState = CheckState.Checked;
            rawValuesToolStripMenuItem.Name = "rawValuesToolStripMenuItem";
            rawValuesToolStripMenuItem.Size = new Size(146, 22);
            rawValuesToolStripMenuItem.Text = "Raw values";
            rawValuesToolStripMenuItem.Click += rawValuesToolStripMenuItem_Click;
            // 
            // phaseModeToolStripMenuItem
            // 
            phaseModeToolStripMenuItem.Name = "phaseModeToolStripMenuItem";
            phaseModeToolStripMenuItem.Size = new Size(146, 22);
            phaseModeToolStripMenuItem.Text = "Phase mode";
            phaseModeToolStripMenuItem.Click += phaseModeToolStripMenuItem_Click;
            // 
            // azimuthToolStripMenuItem
            // 
            azimuthToolStripMenuItem.Checked = true;
            azimuthToolStripMenuItem.CheckState = CheckState.Checked;
            azimuthToolStripMenuItem.Name = "azimuthToolStripMenuItem";
            azimuthToolStripMenuItem.Size = new Size(146, 22);
            azimuthToolStripMenuItem.Text = "Azimuth";
            azimuthToolStripMenuItem.Click += azimuthToolStripMenuItem_Click;
            // 
            // elevationToolStripMenuItem
            // 
            elevationToolStripMenuItem.Checked = true;
            elevationToolStripMenuItem.CheckState = CheckState.Checked;
            elevationToolStripMenuItem.Name = "elevationToolStripMenuItem";
            elevationToolStripMenuItem.Size = new Size(146, 22);
            elevationToolStripMenuItem.Text = "Elevation";
            elevationToolStripMenuItem.Click += elevationToolStripMenuItem_Click;
            // 
            // holdonLongToolStripMenuItem
            // 
            holdonLongToolStripMenuItem.Checked = true;
            holdonLongToolStripMenuItem.CheckState = CheckState.Checked;
            holdonLongToolStripMenuItem.Name = "holdonLongToolStripMenuItem";
            holdonLongToolStripMenuItem.Size = new Size(146, 22);
            holdonLongToolStripMenuItem.Text = "Hold-on long";
            holdonLongToolStripMenuItem.Click += holdonLongToolStripMenuItem_Click;
            // 
            // configurationToolStripMenuItem
            // 
            configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { thresholdToolStripMenuItem, distanceToolStripMenuItem });
            configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            configurationToolStripMenuItem.Size = new Size(93, 20);
            configurationToolStripMenuItem.Text = "Configuration";
            // 
            // thresholdToolStripMenuItem
            // 
            thresholdToolStripMenuItem.Name = "thresholdToolStripMenuItem";
            thresholdToolStripMenuItem.Size = new Size(127, 22);
            thresholdToolStripMenuItem.Text = "Threshold";
            thresholdToolStripMenuItem.Click += thresholdToolStripMenuItem_Click;
            // 
            // distanceToolStripMenuItem
            // 
            distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            distanceToolStripMenuItem.Size = new Size(127, 22);
            distanceToolStripMenuItem.Text = "Distance";
            distanceToolStripMenuItem.Click += distanceToolStripMenuItem_Click;
            // 
            // detectedActionView
            // 
            detectedActionView.BorderStyle = BorderStyle.FixedSingle;
            detectedActionView.Dock = DockStyle.Fill;
            detectedActionView.Location = new Point(0, 0);
            detectedActionView.Name = "detectedActionView";
            detectedActionView.Size = new Size(595, 294);
            detectedActionView.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(921, 654);
            Controls.Add(storeToFileButton);
            Controls.Add(stopRecordingButton);
            Controls.Add(recordCountTextBox);
            Controls.Add(startRecordingButton);
            Controls.Add(mainSplitContainer);
            Controls.Add(connectDisconnectButton);
            Controls.Add(comPortStatusTextBox);
            Controls.Add(comPortComboBox);
            Controls.Add(comPortLabel);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Radar Gesture Detection";
            Load += MainForm_Load;
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            processedSplitContainer.Panel1.ResumeLayout(false);
            processedSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)processedSplitContainer).EndInit();
            processedSplitContainer.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label comPortLabel;
        private ComboBox comPortComboBox;
        private TextBox comPortStatusTextBox;
        private Button connectDisconnectButton;
        private Views.RawSignalView rawSignalView;
        private Views.MagnitudeOverTime magnitudeOverTime;
        private Views.GestureView gestureView;
        private Views.PhaseDifferenceView phaseDifferenceView;
        private SplitContainer mainSplitContainer;
        private TableLayoutPanel tableLayoutPanel;
        private Button startRecordingButton;
        private TextBox recordCountTextBox;
        private Button stopRecordingButton;
        private Button storeToFileButton;
        private Views.DistanceOverTimeView distanceOverTimeView;
        private MenuStrip menuStrip;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem rawValuesToolStripMenuItem;
        private ToolStripMenuItem phaseModeToolStripMenuItem;
        private ToolStripMenuItem azimuthToolStripMenuItem;
        private ToolStripMenuItem elevationToolStripMenuItem;
        private ToolStripMenuItem holdonLongToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem thresholdToolStripMenuItem;
        private ToolStripMenuItem distanceToolStripMenuItem;
        private SplitContainer processedSplitContainer;
        private Views.DetectedActionView detectedActionView;
    }
}
