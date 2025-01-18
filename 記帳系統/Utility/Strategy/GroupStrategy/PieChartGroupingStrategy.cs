using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using 記帳系統.Utility.Strategy.Interface;

namespace 記帳系統.Utility.Strategy
{
    public class PieChartGroupingStrategy : IGroupingStrategy
    {
        public List<AnalysisModel> GroupData(List<AnalysisRawDataDAO> rawData, List<string> conditionTypes, List<string> analyzeTypes)
        {
            // 過濾數據，根據 conditionTypes
            var filteredData = conditionTypes.Any()
                ? rawData.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
                : rawData;

            if (analyzeTypes.Any())
            {
                // 根據 analyzeTypes 分組
                return filteredData
                    .Where(item => DateTime.TryParse(item.Date, out _)) // 確保有有效的日期
                    .GroupBy(item =>
                    {
                        // 確定分組的主鍵
                        DateTime.TryParse(item.Date, out var parsedDate);
                        string month = parsedDate.ToString("yyyy-MM");

                        string key = analyzeTypes.Contains("帳目類型") ? item.AccountType :
                                     analyzeTypes.Contains("用途") ? item.Detail :
                                     analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null;

                        return new { Month = month, Key = key }; // 按月份和 analyzeType 分組
                    })
                    .Where(group => group.Key.Key != null) // 排除空鍵的分組
                    .Select(group => new AnalysisModel
                    {
                        Date = group.Key.Month,
                        AccountType = analyzeTypes.Contains("帳目類型") ? group.Key.Key : null,
                        Detail = analyzeTypes.Contains("用途") ? group.Key.Key : null,
                        PaymentMethod = analyzeTypes.Contains("支付方式") ? group.Key.Key : null,
                        Amount = group.Sum(x => long.TryParse(x.Amount, out var amt) ? amt : 0).ToString()
                    })
                    .ToList();
            }
            else
            {
                // 如果沒有 analyzeTypes，返回原始數據的對應模型
                return filteredData.Select(item => new AnalysisModel
                {
                    AccountType = item.AccountType,
                    Detail = item.Detail,
                    PaymentMethod = item.PaymentMethod,
                    Amount = item.Amount
                }).ToList();
            }
        }
    }
}
