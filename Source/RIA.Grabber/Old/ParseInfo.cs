namespace RIA.Grabber.Old
{
    using System;
    using System.Collections.Generic;

    public class ParseInfo
    {
        /// <summary>
        /// Получает или задает заголовок новости.
        /// </summary>
        public string Article { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public List<string> ImageBase64 { get; set; }
    }
}
