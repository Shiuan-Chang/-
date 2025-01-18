using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.Models;
using 記帳系統.Utility.Strategy.Interface;

namespace 記帳系統.Utility.Strategy.SeriesStrategy
{
    internal class LineChartSeries : ICreateSeriesStrategy
    {
        public List<Series> CreateSeries(List<AnalysisModel> processedData)
        {
            // 分為去年和今年的數據
            var thisYearData = processedData
                .Where(data => DateTime.TryParse(data.Date, out var date) && date.Year == DateTime.Now.Year)
                .GroupBy(data => data.AccountType ?? data.Detail ?? data.PaymentMethod)
                .Select(group => new
                {
                    Category = group.Key,
                    Amount = group.Sum(x => decimal.TryParse(x.Amount, out var amount) ? amount : 0)
                }).ToList();

            var lastYearData = processedData
                .Where(data => DateTime.TryParse(data.Date, out var date) && date.Year == DateTime.Now.Year - 1)
                .GroupBy(data => data.AccountType ?? data.Detail ?? data.PaymentMethod)
                .Select(group => new
                {
                    Category = group.Key,
                    Amount = group.Sum(x => decimal.TryParse(x.Amount, out var amount) ? amount : 0)
                }).ToList();

            // 確保類別對齊
            var allCategories = thisYearData.Select(x => x.Category)
                .Union(lastYearData.Select(x => x.Category))
                .Distinct()
                .ToList();

            // 準備 X 和 Y 軸數據
            string[] xValues = allCategories.ToArray();
            double[] thisYearValues = xValues
                .Select(category => (double)(thisYearData.FirstOrDefault(x => x.Category == category)?.Amount ?? 0))
                .ToArray();

            double[] lastYearValues = xValues
                .Select(category => (double)(lastYearData.FirstOrDefault(x => x.Category == category)?.Amount ?? 0))
                .ToArray();

            // 初始化 Series 列表
            var seriesList = new List<Series>();

            // 建立今年的數據 Series
            var thisYearSeries = new Series("今年")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Blue
            };

            for (int i = 0; i < xValues.Length; i++)
            {
                thisYearSeries.Points.AddXY(xValues[i], thisYearValues[i]);
            }

            seriesList.Add(thisYearSeries);

            // 建立去年的數據 Series
            var lastYearSeries = new Series("去年")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Red
            };

            for (int i = 0; i < xValues.Length; i++)
            {
                lastYearSeries.Points.AddXY(xValues[i], lastYearValues[i]);
            }

            seriesList.Add(lastYearSeries);

            return seriesList;
        }
    }
}
