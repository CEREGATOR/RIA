using System;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace RIA
{
    class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter your link to ria.ru:");
                var html = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter the path to the directory:");
                var path = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("The program is running, please wait");
                Console.WriteLine();

                var htmlDoc = GetHtmlFile(html);

                ParseImage(htmlDoc, path, FileName(htmlDoc));
                Console.WriteLine("Image uploaded");

                SaveJson(AddList(htmlDoc), path, FileName(htmlDoc));
                Console.WriteLine("Json file create");

                Console.WriteLine("Press Y to continue or N to close: ");
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
        }

        public static HtmlDocument GetHtmlFile(string html)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = new HtmlDocument();
            htmlDoc = web.Load(html);

            return htmlDoc;
        }
        public static string ParseArticle(HtmlDocument htmlDoc)
        {
            var article = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='article__title']");

            if (article != null)
                return article.InnerText;
            else
                return "";
        }
        public static DateTime? ParseDate(HtmlDocument htmlDoc)
        {
            var date = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");
            DateTime? Date;

            if (date != null)
            {
                Date = DateTime.ParseExact(date.InnerText.Trim(), "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            else
                Date = null;

            return Date;
        }
        public static DateTime? ParseDateUpdate(HtmlDocument htmlDoc)
        {
            var date_update = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='article__info-date-modified']");
            DateTime? DateUpdate;

            if (date_update != null)
            {
                DateUpdate = DateTime.ParseExact(date_update.InnerText.Replace('(', ' ').Replace(')', ' ').Replace("обновлено:", " ").Trim(), "HH:mm dd.MM.yyyy",CultureInfo.InvariantCulture);
            }
            else
                DateUpdate = null;

            return DateUpdate;
        }
        public static string ParseText(HtmlDocument htmlDoc)
        {
            var text = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__block'][@data-type='text']");

            string Text = "";
            var pos = -1;
            int text_amount = 0;
            if (text != null)
            {
                while (text_amount < text.Count)
                {
                    Text = Text.Insert(pos + 1, text[text_amount].InnerText + Environment.NewLine);
                    pos = Text.LastIndexOf(Environment.NewLine);
                    text_amount += 1;
                }
            }
            if (text != null)
                return Text;
            else
                return "";
        }
        public static string ParseLink(HtmlDocument htmlDoc)
        {
            var link = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__text']/a");
            string Link = "";
            var pos = -1;
            int link_amount = 0;
            if (link != null)
            {
                while (link_amount < link.Count)
                {
                    Link = Link.Insert(pos + 1, link[link_amount].Attributes["href"].Value + " " + "text: " + link[link_amount].InnerText + Environment.NewLine);
                    pos = Link.LastIndexOf(Environment.NewLine);
                    link_amount += 1;
                }
            }
            return Link;
        }
        public static string ParseImageBase64(HtmlDocument htmlDoc)
        {
            var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");
            string ImageBase64 = "";
            var pos = -1;
            var img_amount = 0;
            if (img != null)
            {
                while (img_amount < img.Count)
                {
                    ImageBase64 = ImageBase64.Insert(pos + 1, ConvertImageURLToBase64(img[img_amount].Attributes["src"].Value) + Environment.NewLine);
                    pos = ImageBase64.LastIndexOf(Environment.NewLine);
                    img_amount += 1;
                }
            }
            return ImageBase64;
        }
        public static void ParseImage(HtmlDocument htmlDoc, string path, string filename)
        {
            var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");
            int j = 0;
            if (img != null)
            {
                while (j < img.Count)
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(img[j].Attributes["src"].Value), Path.Combine(MakeValidPath(path), MakeValidFileName(filename) + j + ".jpg"));
                    }
                    j += 1;
                }
            }
        }
        public static List<ParseInfo> AddList(HtmlDocument htmlDoc)
        {
            List<ParseInfo> ParseInfo = new List<ParseInfo>();

            ParseInfo.Add(new ParseInfo()
            {
                Article = ParseArticle(htmlDoc),
                Date = ParseDate(htmlDoc),
                DateUpdate = ParseDateUpdate(htmlDoc),
                Text = ParseText(htmlDoc),
                Link = ParseLink(htmlDoc),
                ImageBase64 = ParseImageBase64(htmlDoc)
            });

            return ParseInfo;
        }
        public static string FileName(HtmlDocument htmlDoc)
        {
            int fileNameLength = ParseArticle(htmlDoc).Length;
            if (fileNameLength > 70)
            {
                fileNameLength = 70;
            }
            var filename = ParseArticle(htmlDoc).Substring(0, fileNameLength);
            return filename;
        }
        public static void SaveJson(List<ParseInfo> ParseInfo, string path, string filename)
        {
            string json = JsonConvert.SerializeObject(ParseInfo, Formatting.Indented,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            File.WriteAllText(Path.Combine(MakeValidPath(path), MakeValidFileName(filename) + ".json"), json);
        }
        public static string MakeValidFileName(string filename)
        {
            var builder = new StringBuilder();
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var cur in filename)
            {
                if (!invalid.Contains(cur))
                {
                    builder.Append(cur);
                }
            }
            return builder.ToString();
        }
        public static string MakeValidPath(string path)
        {
            var builder = new StringBuilder();
            var invalid = Path.GetInvalidPathChars();
            foreach (var cur in path)
            {
                if (!invalid.Contains(cur))
                {
                    builder.Append(cur);
                }
            }
            return builder.ToString();
        }
        public static String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }
        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;
            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }
                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }
            return (buf);
        }
    }
}
