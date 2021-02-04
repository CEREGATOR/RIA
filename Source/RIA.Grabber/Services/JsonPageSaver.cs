namespace RIA.Grabber.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json;

    using RIA.Grabber.Model;

    /// <summary>
    /// Класс по сохранинию стриницы ria в Json файл.
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
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var filename = pageModel.Title.Length <= MaxFileNameLength
                ? pageModel.Title
                : pageModel.Title.Substring(0, MaxFileNameLength);

            var cleanFileName = $"{CleanString(filename, Path.GetInvalidFileNameChars())}.json";
            var filePath = Path.Combine(cleanDirPath, cleanFileName);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Json file create");
            Console.WriteLine();
        }

        /// <summary>
        /// Метод по добавлению модели страницы в json файл.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        private string CreateFileContent(PageModel pageModel)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(pageModel, options);
            return json;
        }

        /// <summary>
        /// Очистка строки (пути дериктории или названия файла) от недопустимых символов.
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