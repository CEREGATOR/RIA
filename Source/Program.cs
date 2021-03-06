﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using HtmlAgilityPack;

namespace RIA
{
    class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter your link to ria.ru:");
                var url = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter the path to the directory:");
                var path = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("The program is running, please wait");
                Console.WriteLine();

                var htmlDoc = new HtmlDocument();
                var request = (HttpWebRequest)WebRequest.Create(url);

                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                            throw new Exception("что-то пошло не так");
                        else
                            htmlDoc.Load(responseStream);
                    }
                }

                DownloadImage(ParseImageLink(htmlDoc), path, GetFileName(htmlDoc));
                Console.WriteLine("Image uploaded");

                SaveJsonFile(htmlDoc, path, GetFileName(htmlDoc));
                Console.WriteLine("Json file create");
                Console.WriteLine();

                Console.WriteLine("Press Y to continue or N to close: ");
                Console.WriteLine();
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
        }
        //public static Stream GetHtmlFile(string requestUriString)
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(requestUriString);

        //    using (var response = request.GetResponse())
        //    {
        //        using (var responseStream = response.GetResponseStream())
        //        {
        //            if (responseStream == null)
        //                throw new Exception("что-то пошло не так");
        //            else
        //                return responseStream;
        //        }
        //    }
        //}
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
            var reg = new Regex ( "\\d{2}:\\d{2}\\s\\d{2}\\.\\d{2}\\.\\d{4}",RegexOptions.Singleline);
            var mathes = reg.Matches(date_html.InnerText);

            if ((date_html != null)&&((string.IsNullOrEmpty(date_html.InnerText) != true) && (string.IsNullOrWhiteSpace(date_html.InnerText) != true)))
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
                DateUpdate = DateTime.ParseExact(mathes[0].Value,"HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
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
        public static void DownloadImage(List<string> LinkList, string path, string filename)
        {
            int img_amount = 0;
            while (img_amount < LinkList.Count)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(LinkList[img_amount]), Path.Combine(CleanString(path, Path.GetInvalidPathChars()),
                        CleanString(filename, Path.GetInvalidFileNameChars()) + img_amount + ".jpg"));
                }
                img_amount += 1;
            }
        }
        public static List<string> ProcessDocumentImages(List<string> LinkList)
        {
            List<string> ImageBase64 = new List<string>();
            int img_amount = 0;
            while (img_amount < LinkList.Count)
            {
                ImageBase64.Add(DownloadImageBase64(LinkList[img_amount]));
                img_amount += 1;
            }
            return ImageBase64;
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
        public static string GetFileName(HtmlDocument htmlDoc)
        {
            int fileNameLength = ParseArticle(htmlDoc).Length;
            if (fileNameLength > 70)
            {
                fileNameLength = 70;
            }
            var filename = ParseArticle(htmlDoc).Substring(0, fileNameLength);
            return filename;
        }
        public static void SaveJsonFile(HtmlDocument htmlDoc, string path, string filename)
        {
            ParseInfo parseInfo =
            new ParseInfo()
            {
                Article = ParseArticle(htmlDoc),
                Date = ParseDate(htmlDoc),
                DateUpdate = ParseDateUpdate(htmlDoc),
                Text = ParseText(htmlDoc),
                Link = ParseLink(htmlDoc),
                ImageBase64 = ProcessDocumentImages(ParseImageLink(htmlDoc))
            };
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(parseInfo, options);
            File.WriteAllText(Path.Combine(CleanString(path, Path.GetInvalidPathChars()), CleanString(filename, Path.GetInvalidFileNameChars()) + ".json"), json);
        }
        public static string CleanString(string input, char[] invalidChars)
        {
            var builder = new StringBuilder();
            foreach (var cur in input)
            {
                if (!invalidChars.Contains(cur))
                {
                    builder.Append(cur);
                }
            }

            return builder.ToString();
        }
        public static String DownloadImageBase64(String url)
        {
            byte[] bytes = GetImage(url);
            return Convert.ToBase64String(bytes);
        }
        private static byte[] GetImage(string url)
        {
            byte[] buf;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                using (var response = req.GetResponse())
                using (var stream = response.GetResponseStream())
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to collect image from link {url}", ex);
            }

            return buf;
        }
    }
}
