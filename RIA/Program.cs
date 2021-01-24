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
            var Page = new List<Variables.ParseInfo>();

            //Console.WriteLine("Enter your link to ria.ru");

            var path = @"D:\C#";

            var html = @"https://ria.ru/20201103/ris-1582804514.html"; //Console.ReadLine();

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
            while (i<text.Count)
            {
                TEXT = TEXT.Insert(pos+1, text[i].InnerText + "\n");
                pos = TEXT.LastIndexOf("\n");
                i += 1;
            }

            Console.WriteLine(TEXT);

            Page.Add(new Variables.ParseInfo()
            {
                Article = article.InnerText,
                Date = date.InnerText,
                Date_update = date_update.InnerText,
                Text = TEXT
            });

            var img = htmlDoc.DocumentNode.SelectNodes("//img[@media-type='ar16x9a']");

            int j = 0;
            while (j < img.Count)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(img[j].Attributes["src"].Value), path+"/"+ article.InnerText.Substring(0,l)+".jpg");
                }
                j += 1;
            }

            string json = JsonConvert.SerializeObject(Page, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            File.WriteAllText(path + "/" + article.InnerText.Substring(0, l) + ".json", json);

            Console.ReadKey();
        }

    }

}
