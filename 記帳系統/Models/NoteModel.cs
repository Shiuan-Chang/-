using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class NoteModel
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Date { get; set; }
        public string Detail { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public string originalPicture1Path { get; set; }
        public string originalPicture2Path { get; set; }
        public string CompressedPicture1Path { get; set; }
        public string CompressedPicture2Path { get; set; }
    }
}
