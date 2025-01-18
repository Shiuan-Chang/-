using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using 記帳系統.Utility.Strategy.Interface;

namespace 記帳系統.Utility.Strategy
{
    public class LineChartGroupingStrategy : IGroupingStrategy
    {
        public List<AnalysisModel> GroupData(List<AnalysisRawDataDAO> rawData, List<string> conditionTypes, List<string> analyzeTypes)
        {
            // 根據 conditionTypes 過濾數據
            var filteredData = conditionTypes.Count > 0
                ? rawData.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
                : rawData;

            // 是否進入分析模式
            bool isAnalysisMode = analyzeTypes.Count > 0;

            if (isAnalysisMode)
            {
                return filteredData
                    .Where(item => DateTime.TryParse(item.Date, out _)) // 確保日期有效
                    .GroupBy(item =>
                    {
                        DateTime.TryParse(item.Date, out var parsedDate);
                        string month = parsedDate.ToString("yyyy-MM");

                        string key = analyzeTypes.Contains("帳目類型") ? item.AccountType :
                                     analyzeTypes.Contains("用途") ? item.Detail :
                                     analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null;

                        return new { Month = month, Key = key }; // 分組鍵包括月份和分析類型
                    })
                    .Where(group => group.Key.Key != null) // 排除空鍵
                    .Select(group => new AnalysisModel
                    {
                        Date = group.Key.Month,
                        AccountType = analyzeTypes.Contains("帳目類型") ? group.Key.Key : null,
                        Detail = analyzeTypes.Contains("用途") ? group.Key.Key : null,
                        PaymentMethod = analyzeTypes.Contains("支付方式") ? group.Key.Key : null,
                        Amount = group.Sum(x => decimal.TryParse(x.Amount, out var amt) ? amt : 0).ToString()
                    })
                    .ToList();
            }
            else
            {
                return filteredData.Select(item => new AnalysisModel
                {
                    AccountType = item.AccountType,
                    Detail = item.Detail,
                    PaymentMethod = item.PaymentMethod,
                    Amount = item.Amount,
                    Date = item.Date
                }).ToList();
            }
        }
    }
}
