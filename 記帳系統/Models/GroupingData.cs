using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class GroupingData
    {
        public string AccountType { get; set; }    
        public string Detail { get; set; }         
        public string PaymentMethod { get; set; }  
        public string Amount { get; set; }         

        public GroupingData()
        {
            AccountType = "";
            Detail = "";
            PaymentMethod = "";
            Amount = "0";
        }
    }
}
