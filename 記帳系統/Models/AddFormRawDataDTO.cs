using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class AddFormRawDataDTO
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public DateTime Date { get; set; }
        public string Detail { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public Image Picture1 { get; set; }
        public Image Picture2 { get; set; }
        public Image CompressedPicture1 { get; set; }
        public Image CompressedPicture2 { get; set; }
        public string Picture1Path { get; set; }
        public string Picture2Path { get; set; }
        public string CompressedPicture1Path { get; set; }
        public string CompressedPicture2Path { get; set; }

    }
}
