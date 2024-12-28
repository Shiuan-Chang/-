using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;

namespace 記帳系統.Contract
{
    public interface IAccountPresenter
    {
        //拿到原始資料
        void LoadData(DateTime startDate, DateTime endDate, List<string> conditionTypes, List<string> analyzeTypes);


        //拿到原始資料並回傳groupby後的結果
        List<AccountingModel> RespondGroupList(List<AccountingModel> lists);


    }
}
