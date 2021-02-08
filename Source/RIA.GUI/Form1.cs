using System;
using System.Windows.Forms;
using RIA.Grabber;
using RIA.Grabber.Services;
using RIA.Grabber.Model;
using System.IO;
using System.Text.Json;
using System.Drawing;

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
            ViewJsonFiles();
        }

        private void PathJson_TextChanged(object sender, EventArgs e)
        {
            CleaningWorkArea();
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
                TextPage.Text = ex.Message;
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
                TextPage.Text = ex.Message;
            }
        }

        private void Url_TextChanged(object sender, EventArgs e)
        {
            CleaningWorkArea();
        }
        private void CleaningWorkArea()
        {
            ImageInPage.Image = null;
            UrlList.Items.Clear();
            DescriptionList.Items.Clear();
            TextPage.Clear();
            Title.Clear();
            PublicationDate.Clear();
            LastChangeDate.Clear();
        }
        private void ViewJsonFiles()
        {
            string filename = ListJsonFiles.GetItemText(ListJsonFiles.SelectedItem);
            var jsonString = File.ReadAllText(Path.Combine(PathJson.Text, filename));
            var pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

            try
            {
                CleaningWorkArea();

                Title.AppendText(pageModel.Title);
                PublicationDate.AppendText(pageModel.PublicationDate.ToString());
                LastChangeDate.AppendText(pageModel.LastChangeDate.ToString());
                TextPage.AppendText(pageModel.Text + Environment.NewLine);
                foreach (var textLink in pageModel.LinksInText)
                {
                    UrlList.Items.Add(textLink.Url);
                    DescriptionList.Items.Add(textLink.Description);
                }
                foreach (var imgLink in pageModel.ImageLinks)
                {
                    var byteArray = DataDownloader.DownloadByteArray(imgLink);
                    MemoryStream memoryStream = new MemoryStream(byteArray);

                    ImageInPage.Image = Image.FromStream(memoryStream);
                }
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }
    }
}
