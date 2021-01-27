using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text;

namespace RIA
{
    class FileName
    {
        public static string CleanString(string input, char[] invalidChars)
        {
            var builder = new StringBuilder();
            foreach (var cur in input)
            {
                if (!invalidChars.Contains(cur))
                {
                    builder.Append(cur);
                }
            }

            return builder.ToString();
        }
        public static string GetFileName(HtmlDocument htmlDoc)
        {
            int fileNameLength = LoadingData.ParseArticle(htmlDoc).Length;
            if (fileNameLength > 70)
            {
                fileNameLength = 70;
            }
            var filename = LoadingData.ParseArticle(htmlDoc).Substring(0, fileNameLength);
            return filename;
        }
    }
}
