using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using 記帳系統.Utility.Strategy.Interface;

namespace 記帳系統.Utility.Strategy
{
    public class StackedChartGroupingStrategy : IGroupingStrategy
    {
        public List<AnalysisModel> GroupData(List<AnalysisRawDataDAO> rawData, List<string> conditionTypes, List<string> analyzeTypes)
        {
            var filteredData = conditionTypes.Any()
    ? rawData.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
    : rawData;

            if (analyzeTypes.Any())
            {
                return filteredData
                    .Where(item => DateTime.TryParse(item.Date, out _)) // 確保日期有效
                    .GroupBy(item =>
                    {
                        // 確定分組的鍵（動態選擇）
                        DateTime.TryParse(item.Date, out var parsedDate);
                        string month = parsedDate.ToString("yyyy-MM");

                        string key = analyzeTypes.Contains("帳目類型") ? item.AccountType :
                                     analyzeTypes.Contains("用途") ? item.Detail :
                                     analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null;

                        return new { Month = month, Category = key }; // 分組鍵包含月份和分類
                    })
                    .Where(group => group.Key.Category != null) // 過濾掉分類為空的分組
                    .Select(group => new AnalysisModel
                    {
                        Date = group.Key.Month,
                        AccountType = analyzeTypes.Contains("帳目類型") ? group.Key.Category : null,
                        Detail = analyzeTypes.Contains("用途") ? group.Key.Category : null,
                        PaymentMethod = analyzeTypes.Contains("支付方式") ? group.Key.Category : null,
                        Amount = group.Sum(x => long.TryParse(x.Amount, out var amt) ? amt : 0).ToString()
                    })
                    .ToList();
            }
            else if (conditionTypes.Any())
            {
                // 如果只有 conditionTypes，按 AccountType 分組
                return filteredData
                    .Where(item => DateTime.TryParse(item.Date, out _))
                    .GroupBy(item =>
                    {
                        DateTime.TryParse(item.Date, out var parsedDate);
                        string month = parsedDate.ToString("yyyy-MM");
                        return new { Month = month, Category = item.AccountType };
                    })
                    .Where(group => group.Key.Category != null)
                    .Select(group => new AnalysisModel
                    {
                        Date = group.Key.Month,
                        AccountType = group.Key.Category,
                        Amount = group.Sum(x => long.TryParse(x.Amount, out var amt) ? amt : 0).ToString()
                    })
                    .ToList();
            }
            else
            {
                // 如果沒有 analyzeTypes 和 conditionTypes，返回原始數據
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
