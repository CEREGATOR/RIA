namespace RIA.Grabber.Services.Utils
{
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Предоставляет вспомогательные методы для работы с файлами.
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// Размер ограничения длины названия json файла.
        /// </summary>
        private const int MaxFileNameLength = 70;

        /// <summary>
        /// Получает название файла.
        /// </summary>
        /// <param name="pageTitle">Название статьи.</param>
        /// <returns>Название файла.</returns>
        public static string GetFileName(string pageTitle)
        {
            var filename = pageTitle.Length <= MaxFileNameLength
                ? pageTitle
                : pageTitle.Substring(0, MaxFileNameLength);

            return $"{CleanString(filename, Path.GetInvalidFileNameChars())}.json";
        }

        /// <summary>
        /// Очищает строку (путь к директории или названия файла) от недопустимых символов.
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <param name="invalidChars">Массив недопустимых символов.</param>
        /// <returns>Очищенную строку.</returns>
        public static string CleanString(string input, char[] invalidChars)
        {
            var builder = new StringBuilder();
            foreach (var cur in input.Where(cur => !invalidChars.Contains(cur)))
                builder.Append(cur);

            return builder.ToString();
        }
    }
}