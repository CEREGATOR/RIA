using System;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
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
                var Page = new List<ParseInfo>();

                Console.WriteLine("Enter your link to ria.ru:");
                var html = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter the path to the directory:");
                var path = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("The program is running, please wait");
                Console.WriteLine();

                HtmlWeb web = new HtmlWeb();
                var htmlDoc = new HtmlDocument();
                htmlDoc = web.Load(html);

                var article = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='article__title']");
                int fileNameLength = article.InnerText.Length;
                if (fileNameLength > 70)
                {
                    fileNameLength = 70;
                }

                var date = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");
                var date_update = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='article__info-date-modified']");
                var text = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__block'][@data-type='text']");

                string TEXT = "";
                var pos = -1;
                int text_amount = 0;
                if (text != null)
                {
                    while (text_amount < text.Count)
                    {
                        TEXT = TEXT.Insert(pos + 1, text[text_amount].InnerText + Environment.NewLine);
                        pos = TEXT.LastIndexOf(Environment.NewLine);
                        text_amount += 1;
                    }
                }

                var link = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__text']/a");
                string LINK = "";
                pos = -1;
                int link_amount = 0;
                if (link != null)
                {
                    while (link_amount < link.Count)
                    {
                        LINK = LINK.Insert(pos + 1, link[link_amount].Attributes["href"].Value + " " + "text: " + link[link_amount].InnerText + Environment.NewLine);
                        pos = LINK.LastIndexOf(Environment.NewLine);
                        link_amount += 1;
                    }
                }

                var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");
                string image64 = "";
                pos = -1;
                var img_amount = 0;
                if (img != null)
                {
                    while (img_amount < img.Count)
                    {
                        image64 = image64.Insert(pos + 1, ConvertImageURLToBase64(img[img_amount].Attributes["src"].Value) + Environment.NewLine);
                        pos = TEXT.LastIndexOf(Environment.NewLine);
                        img_amount += 1;
                    }
                }

                if ((article != null) && (date != null) && (date_update != null))
                {
                    Page.Add(new ParseInfo()
                    {
                        Article = article.InnerText.Trim(),
                        Date = DateTime.ParseExact(date.InnerText, "HH:mm dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture),
                        DateUpdate = DateTime.ParseExact(date_update.InnerText.Replace('(', ' ').Replace(')', ' ').Replace("обновлено:", " ").Trim(), "HH:mm dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture),
                        Text = TEXT,
                        Link = LINK,
                        ImageBase64 = image64
                    });
                }

                var filename = article.InnerText.Substring(0, fileNameLength);
                filename = MakeValidFileName(filename);
                path = MakeValidPath(path);

                int j = 0;
                if (img != null)
                {
                    while (j < img.Count)
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(img[j].Attributes["src"].Value), Path.Combine(path, filename + j + ".jpg"));
                        }
                        j += 1;
                    }
                }

                Console.WriteLine("Image uploaded");

                string json = JsonConvert.SerializeObject(Page, Formatting.Indented,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                File.WriteAllText(Path.Combine(path, filename + ".json"), json);

                Console.WriteLine("Json file create");

                Console.WriteLine(Environment.NewLine + "Press Y to continue or N to close" + Environment.NewLine);
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
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
