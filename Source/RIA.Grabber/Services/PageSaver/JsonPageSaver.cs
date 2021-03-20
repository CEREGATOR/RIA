namespace RIA.Grabber.Services.PageSaver
{
    using System.IO;
    using System.Text.Json;

    using RIA.Grabber.Model;
    using RIA.Grabber.Services.Utils;

    /// <summary>
    /// Класс по сохранению страницы ria в Json файл.
    /// </summary>
    public class JsonPageSaver : IPageSaver
    {
        private string _dirPath;

        public void Initialize(string dirPath)
        {
            _dirPath = dirPath;
        }

        /// <inheritdoc />
        public void SavePageModel(PageModel pageModel)
        {
            var json = CreateFileContent(pageModel);

            var cleanDirPath = FileUtils.CleanString(_dirPath, Path.GetInvalidPathChars());
            CreateDirIfNotExists(_dirPath);

            var cleanFileName = FileUtils.GetFileName(pageModel.Title);
            var filePath = Path.Combine(cleanDirPath, cleanFileName);
            WriteFileContent(filePath, json);
        }

        /// <summary>
        /// Создает директорию по заданному пути, если она не существует.
        /// </summary>
        /// <param name="dirPath">Путь к директории.</param>
        public virtual void CreateDirIfNotExists(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        /// <summary>
        /// Записывает строковое содержимое в файл.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="content">Содержимое.</param>
        public virtual void WriteFileContent(string path, string content) => File.WriteAllText(path, content);

        /// <summary>
        /// Метод по добавлению модели страницы в json файл.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        private string CreateFileContent(PageModel pageModel)
        {
            var json = JsonSerializer.Serialize(pageModel);
            return json;
        }
    }
}