using HtmlAgilityPack;
using System;
using System.Net;

namespace RIA
{
    class ParseHtml
    {
        public static HtmlDocument ParseHtmlFile(string url)
        {
            var htmlDoc = new HtmlDocument();
            var request = (HttpWebRequest)WebRequest.Create(url);

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        throw new Exception("Something went wrong!");
                    else
                        htmlDoc.Load(responseStream);
                }
            }
            return htmlDoc;
        }
    }
}
