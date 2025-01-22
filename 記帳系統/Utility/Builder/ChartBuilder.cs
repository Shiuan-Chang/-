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
using 記帳系統.Utility.Strategy.Interface;
using 記帳系統.Utility.Strategy;

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
            // 拿到原始數據
            rawDataList = repository.GetChartDatas(startDate, endDate);
            return this;
        }

        public IChartBuilder GetChartType(string selectItem)
        {
            chartType = selectItem;
            return this;
        }

        // 反射配合策略模式，反射用來尋找chartType，並從策略模式找到對應的processedData
        public IChartBuilder GroupData(List<string> conditionTypes, List<string> analyzeTypes)
        {
            var typeName = $"記帳系統.Utility.Strategy.{chartType}GroupingStrategy"; // 是PieChartGroupingStrategy, StackedChartGroupingStrategy的後綴
            //
            var type = Type.GetType(typeName);

            // 使用反射創建策略實例
            var strategy = (IGroupingStrategy)Activator.CreateInstance(type);

            // 使用策略執行分組邏輯
            processedData = strategy.GroupData(rawDataList, conditionTypes, analyzeTypes);

            return this;
        }

        public IChartBuilder GetSeries()
        {
            var typeName = $"記帳系統.Utility.Strategy.SeriesStrategy.{chartType}Series"; 
            var type = Type.GetType(typeName);
            var strategy = (ICreateSeriesStrategy)Activator.CreateInstance(type);
            // 使用策略類生成 Series
            seriesList = strategy.CreateSeries(processedData);
            return this;
        }

        public Chart Build()
        {
            Type type = Type.GetType($"記帳系統.Utility.Strategy.ChartStrategy.{chartType}Strategy");
            var strategy = (ICreateChartStrategy)Activator.CreateInstance(type);
            // 使用策略設置圖表區域和數據
            strategy.SetChartArea(chart);         // 設置 ChartArea
            chart.Series.Clear();
            // 將 Series 添加到 Chart
            foreach (var series in seriesList)
            {
                chart.Series.Add(series);
            }
            return chart;
        }
    }
}