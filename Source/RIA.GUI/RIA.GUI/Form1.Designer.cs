
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
            this.ViewPage = new System.Windows.Forms.RichTextBox();
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
            // ViewPage
            // 
            this.ViewPage.Location = new System.Drawing.Point(18, 204);
            this.ViewPage.Name = "ViewPage";
            this.ViewPage.Size = new System.Drawing.Size(749, 419);
            this.ViewPage.TabIndex = 6;
            this.ViewPage.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 635);
            this.Controls.Add(this.ViewPage);
            this.Controls.Add(this.ListJsonFiles);
            this.Controls.Add(this.PathJson);
            this.Controls.Add(this.Url);
            this.Controls.Add(this.ButtonParser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RIA";
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
        private System.Windows.Forms.RichTextBox ViewPage;
    }
}

