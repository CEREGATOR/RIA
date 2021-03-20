using System.IO;

namespace RIA.Grabber.Services
{
    public class ResultDirectoryWatcher
    {
        public string[] ListJsonFilesUpdate(string path)
        {
            var dir = new DirectoryInfo(path);

            var files = dir.GetFiles("*.json");

            var fileName = new string[files.Length];
            var i = 0;

            foreach (var fi in files)
            {
                fileName[i] = fi.Name;
                i++;
            }

            return fileName;
        }
    }
}
