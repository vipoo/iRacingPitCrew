namespace iRacingPitCrew
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.iRacingConnectionStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.carListCombo = new System.Windows.Forms.ComboBox();
            this.raceDurationTextBox = new System.Windows.Forms.TextBox();
            this.raceDurationInMinutesButton = new System.Windows.Forms.RadioButton();
            this.raceDurationInLapsButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tankLimitTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cultureDropDown = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.activationFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pitCrewVoiceCommandsList = new System.Windows.Forms.ListView();
            this.voiceCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.voiceCommandDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label9 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "iRacing Pit Commands";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "iRacing PitCrew";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            this.notifyIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_Exit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // toolStripMenuItem_Open
            // 
            this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
            this.toolStripMenuItem_Open.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem_Open.Text = "Open";
            // 
            // toolStripMenuItem_Exit
            // 
            this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
            this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem_Exit.Text = "Exit";
            // 
            // iRacingConnectionStatus
            // 
            this.iRacingConnectionStatus.AutoSize = true;
            this.iRacingConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iRacingConnectionStatus.Location = new System.Drawing.Point(88, 64);
            this.iRacingConnectionStatus.Name = "iRacingConnectionStatus";
            this.iRacingConnectionStatus.Size = new System.Drawing.Size(107, 20);
            this.iRacingConnectionStatus.TabIndex = 1;
            this.iRacingConnectionStatus.Text = "Disconnected";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Status:";
            // 
            // carListCombo
            // 
            this.carListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carListCombo.FormattingEnabled = true;
            this.carListCombo.Location = new System.Drawing.Point(29, 125);
            this.carListCombo.Name = "carListCombo";
            this.carListCombo.Size = new System.Drawing.Size(193, 21);
            this.carListCombo.TabIndex = 2;
            this.carListCombo.SelectedIndexChanged += new System.EventHandler(this.carListCombo_SelectedIndexChanged);
            // 
            // raceDurationTextBox
            // 
            this.raceDurationTextBox.Location = new System.Drawing.Point(374, 125);
            this.raceDurationTextBox.Name = "raceDurationTextBox";
            this.raceDurationTextBox.Size = new System.Drawing.Size(76, 20);
            this.raceDurationTextBox.TabIndex = 7;
            this.raceDurationTextBox.TextChanged += new System.EventHandler(this.raceDurationTextBox_TextChanged);
            // 
            // raceDurationInMinutesButton
            // 
            this.raceDurationInMinutesButton.AutoSize = true;
            this.raceDurationInMinutesButton.Location = new System.Drawing.Point(456, 126);
            this.raceDurationInMinutesButton.Name = "raceDurationInMinutesButton";
            this.raceDurationInMinutesButton.Size = new System.Drawing.Size(62, 17);
            this.raceDurationInMinutesButton.TabIndex = 8;
            this.raceDurationInMinutesButton.TabStop = true;
            this.raceDurationInMinutesButton.Text = "Minutes";
            this.raceDurationInMinutesButton.UseVisualStyleBackColor = true;
            this.raceDurationInMinutesButton.CheckedChanged += new System.EventHandler(this.raceDurationTypeButton_CheckedChanged);
            // 
            // raceDurationInLapsButton
            // 
            this.raceDurationInLapsButton.AutoSize = true;
            this.raceDurationInLapsButton.Location = new System.Drawing.Point(524, 126);
            this.raceDurationInLapsButton.Name = "raceDurationInLapsButton";
            this.raceDurationInLapsButton.Size = new System.Drawing.Size(48, 17);
            this.raceDurationInLapsButton.TabIndex = 9;
            this.raceDurationInLapsButton.TabStop = true;
            this.raceDurationInLapsButton.Text = "Laps";
            this.raceDurationInLapsButton.UseVisualStyleBackColor = true;
            this.raceDurationInLapsButton.CheckedChanged += new System.EventHandler(this.raceDurationTypeButton_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Car:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(371, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Race Duration:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(239, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Tank Limit:";
            // 
            // tankLimitTextBox
            // 
            this.tankLimitTextBox.Location = new System.Drawing.Point(242, 126);
            this.tankLimitTextBox.Name = "tankLimitTextBox";
            this.tankLimitTextBox.Size = new System.Drawing.Size(76, 20);
            this.tankLimitTextBox.TabIndex = 4;
            this.tankLimitTextBox.TextChanged += new System.EventHandler(this.tankLimitTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(324, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "litres";
            // 
            // cultureDropDown
            // 
            this.cultureDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cultureDropDown.FormattingEnabled = true;
            this.cultureDropDown.Location = new System.Drawing.Point(456, 31);
            this.cultureDropDown.Name = "cultureDropDown";
            this.cultureDropDown.Size = new System.Drawing.Size(193, 21);
            this.cultureDropDown.TabIndex = 10;
            this.cultureDropDown.SelectedIndexChanged += new System.EventHandler(this.cultureDropDown_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(389, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Language";
            // 
            // activationFilterCheckBox
            // 
            this.activationFilterCheckBox.AutoSize = true;
            this.activationFilterCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.helpProvider.SetHelpString(this.activationFilterCheckBox, "If the activation phrase \"pit crew\" is not reconginise, try unchecking this");
            this.activationFilterCheckBox.Location = new System.Drawing.Point(374, 58);
            this.activationFilterCheckBox.Name = "activationFilterCheckBox";
            this.helpProvider.SetShowHelp(this.activationFilterCheckBox, true);
            this.activationFilterCheckBox.Size = new System.Drawing.Size(98, 17);
            this.activationFilterCheckBox.TabIndex = 12;
            this.activationFilterCheckBox.Text = "Activation Filter";
            this.activationFilterCheckBox.UseVisualStyleBackColor = true;
            this.activationFilterCheckBox.CheckedChanged += new System.EventHandler(this.activationFilterCheckBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Location = new System.Drawing.Point(22, 158);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 10);
            this.panel1.TabIndex = 13;
            // 
            // pitCrewVoiceCommandsList
            // 
            this.pitCrewVoiceCommandsList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.pitCrewVoiceCommandsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pitCrewVoiceCommandsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.voiceCommand,
            this.voiceCommandDescription});
            this.pitCrewVoiceCommandsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pitCrewVoiceCommandsList.FullRowSelect = true;
            this.pitCrewVoiceCommandsList.GridLines = true;
            this.pitCrewVoiceCommandsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.pitCrewVoiceCommandsList.Location = new System.Drawing.Point(26, 206);
            this.pitCrewVoiceCommandsList.MultiSelect = false;
            this.pitCrewVoiceCommandsList.Name = "pitCrewVoiceCommandsList";
            this.pitCrewVoiceCommandsList.Size = new System.Drawing.Size(623, 166);
            this.pitCrewVoiceCommandsList.TabIndex = 15;
            this.pitCrewVoiceCommandsList.UseCompatibleStateImageBehavior = false;
            this.pitCrewVoiceCommandsList.View = System.Windows.Forms.View.Details;
            // 
            // voiceCommand
            // 
            this.voiceCommand.Text = "Voice Command";
            this.voiceCommand.Width = 153;
            // 
            // voiceCommandDescription
            // 
            this.voiceCommandDescription.Text = "Description";
            this.voiceCommandDescription.Width = 442;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 180);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(299, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Say \'Pit Crew\', wait for \'Copy\', then say your Voice Commands:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 388);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pitCrewVoiceCommandsList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.activationFilterCheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cultureDropDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tankLimitTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.raceDurationInLapsButton);
            this.Controls.Add(this.raceDurationInMinutesButton);
            this.Controls.Add(this.raceDurationTextBox);
            this.Controls.Add(this.carListCombo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.iRacingConnectionStatus);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "iRacing Virtual Pit Crew";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
        private System.Windows.Forms.Label iRacingConnectionStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox carListCombo;
        private System.Windows.Forms.TextBox raceDurationTextBox;
        private System.Windows.Forms.RadioButton raceDurationInMinutesButton;
        private System.Windows.Forms.RadioButton raceDurationInLapsButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tankLimitTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cultureDropDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox activationFilterCheckBox;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView pitCrewVoiceCommandsList;
        private System.Windows.Forms.ColumnHeader voiceCommand;
        private System.Windows.Forms.ColumnHeader voiceCommandDescription;
        private System.Windows.Forms.Label label9;
    }
}

