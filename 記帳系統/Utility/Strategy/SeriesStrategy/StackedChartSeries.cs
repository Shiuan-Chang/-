using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.Models;
using 記帳系統.Utility.Strategy.Interface;

namespace 記帳系統.Utility.Strategy.SeriesStrategy
{
    public class StackedChartSeries : ICreateSeriesStrategy
    {
        public List<Series> CreateSeries(List<AnalysisModel> processedData)
        {
            // 初始化 Series 列表
            var seriesList = new List<Series>();

            // 獲取所有月份
            var months = processedData
                .Select(x => x.Date)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // 獲取所有分類（支援多欄位：AccountType, Detail, PaymentMethod）
            var categories = processedData
                .Select(x => x.AccountType ?? x.Detail ?? x.PaymentMethod)
                .Distinct()
                .ToList();

            // 為每個分類建立 Series
            foreach (var category in categories)
            {
                var series = new Series(category)
                {
                    ChartType = SeriesChartType.StackedColumn,
                    XValueType = ChartValueType.String,
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };

                // 添加數據點
                foreach (var month in months)
                {
                    // 查找當前分類和月份的數據點
                    var dataPoint = processedData.FirstOrDefault(x =>
                        x.Date == month &&
                        (x.AccountType == category || x.Detail == category || x.PaymentMethod == category));

                    if (dataPoint != null && decimal.TryParse(dataPoint.Amount, out var amount))
                    {
                        // 添加數據點
                        var point = new DataPoint
                        {
                            AxisLabel = month, // X 軸標籤（月份）
                            YValues = new[] { (double)amount } // Y 軸數值
                        };

                        // 顯示分類和數值
                        point.Label = $"{category}: {amount}";

                        // 添加數據點到系列
                        series.Points.Add(point);
                    }
                }

                // 將 Series 添加到列表
                seriesList.Add(series);
            }

            return seriesList;
        }
    }
}
