using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using RIA.Grabber;

namespace RIA.GUI
{
    using RIA.Grabber.Services.ModelConverter;
    using RIA.Grabber.Services.ResultWatcher;

    /// <summary>
    /// Элемент управления главной формы приложения. 
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Список изображений выводимых в форму приложения.
        /// </summary>
        private readonly List<Image> _listImage = new List<Image>();

        /// <summary>
        /// Интерфейс, предоставляющий список названий страниц. 
        /// </summary>
        private IResultWatcher _watcher;
        /// <summary>
        /// Интерфейс, позволяющий считывать хранимые страницы. 
        /// </summary>
        private IPageModelReader _reader;
        /// <summary>
        /// Обеспечивает сбор новости с риа ру. 
        /// </summary>
        private RiaPageProcessor _processor;

        /// <summary>
        /// Номер текущей открытой картинки в форме приложения.
        /// </summary>
        private int _numberPictures;

        /// <summary>
        /// Инициализирует новый экземпляр класса.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            CleaningWorkArea();
        }

        /// <summary>
        /// Внедряет зависимости для работы главной формы приложения.
        /// </summary>
        /// <param name="processor">Обеспечивает сбор новости с риа ру.</param>
        /// <param name="reader">Интерфейс, позволяющий считывать хранимые страницы.</param>
        /// <param name="watcher">Интерфейс, предоставляющий список названий страниц.</param>
        internal void InjectDependencies(RiaPageProcessor processor, IPageModelReader reader, IResultWatcher watcher)
        {
            _processor = processor;
            _reader = reader;
            _watcher = watcher;
            
            FillPageList();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Старт".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonParser_Click(object sender, EventArgs e)
        {
            try
            {
                CleaningWorkArea();
                _processor.ProcessPage(Url.Text);
                FillPageList();
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        /// <summary>
        /// Обрабатывает выбранный элемент из списка страниц.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ListPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PageList.SelectedItem == null)
                return;

            ShowPage();
        }

        /// <summary>
        /// !!!НЕ ИСПОЛЬЗУЕТСЯ (Обрабатывает событие изменения "путь к папке с json файлами").
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PathJson_TextChanged(object sender, EventArgs e)
        {
            throw new NotSupportedException("Этот функционал выпиливается.");
        }

        /// <summary>
        /// Обрабатывает событие изменения url страницы ria.ru.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Url_TextChanged(object sender, EventArgs e)
        {
            CleaningWorkArea();
        }

        /// <summary>
        /// Обрабатывает выбранный элемент ссылок из страницы ria.ru.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void UrlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UrlList.ClearSelected();
        }

        /// <summary>
        /// Обрабатывает выбранный элемент текста ссылки из страницы ria.ru.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DescriptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DescriptionList.ClearSelected();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Следующая картинка".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Предыдущая картинка".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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

        /// <summary>
        /// Показывает стрицу ria.ru в форме приложения.
        /// </summary>
        private void ShowPage()
        {
            var pageName = PageList.GetItemText(PageList.SelectedItem);
            var pageModel = _reader.GetPage(pageName);
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
                    ImageInPage.Image =
                        Image.FromFile(Path.Combine(currentDirPath ?? string.Empty, "No images in page.png"));
                }
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        /// <summary>
        /// Добавляет название статей в список статей в форму приложения.
        /// </summary>
        private void FillPageList()
        {
            PageList.Items.Clear();
            try
            {
                var pageTitle = _watcher.GetPagesList();
                foreach (var pg in pageTitle)
                {
                    PageList.Items.Add(pg);
                }
            }
            catch (Exception ex)
            {
                TextPage.Text = ex.Message;
            }
        }

        /// <summary>
        /// Очищает информацию во всех полях в форме приложения.
        /// </summary>
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
