using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;

namespace 記帳系統.Contract
{
    public interface IAnalysisPresenter
    {
        //拿到原始資料
        void LoadData(DateTime startDate, DateTime endDate);

    }
}
