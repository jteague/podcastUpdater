namespace podcastUpdater
{
    partial class AddNewEpisode
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
            this.label15 = new System.Windows.Forms.Label();
            this.txtEpisodeGuid = new System.Windows.Forms.TextBox();
            this.btnEpisodeUrlTest = new System.Windows.Forms.Button();
            this.txtEpisodeTitle = new System.Windows.Forms.TextBox();
            this.datePickerEpisode = new System.Windows.Forms.DateTimePicker();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEpisodeSummary = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtEpisodeKeywords = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtEpisodeDuration = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEpisodeUrl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEpisodeAuthor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEpisodeSubtitle = new System.Windows.Forms.TextBox();
            this.btnSelectEpisodeAudioFile = new System.Windows.Forms.Button();
            this.btn_AddEpisode = new System.Windows.Forms.Button();
            this.cb_isExplicit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_EpisodeFilePath = new System.Windows.Forms.TextBox();
            this.btn_upload = new System.Windows.Forms.Button();
            this.progBar_uploadFile = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(46, 305);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 58;
            this.label15.Text = "Guid:";
            // 
            // txtEpisodeGuid
            // 
            this.txtEpisodeGuid.Location = new System.Drawing.Point(124, 305);
            this.txtEpisodeGuid.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeGuid.Name = "txtEpisodeGuid";
            this.txtEpisodeGuid.ReadOnly = true;
            this.txtEpisodeGuid.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeGuid.TabIndex = 57;
            // 
            // btnEpisodeUrlTest
            // 
            this.btnEpisodeUrlTest.Location = new System.Drawing.Point(274, 184);
            this.btnEpisodeUrlTest.Name = "btnEpisodeUrlTest";
            this.btnEpisodeUrlTest.Size = new System.Drawing.Size(59, 19);
            this.btnEpisodeUrlTest.TabIndex = 56;
            this.btnEpisodeUrlTest.Text = "Test Link";
            this.btnEpisodeUrlTest.UseVisualStyleBackColor = true;
            this.btnEpisodeUrlTest.Click += new System.EventHandler(this.btnEpisodeUrlTest_Click);
            // 
            // txtEpisodeTitle
            // 
            this.txtEpisodeTitle.Location = new System.Drawing.Point(124, 114);
            this.txtEpisodeTitle.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeTitle.Name = "txtEpisodeTitle";
            this.txtEpisodeTitle.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeTitle.TabIndex = 55;
            // 
            // datePickerEpisode
            // 
            this.datePickerEpisode.Location = new System.Drawing.Point(124, 280);
            this.datePickerEpisode.Name = "datePickerEpisode";
            this.datePickerEpisode.Size = new System.Drawing.Size(209, 20);
            this.datePickerEpisode.TabIndex = 54;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(46, 114);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 53;
            this.label17.Text = "Title:";
            // 
            // txtEpisodeSummary
            // 
            this.txtEpisodeSummary.Location = new System.Drawing.Point(124, 329);
            this.txtEpisodeSummary.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeSummary.Multiline = true;
            this.txtEpisodeSummary.Name = "txtEpisodeSummary";
            this.txtEpisodeSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEpisodeSummary.Size = new System.Drawing.Size(209, 114);
            this.txtEpisodeSummary.TabIndex = 52;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(46, 280);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 51;
            this.label14.Text = "Date:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(46, 231);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Keywords:";
            // 
            // txtEpisodeKeywords
            // 
            this.txtEpisodeKeywords.Location = new System.Drawing.Point(124, 231);
            this.txtEpisodeKeywords.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeKeywords.Name = "txtEpisodeKeywords";
            this.txtEpisodeKeywords.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeKeywords.TabIndex = 49;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(46, 208);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Duration:";
            // 
            // txtEpisodeDuration
            // 
            this.txtEpisodeDuration.Location = new System.Drawing.Point(124, 208);
            this.txtEpisodeDuration.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeDuration.Name = "txtEpisodeDuration";
            this.txtEpisodeDuration.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeDuration.TabIndex = 47;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(46, 329);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "Summary:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(46, 184);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 45;
            this.label10.Text = "URL:";
            // 
            // txtEpisodeUrl
            // 
            this.txtEpisodeUrl.Location = new System.Drawing.Point(124, 184);
            this.txtEpisodeUrl.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeUrl.Name = "txtEpisodeUrl";
            this.txtEpisodeUrl.Size = new System.Drawing.Size(145, 20);
            this.txtEpisodeUrl.TabIndex = 44;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(46, 161);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 43;
            this.label9.Text = "Author:";
            // 
            // txtEpisodeAuthor
            // 
            this.txtEpisodeAuthor.Location = new System.Drawing.Point(124, 161);
            this.txtEpisodeAuthor.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeAuthor.Name = "txtEpisodeAuthor";
            this.txtEpisodeAuthor.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeAuthor.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(46, 138);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Subtitle:";
            // 
            // txtEpisodeSubtitle
            // 
            this.txtEpisodeSubtitle.Location = new System.Drawing.Point(124, 138);
            this.txtEpisodeSubtitle.Margin = new System.Windows.Forms.Padding(2);
            this.txtEpisodeSubtitle.Name = "txtEpisodeSubtitle";
            this.txtEpisodeSubtitle.Size = new System.Drawing.Size(209, 20);
            this.txtEpisodeSubtitle.TabIndex = 40;
            // 
            // btnSelectEpisodeAudioFile
            // 
            this.btnSelectEpisodeAudioFile.Location = new System.Drawing.Point(155, 12);
            this.btnSelectEpisodeAudioFile.Name = "btnSelectEpisodeAudioFile";
            this.btnSelectEpisodeAudioFile.Size = new System.Drawing.Size(98, 38);
            this.btnSelectEpisodeAudioFile.TabIndex = 59;
            this.btnSelectEpisodeAudioFile.Text = "Select Audio File";
            this.btnSelectEpisodeAudioFile.UseVisualStyleBackColor = true;
            this.btnSelectEpisodeAudioFile.Click += new System.EventHandler(this.btnSelectEpisodeAudioFile_Click);
            // 
            // btn_AddEpisode
            // 
            this.btn_AddEpisode.Location = new System.Drawing.Point(246, 448);
            this.btn_AddEpisode.Name = "btn_AddEpisode";
            this.btn_AddEpisode.Size = new System.Drawing.Size(87, 35);
            this.btn_AddEpisode.TabIndex = 60;
            this.btn_AddEpisode.Text = "Add Episode";
            this.btn_AddEpisode.UseVisualStyleBackColor = true;
            this.btn_AddEpisode.Click += new System.EventHandler(this.btn_AddEpisode_Click);
            // 
            // cb_isExplicit
            // 
            this.cb_isExplicit.AutoSize = true;
            this.cb_isExplicit.Location = new System.Drawing.Point(124, 257);
            this.cb_isExplicit.Name = "cb_isExplicit";
            this.cb_isExplicit.Size = new System.Drawing.Size(59, 17);
            this.cb_isExplicit.TabIndex = 61;
            this.cb_isExplicit.Text = "Explicit";
            this.cb_isExplicit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 258);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Is Explicit:";
            // 
            // txt_EpisodeFilePath
            // 
            this.txt_EpisodeFilePath.Location = new System.Drawing.Point(11, 57);
            this.txt_EpisodeFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.txt_EpisodeFilePath.Name = "txt_EpisodeFilePath";
            this.txt_EpisodeFilePath.ReadOnly = true;
            this.txt_EpisodeFilePath.Size = new System.Drawing.Size(258, 20);
            this.txt_EpisodeFilePath.TabIndex = 63;
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(274, 55);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(110, 23);
            this.btn_upload.TabIndex = 64;
            this.btn_upload.Text = "Upload Audio File";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // progBar_uploadFile
            // 
            this.progBar_uploadFile.Location = new System.Drawing.Point(12, 83);
            this.progBar_uploadFile.Name = "progBar_uploadFile";
            this.progBar_uploadFile.Size = new System.Drawing.Size(372, 23);
            this.progBar_uploadFile.TabIndex = 65;
            // 
            // AddNewEpisode
            // 
            this.AcceptButton = this.btn_AddEpisode;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 495);
            this.Controls.Add(this.progBar_uploadFile);
            this.Controls.Add(this.btn_upload);
            this.Controls.Add(this.txt_EpisodeFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_isExplicit);
            this.Controls.Add(this.btn_AddEpisode);
            this.Controls.Add(this.btnSelectEpisodeAudioFile);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtEpisodeGuid);
            this.Controls.Add(this.btnEpisodeUrlTest);
            this.Controls.Add(this.txtEpisodeTitle);
            this.Controls.Add(this.datePickerEpisode);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtEpisodeSummary);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtEpisodeKeywords);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtEpisodeDuration);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtEpisodeUrl);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtEpisodeAuthor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtEpisodeSubtitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddNewEpisode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Episode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtEpisodeGuid;
        private System.Windows.Forms.Button btnEpisodeUrlTest;
        private System.Windows.Forms.TextBox txtEpisodeTitle;
        private System.Windows.Forms.DateTimePicker datePickerEpisode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtEpisodeSummary;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtEpisodeKeywords;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtEpisodeDuration;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEpisodeUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEpisodeAuthor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEpisodeSubtitle;
        private System.Windows.Forms.Button btnSelectEpisodeAudioFile;
        private System.Windows.Forms.Button btn_AddEpisode;
        private System.Windows.Forms.CheckBox cb_isExplicit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_EpisodeFilePath;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.ProgressBar progBar_uploadFile;
    }
}