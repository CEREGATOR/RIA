using System;
using RIA.Grabber;

namespace RIA
{
    using Grabber.Services;

    class Program
    {
        public static void Main()
        {
            var dataDownloaded = new DataDownloader();
            var htmlParser = new HtmlParser();
            var jsonPageSaver = new JsonPageSaver();
            var processor = new RiaPageProcessor(dataDownloaded, htmlParser, jsonPageSaver);

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter your link to ria.ru:");
                    var url = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine("Enter the path to the directory:");
                    var path = Console.ReadLine();
                    Console.WriteLine();

                    processor.ProcessPage(url, path);

                    Console.WriteLine("Press Y to continue or N to close: ");
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
        }
    }
}
