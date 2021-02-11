using System;
using System.Windows.Forms;
using RIA.Grabber;
using RIA.Grabber.Services;
using RIA.Grabber.Model;
using RIA.GUI.Services;
using System.IO;
using System.Text.Json;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;


namespace RIA.GUI
{
    public partial class MainForm : Form
    {
        private List<Image> ListImage = new List<Image>();
        private int NumberPictures;
        private Image previousImage;
        private GetFileNameJson getFile = new GetFileNameJson();
        private GetPageModel Parse = new GetPageModel();
        private GetPageModelFromJson PageModel = new GetPageModelFromJson();

        public MainForm()
        {
            InitializeComponent();
            CleaningWorkArea();
        }

        private void ButtonParser_Click(object sender, EventArgs e)
        {
            try
            {
                CleaningWorkArea();
                Parse.StartParse(Url.Text, PathJson.Text);
                AddFileInListJsonFiles();
            }
            catch(DirectoryNotFoundException ex)
            {
                TextPage.Text = "В папке нет json файлов";
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        private void ListJsonFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ListJsonFiles.SelectedItem != null)
            {
                ViewJsonFiles();
            }
        }

        private void PathJson_TextChanged(object sender, EventArgs e)
        {
            CleaningWorkArea();
            AddFileInListJsonFiles();
        }

        private void Url_TextChanged(object sender, EventArgs e)
        {
            CleaningWorkArea();
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

        private void ViewJsonFiles()
        {
            string filename = ListJsonFiles.GetItemText(ListJsonFiles.SelectedItem);
            var pageModel = PageModel.PageModelJson(filename, PathJson.Text);
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
                if (pageModel.ImagesInBase64.Count == 0)
                {
                    ImageInPage.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        , "No images in page.png"));
                }
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        private void AddFileInListJsonFiles()
        {
            ListJsonFiles.Items.Clear();
            try
            {
                var fileNameArray = getFile.ListJsonFilesUpdate(PathJson.Text);
                foreach (string fi in fileNameArray)
                {
                    ListJsonFiles.Items.Add(fi);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                TextPage.Text = "В папке нет json файлов";
            }
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
    }
}
