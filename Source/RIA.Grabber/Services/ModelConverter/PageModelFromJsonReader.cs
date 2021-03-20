namespace RIA.Grabber.Services.ModelConverter
{
    using System.IO;
    using System.Text.Json;

    using RIA.Grabber.Model;
    using RIA.Grabber.Services.Utils;

    /// <summary>
    /// Читает страницы из папки с json файлами.
    /// </summary>
    public class PageModelFromJsonReader : IPageModelReader
    {
        /// <summary>
        /// Путь к папке с json файлами.
        /// </summary>
        private string _path;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PageModelFromJsonReader"/>.
        /// </summary>
        /// <param name="path">Интерфейс, предоставляющий доступ к папке с json файлами.</param>
        public void Initialize(string path)
        {
            _path = path;
        }

        /// <inheritdoc />
        public PageModel GetPage(string pageTitle)
        {
            var filename = FileUtils.GetFileName(pageTitle);

            var jsonString = File.ReadAllText(Path.Combine(_path, filename));
            var pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

            return pageModel;
        }
    }
}
