using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class RawData
    {
        public string date { get; set; }

        public string accountName { get; set; }
        public string accountType { get; set; }
        public string detail { get; set; }
        public string paymentMethod { get; set; }
        public string amount { get; set; }

        public string csvImagePath1 { get; set; }

        public string csvImagePath2 { get; set; }
        public string compressImagePath1 { get; set; }

        public string compressImagePath2 { get; set; }

    }
}
