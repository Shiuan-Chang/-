using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace 記帳系統.Models
{
    public class AddModel
    {
        public string SelectedAccountName { get; set; }
        public string SelectedAccountType { get; set; }
        public DateTime Date { get; set; }
        public string Detail { get; set; }
        public string Payment { get; set; }
        public string Amount { get; set; }
        public Image Picture1 { get; set; }
        public Image Picture2 { get; set; }

        // 預設建構子，提供一個初始的狀態
        public AddModel()
        {
            Reset();
        }

        // 重置 ViewModel 的狀態
        public void Reset()
        {
            SelectedAccountName = string.Empty;
            SelectedAccountType = string.Empty;
            Date = DateTime.Now;
            Detail = string.Empty;
            Payment = string.Empty;
            Amount = string.Empty;
            Picture1 = null;
            Picture2 = null;
        }
    }
}
