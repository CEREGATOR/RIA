namespace RIA.Grabber.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using HtmlAgilityPack;
    using HtmlAgilityPack.CssSelectors.NetCore;

    using RIA.Grabber.Model;

    /// <summary>
    /// Разбирает модель страницы риа.ру из html кода этой страницы.
    /// </summary>
    public class HtmlParser
    {
        /// <summary>
        /// Регулярное выражение для выделения дат.
        /// </summary>
        private static readonly Regex ExtractDateRegex;

        /// <summary>
        /// Инициализирует статические поля класса <see cref="HtmlParser"/>.
        /// </summary>
        static HtmlParser()
        {
            ExtractDateRegex = new Regex("\\d{2}:\\d{2}\\s\\d{2}\\.\\d{2}\\.\\d{4}", RegexOptions.Singleline);
        }

        /// <summary>
        /// Разбирает модель новости из страницы.
        /// </summary>
        /// <param name="htmlCode">HTML код страницы.</param>
        /// <returns>Модель новости.</returns>
        public PageModel ParsePage(string htmlCode)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlCode);

            var model = new PageModel();

            model.Title = GetPageArticle(htmlDoc);
            model.Text = GetPageText(htmlDoc);

            var dates = GetPageDates(htmlDoc);
            model.PublicationDate = dates.Item1;
            model.LastChangeDate = dates.Item2;

            model.LinksInText = GetInternalLinksFromText(htmlDoc);
            model.ImageLinks = GetImageLinks(htmlDoc);

            foreach (var imgUrl in model.ImageLinks)
            {
                var downloadedByteArray = DataDownloader.DownloadByteArray(imgUrl);
                model.ImagesInBase64.Add(Convert.ToBase64String(downloadedByteArray));
            }

            return model;
        }

        /// <summary>
        /// Получает заголовок новости.
        /// </summary>
        /// <param name="htmlDoc">HTML документ.</param>
        /// <returns>Заголовок новости.</returns>
        private string GetPageArticle(HtmlDocument htmlDoc)
        {
            var parentNode = htmlDoc.QuerySelectorAll(".endless__item").First();
            var articleNode = parentNode.QuerySelector("h1.article__title");

            return string.IsNullOrWhiteSpace(articleNode?.InnerText)
                ? string.Empty
                : articleNode.InnerText;
        }

        /// <summary>
        /// Получает текст новости.
        /// </summary>
        /// <param name="htmlDoc">HTML документ.</param>
        /// <returns>Текст новости.</returns>
        private string GetPageText(HtmlDocument htmlDoc)
        {
            var parentNode = htmlDoc.QuerySelectorAll(".endless__item").First();
            var textNodes = parentNode.QuerySelectorAll(".article__text");
            if (textNodes == null)
                return string.Empty;

            var paragraphs = textNodes.Select(x => x.InnerText).Where(x => !string.IsNullOrWhiteSpace(x));
            return string.Join(Environment.NewLine, paragraphs);
        }

        /// <summary>
        /// Получает текст новости.
        /// </summary>
        /// <param name="htmlDoc">HTML документ.</param>
        /// <returns>Текст новости.</returns>
        private (DateTime?, DateTime?) GetPageDates(HtmlDocument htmlDoc)
        {
            var datesNode = htmlDoc.QuerySelector(".article__info-date");
            var datesRawText = datesNode?.InnerText;
            if (string.IsNullOrWhiteSpace(datesRawText))
                return (null, null);

            var dateMatches = ExtractDateRegex.Matches(datesRawText);
            return (ParseDateTimeRiaFormat(dateMatches[0].Value), dateMatches.Count == 2 ? ParseDateTimeRiaFormat(dateMatches[1].Value) : null);
        }

        /// <summary>
        /// Получает список ссылок из текста новости.
        /// </summary>
        /// <param name="htmlDoc">HTML документ.</param>
        /// <returns>Список ссылок из текста новости.</returns>
        private List<LinkWithDescription> GetInternalLinksFromText(HtmlDocument htmlDoc)
        {
            var parentNode = htmlDoc.QuerySelectorAll(".endless__item").First();
            var linkNodes = parentNode.QuerySelectorAll(".article__text a");

            return linkNodes
                .Select(x => new LinkWithDescription { Url = x.Attributes["href"]?.Value, Description = x.InnerText })
                .Where(x => !string.IsNullOrEmpty(x.Url))
                .ToList();
        }

        /// <summary>
        /// Получает список ссылок на изображения из новости.
        /// </summary>
        /// <param name="htmlDoc">HTML документ.</param>
        /// <returns>Список ссылок на изображения из новости.</returns>
        private List<string> GetImageLinks(HtmlDocument htmlDoc)
        {
            var parentNode = htmlDoc.QuerySelectorAll(".endless__item").First();
            var imageNodes = parentNode.QuerySelectorAll(".media img");

            return imageNodes == null
                ? new List<string>()
                : imageNodes.Select(t => t.Attributes?["src"]?.Value).Where(link => !string.IsNullOrEmpty(link)).ToList();
        }

        /// <summary>
        /// Разбирает дату из <paramref name="input"/> в формате, стандартном для ria.ru.
        /// </summary>
        /// <param name="input">Входная строка с датой.</param>
        /// <returns>Разобранную дату.</returns>
        private DateTime? ParseDateTimeRiaFormat(string input)
        {
            if (input != null)
                return DateTime.ParseExact(input, "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            else
                return null;
        }
    }
}