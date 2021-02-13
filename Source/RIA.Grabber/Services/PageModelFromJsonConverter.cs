using System.IO;
using System.Text.Json;
using RIA.Grabber.Model;

namespace RIA.Grabber.Services
{
    public class PageModelFromJsonConverter
    {
        public PageModel PageModelJson(string filename, string path)
        {
            var jsonString = File.ReadAllText(Path.Combine(path, filename));
            var pageModel = JsonSerializer.Deserialize<PageModel>(jsonString);

            return pageModel;
        }
    }
}
