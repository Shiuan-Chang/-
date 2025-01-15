using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.Models;

namespace 記帳系統.Contract
{
    public interface IAnalysisView
    {
        void UpdateDataView(List<AnalysisModel> lists);
        void Reload();
        void DisplayChart(Chart chart); // 用於顯示圖表
        List<string> GetConditionTypes(); // 獲取條件類型
        List<string> GetAnalyzeTypes(); // 獲取分析類型
    }
}
