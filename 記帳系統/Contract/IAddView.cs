using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using System.Drawing;

namespace 記帳系統.Contract
{
    public interface IAddView
    {
        List<string> AccountNameList { set; }
        string SelectedAccountName { get; }
        List<string> AccountTypeList { set; }
        string SelectedAccountType { get; }
        DateTime Date { get; }
        string Detail { get; }
        string Payment { get; }
        string Amount { get; }
        Image Picture1 { get; }
        Image Picture2 { get; }
        void ShowMessage(string message);

        void ResetForm();



    }
}
