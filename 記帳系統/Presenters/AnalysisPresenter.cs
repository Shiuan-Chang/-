using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.AnalysisChart;
using 記帳系統.Contract;
using 記帳系統.Forms;
using 記帳系統.Models;
using 記帳系統.Repository;
using 記帳系統.Utility.Builder.Interface;

namespace 記帳系統.Presenters
{
    public class AnalysisPresenter : IAnalysisPresenter
    {
        private bool isLoading;
        private string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
        public IAnalysisView analysisView;
        public IRepository repository;
        public IMapper mapper;
        private AnalysisForm analysisForm;


        public AnalysisPresenter(IAnalysisView view, IMapper mapper)
        {
            analysisView = view;
            repository = new CSVRepository();
            this.mapper = mapper;
        }

        private List<AnalysisModel> ConvertToAnalysisModel(List<AnalysisRawDataDAO> rawDataList)
        {
            return mapper.Map<List<AnalysisModel>>(rawDataList);
        }


        public void LoadData(DateTime startDate, DateTime endDate)
        {
            isLoading = true;
            var rawDataList = repository.AnalysisGetDatasByDate(startDate, endDate);
            var analysisModelList = ConvertToAnalysisModel(rawDataList);
            isLoading = false;
            //通知view的程式
            analysisView.UpdateDataView(analysisModelList);
        }

        public void CreateChart(string chartType, DateTime startDate, DateTime endDate) 
        {
            // 獲取視圖中的條件和分析類型
            List<string> currentConditionTypes = analysisView.GetConditionTypes();
            List<string> currentAnalyzeTypes = analysisView.GetAnalyzeTypes();

            // 建立 ChartBuilder 實例
            IChartBuilder chartBuilder = new ChartBuilder();

            // 使用 Builder 來建立圖表
            Chart chart = chartBuilder
                .GetRawDatas(startDate, endDate)
                .GroupData(currentConditionTypes, currentAnalyzeTypes)
                .GetChartType(chartType)
                .GetSeries()
                .Build(); // Build() 現在返回 Chart

            Console.WriteLine("Chart 已生成並傳遞給 DisplayChart 方法。");

            // 將生成的 Chart 傳遞給視圖
            analysisView.DisplayChart(chart);
        }
    }
}
// pie chart
//public static List<AnalysisModel> GetPieChartData(List<AnalysisRawDataDAO> lists, List<string> conditionTypes, List<string> analyzeTypes)
//{
//    // 根據 conditionTypes 篩選
//    var filteredData = conditionTypes.Count > 0
//        ? lists.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
//        : lists;

//    // 是否進入分析模式
//    bool isAnalysisMode = analyzeTypes.Count > 0;

//    if (isAnalysisMode)
//    {
//        return filteredData.GroupBy(item =>
//            analyzeTypes.Contains("帳目類型") ? item.AccountType :
//            analyzeTypes.Contains("用途") ? item.Detail :
//            analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null)
//        .Select(group =>
//        {
//            string key = group.Key;
//            return new AnalysisModel
//            {
//                AccountType = analyzeTypes.Contains("帳目類型") ? key : null,
//                Detail = analyzeTypes.Contains("用途") ? key : null,
//                PaymentMethod = analyzeTypes.Contains("支付方式") ? key : null,
//                Amount = group.Sum(x =>
//                {
//                    if (long.TryParse(x.Amount, out var parsedAmount)) { return parsedAmount; }
//                    else { return 0; }
//                }).ToString()
//            };
//        }).ToList();
//    }
//    else
//    {
//        return filteredData.Select(item => new AnalysisModel
//        {
//            AccountType = item.AccountType,
//            Detail = item.Detail,
//            PaymentMethod = item.PaymentMethod,
//            Amount = item.Amount
//        }).ToList();
//    }
//}

//private void CreatePieChart()
//{
//    // 獲取原始資料
//    var rawDataList = repository.GetPieChartDatas(startPicker.Value, endPicker.Value);

//    // 根據條件篩選資料
//    var pieChartData = GetPieChartData(rawDataList, conditionTypes, analyzeTypes);

//    // 清除舊的圖表數據
//    chart1.Series.Clear();

// 建立新的圓餅圖
//var series = new Series("PieChart")
//{
//    ChartType = SeriesChartType.Pie,
//    XValueType = ChartValueType.String,
//    IsValueShownAsLabel = true,
//    LabelForeColor = Color.Blue,
//    CustomProperties = "PieLabelStyle=Outside",
//    Label = "#VALX: #VALY (#PERCENT)" // 顯示類別名稱、金額和百分比
//};

//    // 添加數據到圓餅圖
//    foreach (var data in pieChartData)
//    {
//        if (decimal.TryParse(data.Amount, out var amount))
//        {
//            series.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod, amount);
//        }
//    }

//    chart1.Series.Add(series);
//}

//// stackedBarChart
//public static List<AnalysisModel> GetStackedBarChartData(List<AnalysisRawDataDAO> lists, List<string> conditionTypes)
//{
//    // 篩選條件類型資料
//    var filteredData = conditionTypes.Count > 0
//        ? lists.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
//        : lists;

//    // 分組資料，按月份和開銷分類
//    return filteredData
//        .Where(item => DateTime.TryParse(item.Date, out _)) // 確保 Date 是有效日期

//        .GroupBy(item =>
//        {
//            DateTime.TryParse(item.Date, out var parsedDate);
//            string month = parsedDate.ToString("yyyy-MM"); // 提取年月作為分組依據
//            return new { Month = month, Category = item.AccountType };
//        })
//        .Select(group =>
//        {
//            var key = group.Key;
//            return new AnalysisModel
//            {
//                Date = key.Month, // 使用月份作為 Date
//                AccountType = key.Category,
//                Amount = group.Sum(x =>
//                {
//                    if (long.TryParse(x.Amount, out var parsedAmount)) { return parsedAmount; }
//                    else { return 0; }
//                }).ToString()
//            };
//        }).ToList();
//}

//private void CreateStackedBarChart()
//{
//    // 獲取原始資料
//    var rawDataList = repository.GetPieChartDatas(startPicker.Value, endPicker.Value);

//    // 根據條件篩選資料，並按月份分組
//    var stackedBarData = GetStackedBarChartData(rawDataList, conditionTypes);

//    // 獲取所有月份
//    var months = stackedBarData.Select(x => x.Date).Distinct().OrderBy(x => x).ToList();

//    // 獲取所有分類
//    var categories = stackedBarData.Select(x => x.AccountType).Distinct().ToList();

//    // 清除舊的圖表數據
//    chart1.Series.Clear();
//    chart1.ChartAreas.Clear();

//    // 設置圖表區域
//    var chartArea = new ChartArea();

//    // 配置 X 軸
//    chartArea.AxisX.Title = "月份";
//    chartArea.AxisX.TitleFont = new Font("Arial", 10f);
//    chartArea.AxisX.Interval = 1;
//    chartArea.AxisX.LineColor = Color.Gray;
//    chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
//    chartArea.AxisX.LabelStyle.Font = new Font("Arial", 10f);

//    // 配置 Y 軸
//    chartArea.AxisY.Title = "金額";
//    chartArea.AxisY.TitleFont = new Font("Arial", 10f);
//    chartArea.AxisY.LineColor = Color.Gray;
//    chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
//    chartArea.AxisY.LabelStyle.Font = new Font("Arial", 10f);

//    chart1.ChartAreas.Add(chartArea);

//    // 為每個分類建立系列
//    foreach (var category in categories)
//    {
//        var series = new Series(category)
//        {
//            ChartType = SeriesChartType.StackedColumn,
//            XValueType = ChartValueType.String,
//            IsValueShownAsLabel = true, // 顯示標籤
//            LabelForeColor = Color.Black // 標籤文字顏色
//        };

//        // 為該分類添加數據點
//        foreach (var month in months)
//        {
//            var dataPoint = stackedBarData.FirstOrDefault(x => x.Date == month && x.AccountType == category);
//            if (dataPoint != null && decimal.TryParse(dataPoint.Amount, out var amount))
//            {
//                var point = new DataPoint
//                {
//                    AxisLabel = month, // X 軸標籤
//                    YValues = new[] { (double)amount }
//                };
//                point.Label = $"{category}: {amount}"; // 顯示分類名稱和數值
//                series.Points.Add(point);
//            }
//        }

//        chart1.Series.Add(series);
//    }
//}


//// line chart
//public static List<AnalysisModel> GetLineChartData(List<AnalysisRawDataDAO> lists, List<string> conditionTypes, List<string> analyzeTypes)
//{
//    // 根據 conditionTypes 篩選
//    var filteredData = conditionTypes.Count > 0
//        ? lists.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
//        : lists;

//    // 是否進入分析模式
//    bool isAnalysisMode = analyzeTypes.Count > 0;

//    if (isAnalysisMode)
//    {
//        return filteredData.GroupBy(item =>
//            analyzeTypes.Contains("帳目類型") ? item.AccountType :
//            analyzeTypes.Contains("用途") ? item.Detail :
//            analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null)
//        .Select(group =>
//        {
//            string key = group.Key;
//            return new AnalysisModel
//            {
//                AccountType = analyzeTypes.Contains("帳目類型") ? key : null,
//                Detail = analyzeTypes.Contains("用途") ? key : null,
//                PaymentMethod = analyzeTypes.Contains("支付方式") ? key : null,
//                Amount = group.Sum(x =>
//                {
//                    if (decimal.TryParse(x.Amount, out var parsedAmount)) { return parsedAmount; }
//                    else { return 0; }
//                }).ToString(),
//                Date = group.FirstOrDefault()?.Date
//            };
//        }).ToList();
//    }
//    else
//    {
//        return filteredData.Select(item => new AnalysisModel
//        {
//            AccountType = item.AccountType,
//            Detail = item.Detail,
//            PaymentMethod = item.PaymentMethod,
//            Amount = item.Amount,
//            Date = item.Date
//        }).ToList();
//    }
//}

//private void CreateLineChart()
//{
//    // 獲取原始資料
//    var thisYearRawDataList = repository.GetLineChartDatas(startPicker.Value, endPicker.Value);
//    var lastYearRawDataList = repository.GetLineChartDatas(startPicker.Value.AddYears(-1), endPicker.Value.AddYears(-1));

//    // 根據條件篩選資料
//    var thisYearlineData = GetLineChartData(thisYearRawDataList, conditionTypes, analyzeTypes);
//    var lastYearlineData = GetLineChartData(lastYearRawDataList, conditionTypes, analyzeTypes);

//    // 分為去年和今年的數據
//    var thisYearData = thisYearlineData
//         .Where(data => DateTime.TryParse(data.Date, out var date) && date.Year == DateTime.Now.Year)
//        .GroupBy(data => data.AccountType ?? data.Detail ?? data.PaymentMethod)
//        .Select(group => new
//        {
//            Category = group.Key,
//            Amount = group.Sum(x => decimal.TryParse(x.Amount, out var amount) ? amount : 0)
//        }).ToList();

//    var lastYearData = lastYearlineData
//        .Where(data => DateTime.TryParse(data.Date, out var date) && date.Year == DateTime.Now.Year - 1)
//        .GroupBy(data => data.AccountType ?? data.Detail ?? data.PaymentMethod)
//        .Select(group => new
//        {
//            Category = group.Key,
//            Amount = group.Sum(x => decimal.TryParse(x.Amount, out var amount) ? amount : 0)
//        }).ToList();

//    // 確保類別對齊
//    var allCategories = thisYearData.Select(x => x.Category)
//        .Union(lastYearData.Select(x => x.Category))
//        .Distinct()
//        .ToList();

//    // 準備 X 和 Y 軸數據
//    string[] xValues = allCategories.ToArray();
//    double[] thisYearValues = xValues
//        .Select(category => (double)(thisYearData.FirstOrDefault(x => x.Category == category)?.Amount ?? 0))
//        .ToArray();

//    double[] lastYearValues = xValues
//        .Select(category => (double)(lastYearData.FirstOrDefault(x => x.Category == category)?.Amount ?? 0))
//        .ToArray();

//    // 清空現有的圖表內容
//    chart1.Series.Clear();
//    chart1.ChartAreas.Clear();
//    chart1.Legends.Clear();

//    // 建立新的折線圖
//    LineChart lineChart = new LineChart("消費數據分析", xValues, thisYearValues, lastYearValues);

//    // 添加折線圖到圖表
//    chart1.ChartAreas.Add(lineChart.Chart.ChartAreas[0]);

//    foreach (var series in lineChart.Chart.Series)
//    {
//        chart1.Series.Add(series);
//    }

//    if (lineChart.Chart.Legends.Count > 0)
//    {
//        chart1.Legends.Add(lineChart.Chart.Legends[0]);
//    }
//}