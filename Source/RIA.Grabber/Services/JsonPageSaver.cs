namespace RIA.Grabber.Services
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;

    using Model;

    /// <summary>
    /// Класс по сохранению страницы ria в Json файл.
    /// </summary>
    public class JsonPageSaver : IPageSaver
    {
        /// <summary>
        /// Размер ограничения длины названия json файла.
        /// </summary>
        private const int MaxFileNameLength = 70;

        /// <inheritdoc />
        public void SavePageModel(PageModel pageModel, string dirPath)
        {
            var json = CreateFileContent(pageModel);

            var cleanDirPath = CleanString(dirPath, Path.GetInvalidPathChars());
            CreateDirIfNotExists(dirPath);

            var filename = pageModel.Title.Length <= MaxFileNameLength
                ? pageModel.Title
                : pageModel.Title.Substring(0, MaxFileNameLength);

            var cleanFileName = $"{CleanString(filename, Path.GetInvalidFileNameChars())}.json";
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

        /// <summary>
        /// Очистка строки (пути директории или названия файла) от недопустимых символов.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="invalidChars"></param>
        /// <returns></returns>
        private string CleanString(string input, char[] invalidChars)
        {
            var builder = new StringBuilder();
            foreach (var cur in input.Where(cur => !invalidChars.Contains(cur)))
                builder.Append(cur);

            return builder.ToString();
        }
    }
}