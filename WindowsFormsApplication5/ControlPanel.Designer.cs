namespace WindowsFormsApplication5
{
    partial class ControlPanel
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
            this.AllTeamsStatisticsBtn = new System.Windows.Forms.Button();
            this.TeamStatisticsBtn = new System.Windows.Forms.Button();
            this.TeamIdTextInput = new System.Windows.Forms.TextBox();
            this.TeamsStatisticsHtmlFileBtn = new System.Windows.Forms.Button();
            this.emptyDbBtn = new System.Windows.Forms.Button();
            this.teamsToCompare = new System.Windows.Forms.TextBox();
            this.compareBtn = new System.Windows.Forms.Button();
            this.browseBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.updatePropsBtn = new System.Windows.Forms.Button();
            this.rootFolderLbl = new System.Windows.Forms.Label();
            this.chooseDaysBtn = new System.Windows.Forms.Button();
            this.daysLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AllTeamsStatisticsBtn
            // 
            this.AllTeamsStatisticsBtn.Location = new System.Drawing.Point(12, 57);
            this.AllTeamsStatisticsBtn.Name = "AllTeamsStatisticsBtn";
            this.AllTeamsStatisticsBtn.Size = new System.Drawing.Size(100, 23);
            this.AllTeamsStatisticsBtn.TabIndex = 0;
            this.AllTeamsStatisticsBtn.Text = "All Teams!";
            this.AllTeamsStatisticsBtn.UseVisualStyleBackColor = true;
            this.AllTeamsStatisticsBtn.Click += new System.EventHandler(this.AllTeamsStatisticsBtn_Click);
            // 
            // TeamStatisticsBtn
            // 
            this.TeamStatisticsBtn.Location = new System.Drawing.Point(12, 141);
            this.TeamStatisticsBtn.Name = "TeamStatisticsBtn";
            this.TeamStatisticsBtn.Size = new System.Drawing.Size(100, 23);
            this.TeamStatisticsBtn.TabIndex = 1;
            this.TeamStatisticsBtn.Text = "Show Team!";
            this.TeamStatisticsBtn.UseVisualStyleBackColor = true;
            this.TeamStatisticsBtn.Click += new System.EventHandler(this.TeamStatisticsBtn_Click);
            // 
            // TeamIdTextInput
            // 
            this.TeamIdTextInput.Location = new System.Drawing.Point(12, 115);
            this.TeamIdTextInput.Name = "TeamIdTextInput";
            this.TeamIdTextInput.Size = new System.Drawing.Size(100, 20);
            this.TeamIdTextInput.TabIndex = 2;
            this.TeamIdTextInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TeamIdTextInput_KeyPress);
            // 
            // TeamsStatisticsHtmlFileBtn
            // 
            this.TeamsStatisticsHtmlFileBtn.Location = new System.Drawing.Point(74, 86);
            this.TeamsStatisticsHtmlFileBtn.Name = "TeamsStatisticsHtmlFileBtn";
            this.TeamsStatisticsHtmlFileBtn.Size = new System.Drawing.Size(96, 23);
            this.TeamsStatisticsHtmlFileBtn.TabIndex = 3;
            this.TeamsStatisticsHtmlFileBtn.Text = "HTML FTW!";
            this.TeamsStatisticsHtmlFileBtn.UseVisualStyleBackColor = true;
            this.TeamsStatisticsHtmlFileBtn.Click += new System.EventHandler(this.TeamsStatisticsHtmlFileBtn_Click);
            // 
            // emptyDbBtn
            // 
            this.emptyDbBtn.Location = new System.Drawing.Point(139, 57);
            this.emptyDbBtn.Name = "emptyDbBtn";
            this.emptyDbBtn.Size = new System.Drawing.Size(96, 23);
            this.emptyDbBtn.TabIndex = 4;
            this.emptyDbBtn.Text = "Empty Database";
            this.emptyDbBtn.UseVisualStyleBackColor = true;
            this.emptyDbBtn.Click += new System.EventHandler(this.emptyDbBtn_Click);
            // 
            // teamsToCompare
            // 
            this.teamsToCompare.Location = new System.Drawing.Point(139, 115);
            this.teamsToCompare.Name = "teamsToCompare";
            this.teamsToCompare.Size = new System.Drawing.Size(96, 20);
            this.teamsToCompare.TabIndex = 5;
            this.teamsToCompare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.teamsToCompare_KeyPress);
            // 
            // compareBtn
            // 
            this.compareBtn.Location = new System.Drawing.Point(139, 141);
            this.compareBtn.Name = "compareBtn";
            this.compareBtn.Size = new System.Drawing.Size(97, 23);
            this.compareBtn.TabIndex = 6;
            this.compareBtn.Text = "Radar Compare!";
            this.compareBtn.UseVisualStyleBackColor = true;
            this.compareBtn.Click += new System.EventHandler(this.compareBtn_Click);
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(12, 13);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(100, 23);
            this.browseBtn.TabIndex = 7;
            this.browseBtn.Text = "Browse...";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // updatePropsBtn
            // 
            this.updatePropsBtn.Location = new System.Drawing.Point(74, 170);
            this.updatePropsBtn.Name = "updatePropsBtn";
            this.updatePropsBtn.Size = new System.Drawing.Size(96, 23);
            this.updatePropsBtn.TabIndex = 8;
            this.updatePropsBtn.Text = "Update Props...";
            this.updatePropsBtn.UseVisualStyleBackColor = true;
            this.updatePropsBtn.Click += new System.EventHandler(this.updatePropsBtn_Click);
            // 
            // rootFolderLbl
            // 
            this.rootFolderLbl.AutoSize = true;
            this.rootFolderLbl.Location = new System.Drawing.Point(118, 18);
            this.rootFolderLbl.Name = "rootFolderLbl";
            this.rootFolderLbl.Size = new System.Drawing.Size(0, 13);
            this.rootFolderLbl.TabIndex = 9;
            // 
            // chooseDaysBtn
            // 
            this.chooseDaysBtn.Location = new System.Drawing.Point(74, 200);
            this.chooseDaysBtn.Name = "chooseDaysBtn";
            this.chooseDaysBtn.Size = new System.Drawing.Size(96, 23);
            this.chooseDaysBtn.TabIndex = 10;
            this.chooseDaysBtn.Text = "Choose Days";
            this.chooseDaysBtn.UseVisualStyleBackColor = true;
            this.chooseDaysBtn.Click += new System.EventHandler(this.chooseDaysBtn_Click);
            // 
            // daysLbl
            // 
            this.daysLbl.AutoSize = true;
            this.daysLbl.Location = new System.Drawing.Point(177, 206);
            this.daysLbl.Name = "daysLbl";
            this.daysLbl.Size = new System.Drawing.Size(0, 13);
            this.daysLbl.TabIndex = 11;
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 231);
            this.Controls.Add(this.daysLbl);
            this.Controls.Add(this.chooseDaysBtn);
            this.Controls.Add(this.rootFolderLbl);
            this.Controls.Add(this.updatePropsBtn);
            this.Controls.Add(this.browseBtn);
            this.Controls.Add(this.compareBtn);
            this.Controls.Add(this.teamsToCompare);
            this.Controls.Add(this.emptyDbBtn);
            this.Controls.Add(this.TeamsStatisticsHtmlFileBtn);
            this.Controls.Add(this.TeamIdTextInput);
            this.Controls.Add(this.TeamStatisticsBtn);
            this.Controls.Add(this.AllTeamsStatisticsBtn);
            this.Name = "ControlPanel";
            this.Text = "Scouting 2630";
            this.Load += new System.EventHandler(this.ControlPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AllTeamsStatisticsBtn;
        private System.Windows.Forms.Button TeamStatisticsBtn;
        private System.Windows.Forms.TextBox TeamIdTextInput;
        private System.Windows.Forms.Button TeamsStatisticsHtmlFileBtn;
        private System.Windows.Forms.Button emptyDbBtn;
        private System.Windows.Forms.TextBox teamsToCompare;
        private System.Windows.Forms.Button compareBtn;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button updatePropsBtn;
        private System.Windows.Forms.Label rootFolderLbl;
        private System.Windows.Forms.Button chooseDaysBtn;
        private System.Windows.Forms.Label daysLbl;
    }
}