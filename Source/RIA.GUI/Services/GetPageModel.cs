using System;
using System.Collections.Generic;
using System.Text;
using RIA.Grabber;
using RIA.Grabber.Services;

namespace RIA.GUI.Services
{
    public class GetPageModel
    {
        public void StartParse(string url, string path)
        {
            var dataDownloader = new DataDownloader();
            var htmlParser = new HtmlParser();
            var jsonPageSaver = new JsonPageSaver();
            var processor = new RiaPageProcessor(dataDownloader, htmlParser, jsonPageSaver);

            processor.ProcessPage(url, path);
        }
    }
}
