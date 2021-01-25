using System;
using System.Collections.Generic;
using System.Text;

namespace RIA
{
    class ParseInfo
    {
        public string Article { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateUpdate { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public string ImageBase64 { get; set; }
    }
}
