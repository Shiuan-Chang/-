using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    //這邊的model受到CSV表頭判讀的影響，必須和Note設定一致(也就是和CSV設定判讀的方式一致)，否則容易重複生成表頭
    public class AccountModel
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Date { get; set; }
        public string Detail { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public string Picture1Path { get; set; }
        public string Picture2Path { get; set; }
        public string CompressedPicture1Path { get; set; }
        public string CompressedPicture2Path { get; set; }
    }
}
