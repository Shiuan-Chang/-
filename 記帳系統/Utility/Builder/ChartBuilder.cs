using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.Models;
using 記帳系統.Repository;

namespace 記帳系統.Utility.Builder.Interface
{
    public class ChartBuilder:IChartBuilder
    {
        private readonly IRepository repository = new CSVRepository();
        private List<AnalysisRawDataDAO> rawDataList = new List<AnalysisRawDataDAO>();
        private List<AnalysisModel> processedData = new List<AnalysisModel>();
        private string chartType = "";
        private Chart chart = new Chart();
        private List<Series> seriesList = new List<Series>(); // 存儲生成的 Series

        public ChartBuilder()
        {
            // 在建構子中先添加 ChartArea
            ChartArea chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);
        }

        public IChartBuilder GetRawDatas(DateTime startDate, DateTime endDate)
        {
            rawDataList = repository.GetChartDatas(startDate, endDate);
            return this;
        }

        public IChartBuilder GroupData(List<string> conditionTypes, List<string> analyzeTypes)
        {
            // 根據 conditionTypes 篩選
            var filteredData = conditionTypes.Count > 0
                ? rawDataList.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
                : rawDataList;

            // 是否進入分析模式
            bool isAnalysisMode = analyzeTypes.Count > 0;

            if (isAnalysisMode)
            {
                processedData = filteredData
                .GroupBy(item =>
                    analyzeTypes.Contains("帳目類型") ? item.AccountType :
                    analyzeTypes.Contains("用途") ? item.Detail :
                    analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null)
                .Select(group =>
                {
                    string key = group.Key;
                    return new AnalysisModel
                    {
                        AccountType = analyzeTypes.Contains("帳目類型") ? key : null,
                        Detail = analyzeTypes.Contains("用途") ? key : null,
                        PaymentMethod = analyzeTypes.Contains("支付方式") ? key : null,
                        Amount = group.Sum(x =>
                        {
                            if (long.TryParse(x.Amount, out var parsedAmount)) { return parsedAmount; }
                            else { return 0; }
                        }).ToString()
                    };
                }).ToList();
            }
            else
            {
                processedData = filteredData.Select(item => new AnalysisModel
                {
                    AccountType = item.AccountType,
                    Detail = item.Detail,
                    PaymentMethod = item.PaymentMethod,
                    Amount = item.Amount
                }).ToList();
            }
            return this;
        }

        public IChartBuilder GetChartType(string selectItem)
        {
            chartType = selectItem;
            return this;
        }

        public IChartBuilder GetSeries()
        {
            // 根據圖表類型生成對應的 Series
            switch (chartType)
            {
                case "圓餅圖":
                    var pieSeries = new Series
                    {
                        Name = "圓餅圖",
                        ChartType = SeriesChartType.Pie,
                        XValueType = ChartValueType.String,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Blue,
                        CustomProperties = "PieLabelStyle=Outside",
                        Label = "#VALX: #VALY (#PERCENT)",
                        ChartArea = "MainArea" // 關聯到已存在的 ChartArea
                    };
                    foreach (var data in processedData)
                    {
                        if (decimal.TryParse(data.Amount, out var amount))
                        {
                            pieSeries.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod, amount);
                        }
                    }
                    seriesList.Add(pieSeries);
                    break;

                case "堆疊圖":
                    foreach (var data in processedData)
                    {
                        var series = new Series
                        {
                            Name = data.AccountType ?? data.Detail ?? data.PaymentMethod,
                            ChartType = SeriesChartType.Column,
                            ChartArea = "MainArea" // 關聯到已存在的 ChartArea
                        };
                        series.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod, Convert.ToDouble(data.Amount));
                        seriesList.Add(series);
                    }
                    break;

                case "折線圖":
                    var lineSeries = new Series
                    {
                        Name = "折線圖",
                        ChartType = SeriesChartType.Line,
                        ChartArea = "MainArea" // 關聯到已存在的 ChartArea
                    };
                    foreach (var data in processedData)
                    {
                        lineSeries.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod ?? "其他", Convert.ToDouble(data.Amount));
                    }
                    seriesList.Add(lineSeries);
                    break;
            }
            return this;
        }

        public Chart Build()
        {
            // 將所有生成的 Series 添加到 Chart 的 Series 集合中
            foreach (var series in seriesList)
            {
                chart.Series.Add(series);
            }

            // 在此處可以進行任何最終的配置或驗證
            return chart;
        }
    }
}
