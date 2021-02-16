using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using RIA.Grabber;
using RIA.Grabber.Services;

namespace RIA.GUI
{
    public partial class MainForm : Form
    {
        private readonly List<Image> _listImage = new List<Image>();

        private ResultDirectoryWatcher _dirWatcher = new ResultDirectoryWatcher();
        private PageModelFromJsonConverter _converter = new PageModelFromJsonConverter();
        private RiaPageProcessor _processor;

        internal void InjectDependencies(RiaPageProcessor processor, PageModelFromJsonConverter converter, ResultDirectoryWatcher dirWatcher)
        {
            _processor = processor;
            _converter = converter;
            _dirWatcher = dirWatcher;
        }

        private int _numberPictures;

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
                _processor.ProcessPage(Url.Text, PathJson.Text);
                AddFileInListJsonFiles();
            }
            catch(DirectoryNotFoundException)
            {
                TextPage.Text = @"В папке нет json файлов";
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
            if (_numberPictures < _listImage.Count - 1)
            {
                _numberPictures++;
                ImageInPage.Image = _listImage[_numberPictures];
            }

            if (_numberPictures == _listImage.Count - 1)
                ButtonNextPicture.Enabled = false;
        }

        private void ButtonPreviousPicture_Click(object sender, EventArgs e)
        {
            ButtonNextPicture.Enabled = true;
            if (_numberPictures > 0)
            {
                _numberPictures--;
                ImageInPage.Image = _listImage[_numberPictures];
            }

            if (_numberPictures == 0)
                ButtonPreviousPicture.Enabled = false;
        }

        private void ViewJsonFiles()
        {
            var filename = ListJsonFiles.GetItemText(ListJsonFiles.SelectedItem);
            var pageModel = _converter.PageModelJson(filename, PathJson.Text);
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
                    var memoryStream = new MemoryStream(byteArray);
                    var newImage = Image.FromStream(memoryStream);
                    _listImage.Add(newImage);
                }

                if (_listImage.Count > 1)
                {
                    ButtonNextPicture.Enabled = true;
                    ButtonPreviousPicture.Enabled = false;

                    _numberPictures = 0;
                    ImageInPage.Image = _listImage[_numberPictures];
                }
                else if (_listImage.Count == 1)
                {
                    _numberPictures = 0;
                    ImageInPage.Image = _listImage[_numberPictures];
                }
                else
                {
                    var currentDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    ImageInPage.Image = Image.FromFile(Path.Combine(currentDirPath ?? string.Empty, "No images in page.png"));
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
                var fileNameArray = _dirWatcher.ListJsonFilesUpdate(PathJson.Text);
                foreach (var fi in fileNameArray)
                {
                    ListJsonFiles.Items.Add(fi);
                }
            }
            catch (DirectoryNotFoundException)
            {
                TextPage.Text = @"В папке нет json файлов";
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

            foreach (var img in _listImage)
                img.Dispose();

            _listImage.Clear();
            ButtonNextPicture.Enabled = false;
            ButtonPreviousPicture.Enabled = false;
        }
    }
}
