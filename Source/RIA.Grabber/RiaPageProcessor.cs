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
        /// Чтение данных из интернета.
        /// </summary>
        private readonly DataDownloader _downloader;

        /// <summary>
        /// Чтение модели страницы ria.
        /// </summary>
        private readonly HtmlParser _parser;

        /// <summary>
        /// Запись модели страницы ria.
        /// </summary>
        private readonly IPageSaver _saver;

        /// <summary>
        /// Процессор по получению спарсенных данных из ria и их сохранению в файл
        /// </summary>
        /// <param name="downloader"></param>
        /// <param name="parser"></param>
        /// <param name="saver"></param>
        public RiaPageProcessor(DataDownloader downloader, HtmlParser parser, IPageSaver saver)
        {
            _downloader = downloader;
            _parser = parser;
            _saver = saver;
        }

        private static readonly Regex CheckUrlRegex;

        static RiaPageProcessor()
        {
            CheckUrlRegex = new Regex("\\w{3}.\\w{2}", RegexOptions.Singleline);
        }

        /// <summary>
        /// Выполняет сбор новости.
        /// </summary>
        /// <param name="url">Ссылка на страницу новости.</param>
        /// <param name="saveDirPath">Путь к директории, куда сохраняется результат.</param>
        public void ProcessPage(string url, string saveDirPath)
        {
            //// TODO сделать проверку на валидность url.
            var urlMathes = CheckUrlRegex.Matches(url);
            var CheckUrl = urlMathes[0].Value;

            if (string.IsNullOrWhiteSpace(url) || CheckUrl != "ria.ru")
                Console.WriteLine("You entered an incorrect link"+Environment.NewLine);
            else
            {
                var htmlCode = _downloader.DownloadPageHtml(url);
                var pageModel = _parser.ParsePage(htmlCode);

                _saver.SavePageModel(pageModel, saveDirPath);
            }
        }
    }
}