using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using RIA.Grabber;
using RIA.Grabber.Model;
using RIA.Grabber.Services;

namespace RIA.GUI.Services
{
    public class GetPageModelFromJson
    {
        public PageModel PageModelJson(string filename, string path)
        {
            var jsonString = File.ReadAllText(Path.Combine(path, filename));
            var pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

            return pageModel;
        }
    }
}
