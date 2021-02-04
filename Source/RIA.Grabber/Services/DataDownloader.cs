namespace RIA.Grabber.Services
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Загружает данные из интернета.
    /// </summary>
    public class DataDownloader
    {
        /// <summary>
        /// Загружает HTML код страницы по заданному адресу.
        /// </summary>
        /// <param name="url">Адрес страницы.</param>
        /// <returns>HTML код страницы.</returns>
        public string DownloadPageHtml(string url) => ProcessGetRequest(url, ReadStreamAsString);

        /// <summary>
        /// Загружает файл по заданному адресу.
        /// </summary>
        /// <param name="url">Адрес файла.</param>
        /// <returns>Массив байт, в который записан файл.</returns>
        public static byte[] DownloadByteArray(string url) => ProcessGetRequest(url, ReadStreamAsByteArray);

        /// <summary>
        /// Выполняет HTTP GET запрос.
        /// </summary>
        /// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
        /// <param name="url">Адрес.</param>
        /// <param name="read">Делегат, читающий ответ по запросу.</param>
        /// <returns>Считанный результат по запросу.</returns>
        private static TResult ProcessGetRequest<TResult>(string url, Func<Stream, TResult> read)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        throw new Exception("Something went wrong!");

                    return read(responseStream);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Читает строку из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Содержимое потока в виде строки.</returns>
        private string ReadStreamAsString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        /// <summary>
        /// Читает массив байт из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Содержимое потока в виде массива байт.</returns>
        private static byte[] ReadStreamAsByteArray(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}