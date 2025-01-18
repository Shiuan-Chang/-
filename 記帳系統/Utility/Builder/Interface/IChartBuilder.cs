using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Utility.Builder.Interface
{
    public interface  IChartBuilder
    {
        IChartBuilder GetRawDatas(DateTime startDate, DateTime endDate);
        IChartBuilder GroupData(List<string> conditionTypes, List<string> analyzeTypes);
        IChartBuilder GetChartType(string selectItem);
        IChartBuilder GetSeries();

        Chart Build();
    }
}
