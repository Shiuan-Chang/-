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
    public class PieChartSeries : ICreateSeriesStrategy
    {
        public List<Series> CreateSeries(List<AnalysisModel> processedData)
        {
            var series = new Series
            {
                Name = "圓餅圖",
                ChartType = SeriesChartType.Pie,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Blue,
                CustomProperties = "PieLabelStyle=Outside",
                Label = "#VALX: #VALY (#PERCENT)"
            };

            foreach (var data in processedData)
            {
                if (decimal.TryParse(data.Amount, out var amount))
                {
                    series.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod, amount);
                }
            }

            return new List<Series> { series };
        }
    }
}
