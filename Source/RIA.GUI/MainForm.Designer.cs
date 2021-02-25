namespace RIA.GUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonParser = new System.Windows.Forms.Button();
            this.Url = new System.Windows.Forms.TextBox();
            this.PathJson = new System.Windows.Forms.TextBox();
            this.ListPage = new System.Windows.Forms.ListBox();
            this.TextPage = new System.Windows.Forms.RichTextBox();
            this.Title = new System.Windows.Forms.TextBox();
            this.PublicationDate = new System.Windows.Forms.TextBox();
            this.LastChangeDate = new System.Windows.Forms.TextBox();
            this.ImageInPage = new System.Windows.Forms.PictureBox();
            this.UrlList = new System.Windows.Forms.ListBox();
            this.DescriptionList = new System.Windows.Forms.ListBox();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DatePublicationLabel = new System.Windows.Forms.Label();
            this.LastChangeDateLabel = new System.Windows.Forms.Label();
            this.ButtonNextPicture = new System.Windows.Forms.Button();
            this.ButtonPreviousPicture = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImageInPage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ButtonParser
            // 
            resources.ApplyResources(this.ButtonParser, "ButtonParser");
            this.ButtonParser.Name = "ButtonParser";
            this.ButtonParser.UseVisualStyleBackColor = true;
            this.ButtonParser.Click += new System.EventHandler(this.ButtonParser_Click);
            // 
            // Url
            // 
            resources.ApplyResources(this.Url, "Url");
            this.Url.Name = "Url";
            this.Url.TextChanged += new System.EventHandler(this.Url_TextChanged);
            // 
            // PathJson
            // 
            resources.ApplyResources(this.PathJson, "PathJson");
            this.PathJson.Name = "PathJson";
            this.PathJson.TextChanged += new System.EventHandler(this.PathJson_TextChanged);
            // 
            // ListPage
            // 
            this.ListPage.FormattingEnabled = true;
            resources.ApplyResources(this.ListPage, "ListPage");
            this.ListPage.Name = "ListPage";
            this.ListPage.SelectedIndexChanged += new System.EventHandler(this.ListPage_SelectedIndexChanged);
            // 
            // TextPage
            // 
            resources.ApplyResources(this.TextPage, "TextPage");
            this.TextPage.Name = "TextPage";
            this.TextPage.ReadOnly = true;
            // 
            // Title
            // 
            resources.ApplyResources(this.Title, "Title");
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // PublicationDate
            // 
            resources.ApplyResources(this.PublicationDate, "PublicationDate");
            this.PublicationDate.Name = "PublicationDate";
            this.PublicationDate.ReadOnly = true;
            // 
            // LastChangeDate
            // 
            resources.ApplyResources(this.LastChangeDate, "LastChangeDate");
            this.LastChangeDate.Name = "LastChangeDate";
            this.LastChangeDate.ReadOnly = true;
            // 
            // ImageInPage
            // 
            resources.ApplyResources(this.ImageInPage, "ImageInPage");
            this.ImageInPage.Name = "ImageInPage";
            this.ImageInPage.TabStop = false;
            // 
            // UrlList
            // 
            this.UrlList.FormattingEnabled = true;
            resources.ApplyResources(this.UrlList, "UrlList");
            this.UrlList.Name = "UrlList";
            this.UrlList.SelectedIndexChanged += new System.EventHandler(this.UrlList_SelectedIndexChanged);
            // 
            // DescriptionList
            // 
            this.DescriptionList.FormattingEnabled = true;
            resources.ApplyResources(this.DescriptionList, "DescriptionList");
            this.DescriptionList.Name = "DescriptionList";
            this.DescriptionList.SelectedIndexChanged += new System.EventHandler(this.DescriptionList_SelectedIndexChanged);
            // 
            // TitleLabel
            // 
            resources.ApplyResources(this.TitleLabel, "TitleLabel");
            this.TitleLabel.Name = "TitleLabel";
            // 
            // DatePublicationLabel
            // 
            resources.ApplyResources(this.DatePublicationLabel, "DatePublicationLabel");
            this.DatePublicationLabel.Name = "DatePublicationLabel";
            // 
            // LastChangeDateLabel
            // 
            resources.ApplyResources(this.LastChangeDateLabel, "LastChangeDateLabel");
            this.LastChangeDateLabel.Name = "LastChangeDateLabel";
            // 
            // ButtonNextPicture
            // 
            resources.ApplyResources(this.ButtonNextPicture, "ButtonNextPicture");
            this.ButtonNextPicture.Name = "ButtonNextPicture";
            this.ButtonNextPicture.UseVisualStyleBackColor = true;
            this.ButtonNextPicture.Click += new System.EventHandler(this.ButtonNextPicture_Click);
            // 
            // ButtonPreviousPicture
            // 
            resources.ApplyResources(this.ButtonPreviousPicture, "ButtonPreviousPicture");
            this.ButtonPreviousPicture.Name = "ButtonPreviousPicture";
            this.ButtonPreviousPicture.UseVisualStyleBackColor = true;
            this.ButtonPreviousPicture.Click += new System.EventHandler(this.ButtonPreviousPicture_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ButtonPreviousPicture);
            this.Controls.Add(this.ButtonNextPicture);
            this.Controls.Add(this.LastChangeDateLabel);
            this.Controls.Add(this.DatePublicationLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.DescriptionList);
            this.Controls.Add(this.UrlList);
            this.Controls.Add(this.ImageInPage);
            this.Controls.Add(this.LastChangeDate);
            this.Controls.Add(this.PublicationDate);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.TextPage);
            this.Controls.Add(this.ListPage);
            this.Controls.Add(this.PathJson);
            this.Controls.Add(this.Url);
            this.Controls.Add(this.ButtonParser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.ImageInPage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonParser;
        private System.Windows.Forms.TextBox Url;
        private System.Windows.Forms.TextBox PathJson;
        private System.Windows.Forms.ListBox ListPage;
        private System.Windows.Forms.RichTextBox TextPage;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.TextBox PublicationDate;
        private System.Windows.Forms.TextBox LastChangeDate;
        private System.Windows.Forms.PictureBox ImageInPage;
        private System.Windows.Forms.ListBox UrlList;
        private System.Windows.Forms.ListBox DescriptionList;
        private System.Windows.Forms.Label PageTitle;
        private System.Windows.Forms.Label DatePublicationPageLabel;
        private System.Windows.Forms.Label LastChangeDateLabel;
        private System.Windows.Forms.Label PageTitleLabel;
        private System.Windows.Forms.Label LastChangeDatePage;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DatePublicationLabel;
        private System.Windows.Forms.Button ButtonNextPicture;
        private System.Windows.Forms.Button ButtonPreviousPicture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

