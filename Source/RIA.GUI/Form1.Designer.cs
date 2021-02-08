
namespace RIA.GUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonParser = new System.Windows.Forms.Button();
            this.Url = new System.Windows.Forms.TextBox();
            this.PathJson = new System.Windows.Forms.TextBox();
            this.ListJsonFiles = new System.Windows.Forms.ListBox();
            this.TextPage = new System.Windows.Forms.RichTextBox();
            this.Title = new System.Windows.Forms.TextBox();
            this.PublicationDate = new System.Windows.Forms.TextBox();
            this.LastChangeDate = new System.Windows.Forms.TextBox();
            this.ImageInPage = new System.Windows.Forms.PictureBox();
            this.UrlList = new System.Windows.Forms.ListBox();
            this.DescriptionList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageInPage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ссылка на статью ria.ru";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Путь к папке с Json файлами";
            // 
            // ButtonParser
            // 
            this.ButtonParser.Location = new System.Drawing.Point(118, 149);
            this.ButtonParser.Name = "ButtonParser";
            this.ButtonParser.Size = new System.Drawing.Size(75, 23);
            this.ButtonParser.TabIndex = 2;
            this.ButtonParser.Text = "Старт";
            this.ButtonParser.UseVisualStyleBackColor = true;
            this.ButtonParser.Click += new System.EventHandler(this.ButtonParser_Click);
            // 
            // Url
            // 
            this.Url.Location = new System.Drawing.Point(26, 36);
            this.Url.Name = "Url";
            this.Url.Size = new System.Drawing.Size(259, 23);
            this.Url.TabIndex = 3;
            this.Url.TextChanged += new System.EventHandler(this.Url_TextChanged);
            // 
            // PathJson
            // 
            this.PathJson.Location = new System.Drawing.Point(26, 99);
            this.PathJson.Name = "PathJson";
            this.PathJson.Size = new System.Drawing.Size(259, 23);
            this.PathJson.TabIndex = 4;
            this.PathJson.TextChanged += new System.EventHandler(this.PathJson_TextChanged);
            // 
            // ListJsonFiles
            // 
            this.ListJsonFiles.FormattingEnabled = true;
            this.ListJsonFiles.ItemHeight = 15;
            this.ListJsonFiles.Location = new System.Drawing.Point(319, 30);
            this.ListJsonFiles.Name = "ListJsonFiles";
            this.ListJsonFiles.Size = new System.Drawing.Size(448, 154);
            this.ListJsonFiles.TabIndex = 5;
            this.ListJsonFiles.SelectedIndexChanged += new System.EventHandler(this.ListJsonFiles_SelectedIndexChanged);
            // 
            // TextPage
            // 
            this.TextPage.Location = new System.Drawing.Point(12, 315);
            this.TextPage.Name = "TextPage";
            this.TextPage.ReadOnly = true;
            this.TextPage.Size = new System.Drawing.Size(749, 394);
            this.TextPage.TabIndex = 6;
            this.TextPage.Text = "";
            // 
            // Title
            // 
            this.Title.Location = new System.Drawing.Point(12, 207);
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Size = new System.Drawing.Size(698, 23);
            this.Title.TabIndex = 7;
            // 
            // PublicationDate
            // 
            this.PublicationDate.Location = new System.Drawing.Point(12, 247);
            this.PublicationDate.Name = "PublicationDate";
            this.PublicationDate.ReadOnly = true;
            this.PublicationDate.Size = new System.Drawing.Size(106, 23);
            this.PublicationDate.TabIndex = 8;
            // 
            // LastChangeDate
            // 
            this.LastChangeDate.Location = new System.Drawing.Point(12, 286);
            this.LastChangeDate.Name = "LastChangeDate";
            this.LastChangeDate.ReadOnly = true;
            this.LastChangeDate.Size = new System.Drawing.Size(106, 23);
            this.LastChangeDate.TabIndex = 9;
            // 
            // ImageInPage
            // 
            this.ImageInPage.Location = new System.Drawing.Point(780, 35);
            this.ImageInPage.Name = "ImageInPage";
            this.ImageInPage.Size = new System.Drawing.Size(454, 273);
            this.ImageInPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageInPage.TabIndex = 10;
            this.ImageInPage.TabStop = false;
            // 
            // UrlList
            // 
            this.UrlList.FormattingEnabled = true;
            this.UrlList.ItemHeight = 15;
            this.UrlList.Location = new System.Drawing.Point(767, 314);
            this.UrlList.Name = "UrlList";
            this.UrlList.Size = new System.Drawing.Size(242, 394);
            this.UrlList.TabIndex = 11;
            // 
            // DescriptionList
            // 
            this.DescriptionList.FormattingEnabled = true;
            this.DescriptionList.ItemHeight = 15;
            this.DescriptionList.Location = new System.Drawing.Point(1015, 314);
            this.DescriptionList.Name = "DescriptionList";
            this.DescriptionList.Size = new System.Drawing.Size(223, 394);
            this.DescriptionList.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 721);
            this.Controls.Add(this.DescriptionList);
            this.Controls.Add(this.UrlList);
            this.Controls.Add(this.ImageInPage);
            this.Controls.Add(this.LastChangeDate);
            this.Controls.Add(this.PublicationDate);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.TextPage);
            this.Controls.Add(this.ListJsonFiles);
            this.Controls.Add(this.PathJson);
            this.Controls.Add(this.Url);
            this.Controls.Add(this.ButtonParser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RIA";
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
        private System.Windows.Forms.ListBox ListJsonFiles;
        private System.Windows.Forms.RichTextBox TextPage;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.TextBox PublicationDate;
        private System.Windows.Forms.TextBox LastChangeDate;
        private System.Windows.Forms.PictureBox ImageInPage;
        private System.Windows.Forms.ListBox UrlList;
        private System.Windows.Forms.ListBox DescriptionList;
    }
}

