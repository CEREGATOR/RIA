using System;
using RIA.Grabber;

namespace RIA
{
    using RIA.Grabber.Old;
    using RIA.Grabber.Services;

    class Program
    {
        public static void Main()
        {
            var dataDownloader = new DataDownloader();
            var htmlParser = new HtmlParser();
            var jsonPageSaver = new JsonPageSaver();
            var processor = new RiaPageProcessor(dataDownloader, htmlParser, jsonPageSaver);

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

                //var htmlDoc = ParseHtml.ParseHtmlFile(url);
                //Image.DownloadImage(LoadingData.ParseImageLink(htmlDoc), path, FileName.GetFileName(htmlDoc));
                //SaveJson.SaveJsonFile(htmlDoc, path, FileName.GetFileName(htmlDoc));

                processor.ProcessPage(url, path);

                Console.WriteLine("Json file create");
                Console.WriteLine();

                Console.WriteLine("Press Y to continue or N to close: ");
                Console.WriteLine();
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
        }
    }
}
