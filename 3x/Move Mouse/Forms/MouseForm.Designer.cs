namespace Ellanet.Forms
{
    partial class MouseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MouseForm));
            this.actionButton = new System.Windows.Forms.Button();
            this.countdownProgressBar = new System.Windows.Forms.ProgressBar();
            this.behaviourTabPage = new System.Windows.Forms.TabPage();
            this.hotkeyComboBox = new System.Windows.Forms.ComboBox();
            this.hotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.disableOnBatteryCheckBox = new System.Windows.Forms.CheckBox();
            this.autoPauseCheckBox = new System.Windows.Forms.CheckBox();
            this.resumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.startOnLaunchCheckBox = new System.Windows.Forms.CheckBox();
            this.minimiseToSystemTrayCheckBox = new System.Windows.Forms.CheckBox();
            this.launchAtLogonCheckBox = new System.Windows.Forms.CheckBox();
            this.minimiseOnStartCheckBox = new System.Windows.Forms.CheckBox();
            this.minimiseOnPauseCheckBox = new System.Windows.Forms.CheckBox();
            this.resumeCheckBox = new System.Windows.Forms.CheckBox();
            this.actionsTabPage = new System.Windows.Forms.TabPage();
            this.delayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.moveMouseCheckBox = new System.Windows.Forms.CheckBox();
            this.clickMouseCheckBox = new System.Windows.Forms.CheckBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.stealthCheckBox = new System.Windows.Forms.CheckBox();
            this.xNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.yNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.traceButton = new System.Windows.Forms.Button();
            this.appActivateCheckBox = new System.Windows.Forms.CheckBox();
            this.processComboBox = new System.Windows.Forms.ComboBox();
            this.keystrokeCheckBox = new System.Windows.Forms.CheckBox();
            this.keystrokeComboBox = new System.Windows.Forms.ComboBox();
            this.staticPositionCheckBox = new System.Windows.Forms.CheckBox();
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.behaviourTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resumeNumericUpDown)).BeginInit();
            this.actionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).BeginInit();
            this.optionsTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionButton
            // 
            this.actionButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.actionButton.Location = new System.Drawing.Point(212, 357);
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(75, 23);
            this.actionButton.TabIndex = 1;
            this.actionButton.Text = "launch";
            this.actionButton.UseVisualStyleBackColor = true;
            // 
            // countdownProgressBar
            // 
            this.countdownProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.countdownProgressBar.Location = new System.Drawing.Point(12, 397);
            this.countdownProgressBar.Name = "countdownProgressBar";
            this.countdownProgressBar.Size = new System.Drawing.Size(475, 17);
            this.countdownProgressBar.TabIndex = 2;
            // 
            // behaviourTabPage
            // 
            this.behaviourTabPage.Controls.Add(this.hotkeyComboBox);
            this.behaviourTabPage.Controls.Add(this.hotkeyCheckBox);
            this.behaviourTabPage.Controls.Add(this.disableOnBatteryCheckBox);
            this.behaviourTabPage.Controls.Add(this.autoPauseCheckBox);
            this.behaviourTabPage.Controls.Add(this.resumeNumericUpDown);
            this.behaviourTabPage.Controls.Add(this.startOnLaunchCheckBox);
            this.behaviourTabPage.Controls.Add(this.minimiseToSystemTrayCheckBox);
            this.behaviourTabPage.Controls.Add(this.launchAtLogonCheckBox);
            this.behaviourTabPage.Controls.Add(this.minimiseOnStartCheckBox);
            this.behaviourTabPage.Controls.Add(this.minimiseOnPauseCheckBox);
            this.behaviourTabPage.Controls.Add(this.resumeCheckBox);
            this.behaviourTabPage.Location = new System.Drawing.Point(4, 25);
            this.behaviourTabPage.Name = "behaviourTabPage";
            this.behaviourTabPage.Size = new System.Drawing.Size(467, 300);
            this.behaviourTabPage.TabIndex = 3;
            this.behaviourTabPage.Text = "Behaviour";
            this.behaviourTabPage.UseVisualStyleBackColor = true;
            // 
            // hotkeyComboBox
            // 
            this.hotkeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hotkeyComboBox.Enabled = false;
            this.hotkeyComboBox.FormattingEnabled = true;
            this.hotkeyComboBox.ItemHeight = 16;
            this.hotkeyComboBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.hotkeyComboBox.Location = new System.Drawing.Point(290, 108);
            this.hotkeyComboBox.Name = "hotkeyComboBox";
            this.hotkeyComboBox.Size = new System.Drawing.Size(48, 24);
            this.hotkeyComboBox.Sorted = true;
            this.hotkeyComboBox.TabIndex = 17;
            // 
            // hotkeyCheckBox
            // 
            this.hotkeyCheckBox.AutoSize = true;
            this.hotkeyCheckBox.Location = new System.Drawing.Point(20, 110);
            this.hotkeyCheckBox.Name = "hotkeyCheckBox";
            this.hotkeyCheckBox.Size = new System.Drawing.Size(273, 20);
            this.hotkeyCheckBox.TabIndex = 16;
            this.hotkeyCheckBox.Text = "Enable start/pause hotkeys      CTRL+ALT+";
            this.hotkeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // disableOnBatteryCheckBox
            // 
            this.disableOnBatteryCheckBox.AutoSize = true;
            this.disableOnBatteryCheckBox.Location = new System.Drawing.Point(20, 80);
            this.disableOnBatteryCheckBox.Name = "disableOnBatteryCheckBox";
            this.disableOnBatteryCheckBox.Size = new System.Drawing.Size(212, 20);
            this.disableOnBatteryCheckBox.TabIndex = 3;
            this.disableOnBatteryCheckBox.Text = "Disable when running on battery";
            this.disableOnBatteryCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoPauseCheckBox
            // 
            this.autoPauseCheckBox.AutoSize = true;
            this.autoPauseCheckBox.Location = new System.Drawing.Point(20, 20);
            this.autoPauseCheckBox.Name = "autoPauseCheckBox";
            this.autoPauseCheckBox.Size = new System.Drawing.Size(224, 20);
            this.autoPauseCheckBox.TabIndex = 0;
            this.autoPauseCheckBox.Text = "Pause when mouse pointer moved";
            this.autoPauseCheckBox.UseVisualStyleBackColor = true;
            // 
            // resumeNumericUpDown
            // 
            this.resumeNumericUpDown.Enabled = false;
            this.resumeNumericUpDown.Location = new System.Drawing.Point(199, 48);
            this.resumeNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.resumeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.resumeNumericUpDown.Name = "resumeNumericUpDown";
            this.resumeNumericUpDown.Size = new System.Drawing.Size(53, 23);
            this.resumeNumericUpDown.TabIndex = 2;
            this.resumeNumericUpDown.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // startOnLaunchCheckBox
            // 
            this.startOnLaunchCheckBox.AutoSize = true;
            this.startOnLaunchCheckBox.Location = new System.Drawing.Point(20, 140);
            this.startOnLaunchCheckBox.Name = "startOnLaunchCheckBox";
            this.startOnLaunchCheckBox.Size = new System.Drawing.Size(267, 20);
            this.startOnLaunchCheckBox.TabIndex = 4;
            this.startOnLaunchCheckBox.Text = "Automatically start Move Mouse on launch";
            this.startOnLaunchCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimiseToSystemTrayCheckBox
            // 
            this.minimiseToSystemTrayCheckBox.AutoSize = true;
            this.minimiseToSystemTrayCheckBox.Location = new System.Drawing.Point(20, 260);
            this.minimiseToSystemTrayCheckBox.Name = "minimiseToSystemTrayCheckBox";
            this.minimiseToSystemTrayCheckBox.Size = new System.Drawing.Size(168, 20);
            this.minimiseToSystemTrayCheckBox.TabIndex = 8;
            this.minimiseToSystemTrayCheckBox.Text = "Minimise to System Tray";
            this.minimiseToSystemTrayCheckBox.UseVisualStyleBackColor = true;
            // 
            // launchAtLogonCheckBox
            // 
            this.launchAtLogonCheckBox.AutoSize = true;
            this.launchAtLogonCheckBox.Location = new System.Drawing.Point(20, 170);
            this.launchAtLogonCheckBox.Name = "launchAtLogonCheckBox";
            this.launchAtLogonCheckBox.Size = new System.Drawing.Size(325, 20);
            this.launchAtLogonCheckBox.TabIndex = 5;
            this.launchAtLogonCheckBox.Text = "Automatically launch Move Mouse at Windows logon";
            this.launchAtLogonCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimiseOnStartCheckBox
            // 
            this.minimiseOnStartCheckBox.AutoSize = true;
            this.minimiseOnStartCheckBox.Location = new System.Drawing.Point(20, 230);
            this.minimiseOnStartCheckBox.Name = "minimiseOnStartCheckBox";
            this.minimiseOnStartCheckBox.Size = new System.Drawing.Size(125, 20);
            this.minimiseOnStartCheckBox.TabIndex = 7;
            this.minimiseOnStartCheckBox.Text = "Minimise on start";
            this.minimiseOnStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimiseOnPauseCheckBox
            // 
            this.minimiseOnPauseCheckBox.AutoSize = true;
            this.minimiseOnPauseCheckBox.Location = new System.Drawing.Point(20, 200);
            this.minimiseOnPauseCheckBox.Name = "minimiseOnPauseCheckBox";
            this.minimiseOnPauseCheckBox.Size = new System.Drawing.Size(133, 20);
            this.minimiseOnPauseCheckBox.TabIndex = 6;
            this.minimiseOnPauseCheckBox.Text = "Minimise on pause";
            this.minimiseOnPauseCheckBox.UseVisualStyleBackColor = true;
            // 
            // resumeCheckBox
            // 
            this.resumeCheckBox.AutoSize = true;
            this.resumeCheckBox.Location = new System.Drawing.Point(20, 50);
            this.resumeCheckBox.Name = "resumeCheckBox";
            this.resumeCheckBox.Size = new System.Drawing.Size(359, 20);
            this.resumeCheckBox.TabIndex = 1;
            this.resumeCheckBox.Text = "Automatically resume after                seconds of inactivity";
            this.resumeCheckBox.UseVisualStyleBackColor = true;
            // 
            // actionsTabPage
            // 
            this.actionsTabPage.Controls.Add(this.delayNumericUpDown);
            this.actionsTabPage.Controls.Add(this.label1);
            this.actionsTabPage.Controls.Add(this.moveMouseCheckBox);
            this.actionsTabPage.Controls.Add(this.clickMouseCheckBox);
            this.actionsTabPage.Controls.Add(this.refreshButton);
            this.actionsTabPage.Controls.Add(this.stealthCheckBox);
            this.actionsTabPage.Controls.Add(this.xNumericUpDown);
            this.actionsTabPage.Controls.Add(this.yNumericUpDown);
            this.actionsTabPage.Controls.Add(this.traceButton);
            this.actionsTabPage.Controls.Add(this.appActivateCheckBox);
            this.actionsTabPage.Controls.Add(this.processComboBox);
            this.actionsTabPage.Controls.Add(this.keystrokeCheckBox);
            this.actionsTabPage.Controls.Add(this.keystrokeComboBox);
            this.actionsTabPage.Controls.Add(this.staticPositionCheckBox);
            this.actionsTabPage.Location = new System.Drawing.Point(4, 25);
            this.actionsTabPage.Name = "actionsTabPage";
            this.actionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.actionsTabPage.Size = new System.Drawing.Size(467, 300);
            this.actionsTabPage.TabIndex = 0;
            this.actionsTabPage.Text = "main setting";
            this.actionsTabPage.UseVisualStyleBackColor = true;
            // 
            // delayNumericUpDown
            // 
            this.delayNumericUpDown.Location = new System.Drawing.Point(20, 18);
            this.delayNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.delayNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.delayNumericUpDown.Name = "delayNumericUpDown";
            this.delayNumericUpDown.Size = new System.Drawing.Size(53, 23);
            this.delayNumericUpDown.TabIndex = 0;
            this.delayNumericUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "time interval(s)";
            // 
            // moveMouseCheckBox
            // 
            this.moveMouseCheckBox.AutoSize = true;
            this.moveMouseCheckBox.Checked = true;
            this.moveMouseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moveMouseCheckBox.Location = new System.Drawing.Point(20, 50);
            this.moveMouseCheckBox.Name = "moveMouseCheckBox";
            this.moveMouseCheckBox.Size = new System.Drawing.Size(147, 20);
            this.moveMouseCheckBox.TabIndex = 1;
            this.moveMouseCheckBox.Text = "is move the mouse cursor";
            this.moveMouseCheckBox.UseVisualStyleBackColor = true;
            // 
            // clickMouseCheckBox
            // 
            this.clickMouseCheckBox.AutoSize = true;
            this.clickMouseCheckBox.Location = new System.Drawing.Point(20, 140);
            this.clickMouseCheckBox.Name = "clickMouseCheckBox";
            this.clickMouseCheckBox.Size = new System.Drawing.Size(157, 20);
            this.clickMouseCheckBox.TabIndex = 7;
            this.clickMouseCheckBox.Text = "Click left mouse button";
            this.clickMouseCheckBox.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Enabled = false;
            this.refreshButton.Location = new System.Drawing.Point(375, 198);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 24);
            this.refreshButton.TabIndex = 12;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            // 
            // stealthCheckBox
            // 
            this.stealthCheckBox.AutoSize = true;
            this.stealthCheckBox.Location = new System.Drawing.Point(20, 80);
            this.stealthCheckBox.Name = "stealthCheckBox";
            this.stealthCheckBox.Size = new System.Drawing.Size(282, 20);
            this.stealthCheckBox.TabIndex = 2;
            this.stealthCheckBox.Text = "Stealth mode (pointer movement not visible)";
            this.stealthCheckBox.UseVisualStyleBackColor = true;
            // 
            // xNumericUpDown
            // 
            this.xNumericUpDown.Enabled = false;
            this.xNumericUpDown.Location = new System.Drawing.Point(228, 108);
            this.xNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.xNumericUpDown.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.xNumericUpDown.Name = "xNumericUpDown";
            this.xNumericUpDown.Size = new System.Drawing.Size(53, 23);
            this.xNumericUpDown.TabIndex = 4;
            // 
            // yNumericUpDown
            // 
            this.yNumericUpDown.Enabled = false;
            this.yNumericUpDown.Location = new System.Drawing.Point(307, 108);
            this.yNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.yNumericUpDown.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.yNumericUpDown.Name = "yNumericUpDown";
            this.yNumericUpDown.Size = new System.Drawing.Size(53, 23);
            this.yNumericUpDown.TabIndex = 5;
            // 
            // traceButton
            // 
            this.traceButton.Enabled = false;
            this.traceButton.Location = new System.Drawing.Point(375, 107);
            this.traceButton.Name = "traceButton";
            this.traceButton.Size = new System.Drawing.Size(75, 24);
            this.traceButton.TabIndex = 6;
            this.traceButton.Text = "Track";
            this.traceButton.UseVisualStyleBackColor = true;
            // 
            // appActivateCheckBox
            // 
            this.appActivateCheckBox.AutoSize = true;
            this.appActivateCheckBox.Location = new System.Drawing.Point(20, 200);
            this.appActivateCheckBox.Name = "appActivateCheckBox";
            this.appActivateCheckBox.Size = new System.Drawing.Size(137, 20);
            this.appActivateCheckBox.TabIndex = 10;
            this.appActivateCheckBox.Text = "Activate application";
            this.appActivateCheckBox.UseVisualStyleBackColor = true;
            // 
            // processComboBox
            // 
            this.processComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processComboBox.Enabled = false;
            this.processComboBox.FormattingEnabled = true;
            this.processComboBox.Location = new System.Drawing.Point(161, 198);
            this.processComboBox.Name = "processComboBox";
            this.processComboBox.Size = new System.Drawing.Size(199, 24);
            this.processComboBox.Sorted = true;
            this.processComboBox.TabIndex = 11;
            // 
            // keystrokeCheckBox
            // 
            this.keystrokeCheckBox.AutoSize = true;
            this.keystrokeCheckBox.Location = new System.Drawing.Point(20, 170);
            this.keystrokeCheckBox.Name = "keystrokeCheckBox";
            this.keystrokeCheckBox.Size = new System.Drawing.Size(114, 20);
            this.keystrokeCheckBox.TabIndex = 8;
            this.keystrokeCheckBox.Text = "Send keystroke";
            this.keystrokeCheckBox.UseVisualStyleBackColor = true;
            // 
            // keystrokeComboBox
            // 
            this.keystrokeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keystrokeComboBox.Enabled = false;
            this.keystrokeComboBox.FormattingEnabled = true;
            this.keystrokeComboBox.ItemHeight = 16;
            this.keystrokeComboBox.Items.AddRange(new object[] {
            "{BACKSPACE}",
            "{BREAK}",
            "{CAPSLOCK}",
            "{DELETE}",
            "{DOWN}",
            "{END}",
            "{ENTER}",
            "{ESC}",
            "{HELP}",
            "{HOME}",
            "{INSERT}",
            "{LEFT}",
            "{NUMLOCK}",
            "{PGDN}",
            "{PGUP}",
            "{PRTSC}",
            "{RIGHT}",
            "{SCROLLLOCK}",
            "{TAB}",
            "{UP}",
            "{F1}",
            "{F2}",
            "{F3}",
            "{F4}",
            "{F5}",
            "{F6}",
            "{F7}",
            "{F8}",
            "{F9}",
            "{F10}",
            "{F11}",
            "{F12}",
            "{F13}",
            "{F14}",
            "{F15}",
            "{F16}",
            "{ADD}",
            "{SUBTRACT}",
            "{MULTIPLY}",
            "{DIVIDE}"});
            this.keystrokeComboBox.Location = new System.Drawing.Point(161, 168);
            this.keystrokeComboBox.Name = "keystrokeComboBox";
            this.keystrokeComboBox.Size = new System.Drawing.Size(199, 24);
            this.keystrokeComboBox.TabIndex = 15;
            // 
            // staticPositionCheckBox
            // 
            this.staticPositionCheckBox.AutoSize = true;
            this.staticPositionCheckBox.Location = new System.Drawing.Point(20, 110);
            this.staticPositionCheckBox.Name = "staticPositionCheckBox";
            this.staticPositionCheckBox.Size = new System.Drawing.Size(289, 20);
            this.staticPositionCheckBox.TabIndex = 3;
            this.staticPositionCheckBox.Text = "Static mouse pointer position   x                  y";
            this.staticPositionCheckBox.UseVisualStyleBackColor = true;
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsTabControl.Controls.Add(this.actionsTabPage);
            this.optionsTabControl.Controls.Add(this.behaviourTabPage);
            this.optionsTabControl.Location = new System.Drawing.Point(12, 12);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(475, 329);
            this.optionsTabControl.TabIndex = 0;
            // 
            // MouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 426);
            this.Controls.Add(this.optionsTabControl);
            this.Controls.Add(this.countdownProgressBar);
            this.Controls.Add(this.actionButton);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MouseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "琴";
            this.behaviourTabPage.ResumeLayout(false);
            this.behaviourTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resumeNumericUpDown)).EndInit();
            this.actionsTabPage.ResumeLayout(false);
            this.actionsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).EndInit();
            this.optionsTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button actionButton;
        private System.Windows.Forms.ProgressBar countdownProgressBar;
        private System.Windows.Forms.TabPage behaviourTabPage;
        private System.Windows.Forms.ComboBox hotkeyComboBox;
        private System.Windows.Forms.CheckBox hotkeyCheckBox;
        private System.Windows.Forms.CheckBox disableOnBatteryCheckBox;
        private System.Windows.Forms.CheckBox autoPauseCheckBox;
        private System.Windows.Forms.NumericUpDown resumeNumericUpDown;
        private System.Windows.Forms.CheckBox startOnLaunchCheckBox;
        private System.Windows.Forms.CheckBox minimiseToSystemTrayCheckBox;
        private System.Windows.Forms.CheckBox launchAtLogonCheckBox;
        private System.Windows.Forms.CheckBox minimiseOnStartCheckBox;
        private System.Windows.Forms.CheckBox minimiseOnPauseCheckBox;
        private System.Windows.Forms.CheckBox resumeCheckBox;
        private System.Windows.Forms.TabPage actionsTabPage;
        private System.Windows.Forms.NumericUpDown delayNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox moveMouseCheckBox;
        private System.Windows.Forms.CheckBox clickMouseCheckBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.CheckBox stealthCheckBox;
        private System.Windows.Forms.NumericUpDown xNumericUpDown;
        private System.Windows.Forms.NumericUpDown yNumericUpDown;
        private System.Windows.Forms.Button traceButton;
        private System.Windows.Forms.CheckBox appActivateCheckBox;
        private System.Windows.Forms.ComboBox processComboBox;
        private System.Windows.Forms.CheckBox keystrokeCheckBox;
        private System.Windows.Forms.ComboBox keystrokeComboBox;
        private System.Windows.Forms.CheckBox staticPositionCheckBox;
        private System.Windows.Forms.TabControl optionsTabControl;
    }
}