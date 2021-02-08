using System;
using System.Windows.Forms;
using RIA.Grabber;
using RIA.Grabber.Services;
using RIA.Grabber.Model;
using System.IO;
using System.Text.Json;

namespace RIA.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonParser_Click(object sender, EventArgs e)
        {
            StartUp();
        }
        private void ListJsonFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = ListJsonFiles.GetItemText(ListJsonFiles.SelectedItem);
            var jsonString = File.ReadAllText(Path.Combine(PathJson.Text, filename));
            var pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

            try
            {
                ViewPage.Clear();
                ViewPage.AppendText(pageModel.Title + Environment.NewLine);
                ViewPage.AppendText(pageModel.PublicationDate.ToString() + Environment.NewLine);
                ViewPage.AppendText(pageModel.LastChangeDate.ToString() + Environment.NewLine);
                ViewPage.AppendText(pageModel.Text + Environment.NewLine);
                foreach(var textLink in pageModel.LinksInText)
                {
                    ViewPage.AppendText("Текст ссылки: " + textLink.Description + Environment.NewLine);
                    ViewPage.AppendText("Cсылка: " + textLink.Url + Environment.NewLine);
                }
                foreach (var imgLink in pageModel.ImageLinks)
                {
                    ViewPage.AppendText("Cсылка на изображение: " + imgLink + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                ViewPage.Text = ex.Message;
            }
        }

        private void PathJson_TextChanged(object sender, EventArgs e)
        {
            ViewPage.Clear();
            ListJsonFilesUpdate();
        }

        private void ListJsonFilesUpdate()
        {
            ListJsonFiles.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(PathJson.Text);
            try
            {
                FileInfo[] files = dir.GetFiles("*.json");
                foreach (FileInfo fi in files)
                {
                    ListJsonFiles.Items.Add(fi.Name.ToString());
                }
            }
            catch (Exception ex)
            {
                ViewPage.Text = ex.Message;
            }
        }
        private void StartUp()
        {
            try
            {
                var dataDownloader = new DataDownloader();
                var htmlParser = new HtmlParser();
                var jsonPageSaver = new JsonPageSaver();
                var processor = new RiaPageProcessor(dataDownloader, htmlParser, jsonPageSaver);

                processor.ProcessPage(Url.Text, PathJson.Text);

                ListJsonFilesUpdate();
            }
            catch (Exception ex)
            {
                ViewPage.Text = ex.Message;
            }
        }

        private void Url_TextChanged(object sender, EventArgs e)
        {
            ViewPage.Clear();
        }
    }
}
