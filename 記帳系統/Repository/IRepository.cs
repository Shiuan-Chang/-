using System;
using System.Collections.Generic;
using 記帳系統.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Repository
{
    public interface IRepository
    {
        // 只留下新增、修改、刪除、群組資料

        bool AddData(AddFormRawDataDAO dao);


        bool ModifyData(RawData data);
        bool RemoveData(string date);

        List<AnalysisRawDataDAO> AnalysisGetDatasByDate(DateTime start, DateTime end);
        List<NotFormRawDataDAO> GetDatasByDate(DateTime start, DateTime end);
        List<AccountRawDataDAO> accountFormGetDatasByDate(DateTime start, DateTime end);

        List<AnalysisRawDataDAO> GetChartDatas(DateTime start, DateTime end);
    }
}
