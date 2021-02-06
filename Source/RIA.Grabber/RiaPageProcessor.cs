namespace RIA.Grabber
{
    using System;
    using System.Text.RegularExpressions;

    using RIA.Grabber.Services;

    /// <summary>
    /// Обеспечивает сбор новости с риа ру.
    /// </summary>
    public class RiaPageProcessor
    {
        /// <summary>
        /// Регулярное выражение определяющее валидность URL.
        /// </summary>
        private static readonly Regex CheckUrlRegex;

        /// <summary>
        /// Загрузчик данных из интернета.
        /// </summary>
        private readonly DataDownloader _downloader;

        /// <summary>
        /// Парсер HTML кода.
        /// </summary>
        private readonly HtmlParser _parser;

        /// <summary>
        /// Интерфейс, предоставляющий метод сохранения модели страницы.
        /// </summary>
        private readonly IPageSaver _saver;

        /// <summary>
        /// Инициализирует статические поля класса <see cref="RiaPageProcessor"/>.
        /// </summary>
        static RiaPageProcessor()
        {
            CheckUrlRegex = new Regex("https://ria\\.ru/\\d{8}/\\w+-\\d+\\.html$", RegexOptions.Singleline);
        }

        /// <summary>
        /// Процессор по получению спарсенных данных из ria и их сохранению в файл
        /// </summary>
        /// <param name="downloader">Загрузчик данных из интернета.</param>
        /// <param name="parser">Парсер HTML кода.</param>
        /// <param name="saver">Интерфейс, предоставляющий метод сохранения модели страницы.</param>
        public RiaPageProcessor(DataDownloader downloader, HtmlParser parser, IPageSaver saver)
        {
            _downloader = downloader;
            _parser = parser;
            _saver = saver;
        }

        /// <summary>
        /// Выполняет сбор новости.
        /// </summary>
        /// <param name="url">Ссылка на страницу новости.</param>
        /// <param name="saveDirPath">Путь к директории, куда сохраняется результат.</param>
        public void ProcessPage(string url, string saveDirPath)
        {
            if (!CheckUrlRegex.IsMatch(url))
                throw new Exception("URL не ведет на новость ria.ru.");

            var htmlCode = _downloader.DownloadPageHtml(url);
            var pageModel = _parser.ParsePage(htmlCode);

            _saver.SavePageModel(pageModel, saveDirPath);
        }
    }
}