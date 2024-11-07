using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;

namespace 記帳系統.Contract
{
    internal interface IAddPresenter
    {
        //儲存原始資料
        void SaveData(AddModel model);
    }
}
