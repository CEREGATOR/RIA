﻿namespace RIA.Grabber
{
    using System;

    using RIA.Grabber.Services;

    /// <summary>
    /// Обеспечивает сбор новости с риа ру.
    /// </summary>
    public class RiaPageProcessor
    {
        /// <summary>
        /// мне лень писать.
        /// </summary>
        private readonly DataDownloader _downloader;

        /// <summary>
        /// мне лень писать.
        /// </summary>
        private readonly HtmlParser _parser;

        /// <summary>
        /// мне лень писать.
        /// </summary>
        private readonly IPageSaver _saver;

        /// <summary>
        /// мне лень писать.
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

        /// <summary>
        /// Выполняет сбор новости.
        /// </summary>
        /// <param name="url">Ссылка на страницу новости.</param>
        /// <param name="saveDirPath">Путь к директории, куда сохраняется результат.</param>
        public void ProcessPage(string url, string saveDirPath)
        {
            //// TODO сделать проверку на валидность url.

            var htmlCode = _downloader.DownloadPageHtml(url);
            var pageModel = _parser.ParsePage(htmlCode);

            foreach (var imgUrl in pageModel.ImageLinks)
            {
                var downloadedByteArray = _downloader.DownloadByteArray(imgUrl);
                pageModel.ImagesInBase64.Add(Convert.ToBase64String(downloadedByteArray));
            }

            _saver.SavePageModel(pageModel, saveDirPath);
        }
    }
}