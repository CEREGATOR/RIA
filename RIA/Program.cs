using System;
using System.Xml;
using System.Linq;
using System.Net;
using System.Drawing;
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
        Repeat:

            var Page = new List<Variables.ParseInfo>();

            //Console.WriteLine("Enter your link to ria.ru");

            var html = @"https://ria.ru/20210124/sirota-1594173848.html"/*Console.ReadLine()*/;

            //Console.WriteLine("Enter the path to the directory");

            var path = @"D:\C#"/*Console.ReadLine()*/;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var article = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='article__title']");

            int l = article.InnerText.Length;

            if(l>70)
            {
                l = 70;
            }

            Console.WriteLine(article.InnerText);

            var date = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");

            Console.WriteLine(date.InnerText);

            var date_update = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='article__info-date-modified']");

            Console.WriteLine(date_update.InnerText); 

            var text = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__block'][@data-type='text']");

            string TEXT="";
            var pos = -1;
            int i = 0;
            if (text != null)
            {
                while (i < text.Count)
                {
                    TEXT = TEXT.Insert(pos + 1, text[i].InnerText + " \n");
                    pos = TEXT.LastIndexOf("\n");
                    i += 1;
                }
            }
            Console.WriteLine(TEXT);

            var link = htmlDoc.DocumentNode.SelectNodes("//div[@class='article__text']/a");

            string LINK = "";
            pos = -1;
            int k = 0;
            if (link != null)
            {
                while (k < link.Count)
                {
                    LINK = LINK.Insert(pos + 1, link[k].Attributes["href"].Value + " " + "text: " + link[k].InnerText + " \n");
                    pos = LINK.LastIndexOf("\n");
                    k += 1;
                }
            }
            Console.WriteLine(LINK);

            Page.Add(new Variables.ParseInfo()
            {
                Article = article.InnerText.Trim(),
                Date = date.InnerText,
                Date_update = date_update.InnerText,
                Text = TEXT,
                Link = LINK
            }) ;

            var img = htmlDoc.DocumentNode.SelectNodes("//div[@class='media']//img");

            //var c = 0;
            //if (img != null)
            //{
            //    while (c < img.Count)
            //{
            //    Console.WriteLine(img[c].OuterHtml);
            //    c += 1;
            //}
            //}

            var filename = article.InnerText.Substring(0, l).Replace('"', ' ').Trim();

            int j = 0;
            if(img != null)
            {
                while (j < img.Count)
                {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(img[j].Attributes["src"].Value), path+"/"+ filename + j +".jpg");
                }
                j += 1;
                }
            }
            
            Console.WriteLine("Image uploaded");

            string json = JsonConvert.SerializeObject(Page, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            File.WriteAllText(path + "/" + filename + ".json", json);

            Console.WriteLine("Json file create");

            Console.WriteLine("\nDial Y to repeat, N to exit");
            string rep = Console.ReadLine();
            if (rep == "Y") goto Repeat;
            if (rep == "N") return;
        }
    }

}
