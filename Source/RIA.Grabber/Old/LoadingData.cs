namespace RIA.Grabber.Old
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    using HtmlAgilityPack;

    public class LoadingData
    {
        public static String ParseArticle(HtmlDocument htmlDoc)
        {
            var article = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='article__title']");

            if ((article != null) && (string.IsNullOrEmpty(article.InnerText) != true) && (string.IsNullOrWhiteSpace(article.InnerText) != true))
                return article.InnerText;
            else
                return String.Empty;
        }
        public static DateTime? ParseDate(HtmlDocument htmlDoc)
        {
            var date_html = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");
            DateTime? Date;
            var reg = new Regex("\\d{2}:\\d{2}\\s\\d{2}\\.\\d{2}\\.\\d{4}", RegexOptions.Singleline);
            var mathes = reg.Matches(date_html.InnerText);

            if ((date_html != null) && ((string.IsNullOrEmpty(date_html.InnerText) != true) && (string.IsNullOrWhiteSpace(date_html.InnerText) != true)))
            {
                Date = DateTime.ParseExact(mathes[0].Value, "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            else
                Date = null;

            return Date;
        }
        public static DateTime? ParseDateUpdate(HtmlDocument htmlDoc)
        {
            var dateupdate_html = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='article__info-date-modified']");
            DateTime? DateUpdate;

            var reg = new Regex("\\d{2}:\\d{2}\\s\\d{2}\\.\\d{2}\\.\\d{4}", RegexOptions.Singleline);
            var mathes = reg.Matches(dateupdate_html.InnerText);

            if ((dateupdate_html != null) && ((string.IsNullOrEmpty(dateupdate_html.InnerText) != true) && (string.IsNullOrWhiteSpace(dateupdate_html.InnerText) != true)))
            {
                DateUpdate = DateTime.ParseExact(mathes[0].Value, "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            else
                DateUpdate = null;

            return DateUpdate;
        }
        public static String ParseText(HtmlDocument htmlDoc)
        {
            var text = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__block'][@data-type='text']");
            StringBuilder Text = new StringBuilder();
            if (text != null)
            {
                for (int text_amount = 0; text_amount < text.Count; text_amount++)
                {
                    if ((string.IsNullOrEmpty(text[text_amount].InnerText) != true) && (string.IsNullOrWhiteSpace(text[text_amount].InnerText) != true))
                    {
                        Text.AppendJoin(Environment.NewLine, text[text_amount].InnerText);
                    }
                }
            }
            if (text != null)
                return Text.ToString();
            else
                return String.Empty;
        }
        public static String ParseLink(HtmlDocument htmlDoc)
        {
            var link = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__text']/a");
            StringBuilder Link = new StringBuilder();
            if (link != null)
            {
                for (int link_amount = 0; link_amount < link.Count; link_amount++)
                {
                    if ((string.IsNullOrEmpty(link[link_amount].Attributes["href"].Value) != true) && (string.IsNullOrWhiteSpace(link[link_amount].Attributes["href"].Value) != true))
                    {
                        Link.AppendJoin(Environment.NewLine, link[link_amount].Attributes["href"].Value + " " + "text: " + link[link_amount].InnerText + Environment.NewLine);
                    }
                }
            }
            if (link != null)
                return Link.ToString();
            else
                return String.Empty;
        }
        public static List<string> ParseImageLink(HtmlDocument htmlDoc)
        {
            var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");
            List<string> LinkList = new List<string>();

            if (img != null)
            {
                for (int img_amount = 0; img_amount < img.Count; img_amount++)
                {
                    if ((string.IsNullOrEmpty(img[img_amount].Attributes["src"].Value) != true) && (string.IsNullOrWhiteSpace(img[img_amount].Attributes["src"].Value) != true))
                    {
                        LinkList.Add(img[img_amount].Attributes["src"].Value);
                    }
                }
            }

            return LinkList;
        }
    }
}
