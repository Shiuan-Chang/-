using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace 記帳系統.Models
{
    public class AccountingModel
    {
        //string combinedData = date + "," + accountName + "," + accountType + "," + detail + "," + paymentMethod + "," + amount + "," + csvImagePath;

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



        public AccountingModel() { }
        //public parameterless constructor 公共無參數構造函數

        public AccountingModel(string date, string accountName, string accountType, string detail, string paymentMethod, string amount, string compressImagePath1, string compressImagePath2, string csvImagePath1, string csvImagePath2)
        {
            this.date = date;
            this.accountName = accountName;
            this.accountType = accountType;
            this.detail = detail;
            this.paymentMethod = paymentMethod;
            this.amount = amount;
            this.compressImagePath1 = compressImagePath1;
            this.compressImagePath2 = compressImagePath2;
            this.csvImagePath1 = csvImagePath1;
            this.csvImagePath2 = csvImagePath2;

        }
    }
}
