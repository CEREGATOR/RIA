using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Net.Http;
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

            if (article != null)
                return article.InnerText;
            else
                return String.Empty;
        }
        public static DateTime? ParseDate(HtmlDocument htmlDoc)
        {
            var date_html = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");
            DateTime? Date;

            if (date_html != null)
            {
                Date = DateTime.ParseExact(date_html.InnerText.Trim(), "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            else
                Date = null;

            return Date;
        }
        public static DateTime? ParseDateUpdate(HtmlDocument htmlDoc)
        {
            var dateupdate_html = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='article__info-date-modified']");
            DateTime? DateUpdate;

            string pattern = @"\W\b(обновлено)\b\W\s";

            if (dateupdate_html != null)
            {
                DateUpdate = DateTime.ParseExact(Regex.Replace(dateupdate_html.InnerText, pattern, string.Empty).Replace(')', ' ').Trim(),
                    "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            else
                DateUpdate = null;

            return DateUpdate;
        }
        public static String ParseText(HtmlDocument htmlDoc)
        {
            var text = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__block'][@data-type='text']");

            StringBuilder Text = new StringBuilder();
            int text_amount = 0;
            if (text != null)
            {
                while (text_amount < text.Count)
                {
                    Text.AppendJoin(Environment.NewLine, text[text_amount].InnerText);
                    text_amount += 1;
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
            int link_amount = 0;
            if (link != null)
            {
                while (link_amount < link.Count)
                {
                    Link.AppendJoin(Environment.NewLine, link[link_amount].Attributes["href"].Value + " " + "text: " + link[link_amount].InnerText + Environment.NewLine);
                    link_amount += 1;
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
        public static String ProcessDocumentImages(List<string> LinkList)
        {
            StringBuilder ImageBase64 = new StringBuilder();
            int img_amount = 0;
            while (img_amount < LinkList.Count)
            {
                ImageBase64.AppendJoin(Environment.NewLine, DownloadImageBase64(LinkList[img_amount]));
                img_amount += 1;
            }
            return ImageBase64.ToString();
        }
        public static List<string> ParseImageLink(HtmlDocument htmlDoc)
        {
            var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");
            var img_amount = 0;

            List<string> LinkList = new List<string>();
            if (img != null)
            {
                while (img_amount < img.Count)
                {
                    LinkList.Add(img[img_amount].Attributes["src"].Value);
                    img_amount += 1;
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
