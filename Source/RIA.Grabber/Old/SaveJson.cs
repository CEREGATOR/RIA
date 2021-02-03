namespace RIA.Grabber.Old
{
    using System.IO;
    using System.Text.Encodings.Web;
    using System.Text.Json;

    using HtmlAgilityPack;

    public class SaveJson
    {
        public static void SaveJsonFile(HtmlDocument htmlDoc, string path, string filename)
        {
            ParseInfo parseInfo =
            new ParseInfo()
            {
                Article = LoadingData.ParseArticle(htmlDoc),
                Date = LoadingData.ParseDate(htmlDoc),
                DateUpdate = LoadingData.ParseDateUpdate(htmlDoc),
                Text = LoadingData.ParseText(htmlDoc),
                Link = LoadingData.ParseLink(htmlDoc),
                ImageBase64 =  Image.ProcessDocumentImages(LoadingData.ParseImageLink(htmlDoc))
            };

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(parseInfo, options);
            File.WriteAllText(Path.Combine(FileName.CleanString(path, Path.GetInvalidPathChars()), FileName.CleanString(filename, Path.GetInvalidFileNameChars()) + ".json"), json);
        }
    }
}
