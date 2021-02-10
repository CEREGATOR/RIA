using System;
using System.Windows.Forms;
using RIA.Grabber;
using RIA.Grabber.Services;
using RIA.Grabber.Model;
using System.IO;
using System.Text.Json;
using System.Drawing;
using System.Collections.Generic;

namespace RIA.GUI
{
    public partial class MainForm : Form
    {
        private List<Image> ListImage = new List<Image>();
        private int NumberPictures;
        private PageModel pageModel = new PageModel();
        private Image previousImage;

        public MainForm()
        {
            InitializeComponent();
            CleaningWorkArea();
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
            ListImage.Clear();
            ButtonNextPicture.Enabled = false;
            ButtonPreviousPicture.Enabled = false;
        }

        private void ViewJsonFiles()
        {
            string filename = ListJsonFiles.GetItemText(ListJsonFiles.SelectedItem);
            var jsonString = File.ReadAllText(Path.Combine(PathJson.Text, filename));
            pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

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
                foreach (var imgBase64 in pageModel.ImagesInBase64)
                {
                    var byteArray = Convert.FromBase64String(imgBase64);
                    MemoryStream memoryStream = new MemoryStream(byteArray);
                    previousImage = Image.FromStream(memoryStream);
                    ListImage.Add(previousImage);
                }
                //previousImage?.Dispose(); //Dispose кращет последнее изображение
                if (pageModel.ImagesInBase64.Count > 1)
                {
                    ButtonNextPicture.Enabled = true;
                    ButtonPreviousPicture.Enabled = false;

                    NumberPictures = 0;
                    ImageInPage.Image = ListImage[NumberPictures];
                }
                if (pageModel.ImagesInBase64.Count == 1)
                {
                    NumberPictures = 0;
                    ImageInPage.Image = ListImage[NumberPictures];
                }
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        private void UrlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UrlList.ClearSelected();
        }

        private void DescriptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DescriptionList.ClearSelected();
        }

        private void ButtonNextPicture_Click(object sender, EventArgs e)
        {
            ButtonPreviousPicture.Enabled = true;
            if (NumberPictures < ListImage.Count - 1)
            {
                NumberPictures++;
                ImageInPage.Image = ListImage[NumberPictures];
            }
            if (NumberPictures == ListImage.Count - 1)
                ButtonNextPicture.Enabled = false;
        }

        private void ButtonPreviousPicture_Click(object sender, EventArgs e)
        {
            ButtonNextPicture.Enabled = true;
            if (NumberPictures > 0)
            {
                NumberPictures--;
                ImageInPage.Image = ListImage[NumberPictures];
            }
            if (NumberPictures == 0)
                ButtonPreviousPicture.Enabled = false;
        }
    }
}
