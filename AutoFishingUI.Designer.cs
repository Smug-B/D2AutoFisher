using System.ComponentModel;

namespace D2AutoFisher
{
    public partial class AutoFishingUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer Components = null;

        public RichTextBox InfoTextbox;

        private Button RunButton;
        private Button CalibrateButton;
        private Button SaveSettingsButton;

        private Label SettingsLabel;
        private Label RPSLabel;
        private NumericUpDown RPSInput;
        private Label KeyLabel;
        private TextBox KeyInput;

        private LinkLabel GuideLink;
        private LinkLabel GitHubLink;
        private LinkLabel DiscordLink;

        public CheckBox PredictionEngineCheckbox;

        private ToolTip ToolTip;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AutoFishingUI));
            InfoTextbox = new RichTextBox();
            RunButton = new Button();
            SaveSettingsButton = new Button();
            CalibrateButton = new Button();
            SettingsLabel = new Label();
            RPSLabel = new Label();
            RPSInput = new NumericUpDown();
            KeyLabel = new Label();
            KeyInput = new TextBox();
            GuideLink = new LinkLabel();
            GitHubLink = new LinkLabel();
            PredictionEngineCheckbox = new CheckBox();
            ToolTip = new ToolTip(components);
            ((ISupportInitialize)RPSInput).BeginInit();
            SuspendLayout();
            // 
            // InfoTextbox
            // 
            InfoTextbox.BorderStyle = BorderStyle.FixedSingle;
            InfoTextbox.Location = new Point(12, 12);
            InfoTextbox.Name = "InfoTextbox";
            InfoTextbox.ReadOnly = true;
            InfoTextbox.Size = new Size(397, 201);
            InfoTextbox.TabIndex = 0;
            InfoTextbox.Text = resources.GetString("InfoTextbox.Text");
            // 
            // RunButton
            // 
            RunButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            RunButton.Location = new Point(12, 219);
            RunButton.Name = "RunButton";
            RunButton.Size = new Size(200, 100);
            RunButton.TabIndex = 0;
            RunButton.Text = "Run";
            ToolTip.SetToolTip(RunButton, resources.GetString("RunButton.ToolTip"));
            RunButton.UseVisualStyleBackColor = true;
            RunButton.Click += RunButton_Click;
            // 
            // SaveSettingsButton
            // 
            SaveSettingsButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            SaveSettingsButton.Location = new Point(424, 219);
            SaveSettingsButton.Name = "SaveSettingsButton";
            SaveSettingsButton.Size = new Size(200, 100);
            SaveSettingsButton.TabIndex = 2;
            SaveSettingsButton.Text = "Save Settings";
            ToolTip.SetToolTip(SaveSettingsButton, "Saves your settings so they'll be accounted for on the next start-up of this program.");
            SaveSettingsButton.UseVisualStyleBackColor = true;
            SaveSettingsButton.Click += SaveSettingsButton_Click;
            // 
            // CalibrateButton
            // 
            CalibrateButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            CalibrateButton.Location = new Point(218, 219);
            CalibrateButton.Name = "CalibrateButton";
            CalibrateButton.Size = new Size(200, 100);
            CalibrateButton.TabIndex = 1;
            CalibrateButton.Text = "Calibrate";
            ToolTip.SetToolTip(CalibrateButton, resources.GetString("CalibrateButton.ToolTip"));
            CalibrateButton.UseVisualStyleBackColor = true;
            CalibrateButton.Click += CalibrateButton_Click;
            // 
            // SettingsLabel
            // 
            SettingsLabel.AutoSize = true;
            SettingsLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            SettingsLabel.Location = new Point(415, 9);
            SettingsLabel.Name = "SettingsLabel";
            SettingsLabel.Size = new Size(106, 32);
            SettingsLabel.TabIndex = 1;
            SettingsLabel.Text = "Settings";
            // 
            // RPSLabel
            // 
            RPSLabel.AutoSize = true;
            RPSLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            RPSLabel.Location = new Point(415, 49);
            RPSLabel.Name = "RPSLabel";
            RPSLabel.Size = new Size(133, 21);
            RPSLabel.TabIndex = 2;
            RPSLabel.Text = "Runs Per Seconds";
            // 
            // RPSInput
            // 
            RPSInput.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            RPSInput.Location = new Point(554, 47);
            RPSInput.Name = "RPSInput";
            RPSInput.Size = new Size(70, 29);
            RPSInput.TabIndex = 3;
            ToolTip.SetToolTip(RPSInput, resources.GetString("RPSInput.ToolTip"));
            RPSInput.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // KeyLabel
            // 
            KeyLabel.AutoSize = true;
            KeyLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            KeyLabel.Location = new Point(415, 85);
            KeyLabel.Name = "KeyLabel";
            KeyLabel.Size = new Size(113, 21);
            KeyLabel.TabIndex = 4;
            KeyLabel.Text = "Interaction Key";
            // 
            // KeyInput
            // 
            KeyInput.BorderStyle = BorderStyle.FixedSingle;
            KeyInput.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            KeyInput.Location = new Point(554, 82);
            KeyInput.MaxLength = 1;
            KeyInput.Name = "KeyInput";
            KeyInput.Size = new Size(70, 29);
            KeyInput.TabIndex = 4;
            KeyInput.Text = "E";
            ToolTip.SetToolTip(KeyInput, "Set this to reflect your in-game interaction keybind for fishing!\nThis is the button input that auto-fisher will send to Destiny in order to automate fishing.\nDefaults to 'E'.");
            // 
            // GuideLink
            // 
            GuideLink.AutoSize = true;
            GuideLink.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GuideLink.Location = new Point(543, 153);
            GuideLink.Name = "GuideLink";
            GuideLink.Size = new Size(81, 21);
            GuideLink.TabIndex = 5;
            GuideLink.TabStop = true;
            GuideLink.Text = "Use Guide";
            ToolTip.SetToolTip(GuideLink, "Links to a GitHub wiki that goes in-depth on how this program works and how to use it.");
            // 
            // GitHubLink
            // 
            GitHubLink.AutoSize = true;
            GitHubLink.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GitHubLink.Location = new Point(565, 187);
            GitHubLink.Name = "GitHubLink";
            GitHubLink.Size = new Size(59, 21);
            GitHubLink.TabIndex = 6;
            GitHubLink.TabStop = true;
            GitHubLink.Text = "GitHub";
            GitHubLink.LinkClicked += GitHubLink_LinkClicked;
            ToolTip.SetToolTip(GitHubLink, "Links to this program's repository, useful for reporting issues formally.");
            // 
            // PredictionEngineCheckbox
            // 
            PredictionEngineCheckbox.AutoSize = true;
            PredictionEngineCheckbox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PredictionEngineCheckbox.Location = new Point(415, 118);
            PredictionEngineCheckbox.Name = "PredictionEngineCheckbox";
            PredictionEngineCheckbox.Size = new Size(180, 25);
            PredictionEngineCheckbox.TabIndex = 7;
            PredictionEngineCheckbox.Text = "Use Prediction Engine";
            ToolTip.SetToolTip(PredictionEngineCheckbox, resources.GetString("PredictionEngineCheckbox.ToolTip"));
            PredictionEngineCheckbox.UseVisualStyleBackColor = true;
            // 
            // ToolTip
            // 
            ToolTip.AutoPopDelay = 3000;
            ToolTip.InitialDelay = 500;
            ToolTip.ReshowDelay = 100;
            // 
            // AutoFishingUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(636, 328);
            Controls.Add(InfoTextbox);
            Controls.Add(SettingsLabel);
            Controls.Add(RPSLabel);
            Controls.Add(RPSInput);
            Controls.Add(KeyLabel);
            Controls.Add(KeyInput);
            Controls.Add(RunButton);
            Controls.Add(CalibrateButton);
            Controls.Add(SaveSettingsButton);
            Controls.Add(GuideLink);
            Controls.Add(GitHubLink);
            Controls.Add(PredictionEngineCheckbox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AutoFishingUI";
            Text = "Destiny 2 Auto-Fisher";
            Load += AutoFishingUI_Load;
            ((ISupportInitialize)RPSInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && Components != null)
            {
                Components.Dispose();
            }

            base.Dispose(disposing);
        }

        private IContainer components;
    }
}