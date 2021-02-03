namespace RIA.Grabber.Old
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    public class Image
    {
        public static void DownloadImage(List<string> LinkList, string path, string filename)
        {
            int img_amount = 0;
            while (img_amount < LinkList.Count)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(LinkList[img_amount]), Path.Combine(FileName.CleanString(path, Path.GetInvalidPathChars()),
                        FileName.CleanString(filename, Path.GetInvalidFileNameChars()) + img_amount + ".jpg"));
                }
                img_amount += 1;
            }
        }
        public static List<string> ProcessDocumentImages(List<string> LinkList)
        {
            List<string> ImageBase64 = new List<string>();
            int img_amount = 0;
            while (img_amount < LinkList.Count)
            {
                ImageBase64.Add(DownloadImageBase64(LinkList[img_amount]));
                img_amount += 1;
            }
            return ImageBase64;
        }
        public static String DownloadImageBase64(String url)
        {
            byte[] bytes = GetImage(url);
            return Convert.ToBase64String(bytes);
        }
        private static byte[] GetImage(string url)
        {
            byte[] buf;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                using (var response = req.GetResponse())
                using (var stream = response.GetResponseStream())
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to collect image from link {url}", ex);
            }

            return buf;
        }
    }
}
