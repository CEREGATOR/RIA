using System.IO;

namespace RIA.Grabber.Services
{
    public class ResultDirectoryWatcher
    {
        public string[] ListJsonFilesUpdate(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] files = dir.GetFiles("*.json");

            string[] fileName = new string[files.Length];
            int i = 0;

            foreach (FileInfo fi in files)
            {
                fileName[i] = fi.Name;
                i++;
            }

            return fileName;
        }
    }
}
