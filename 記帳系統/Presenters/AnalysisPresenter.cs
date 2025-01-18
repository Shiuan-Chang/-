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
                .GetChartType(chartType)
                .GroupData(currentConditionTypes, currentAnalyzeTypes)
                .GetSeries()
                .Build(); // Build() 現在返回 Chart

         

            // 將生成的 Chart 傳遞給視圖
            analysisView.DisplayChart(chart);
        }
    }
}

