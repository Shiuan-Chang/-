using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace 記帳系統.AnalysisChart
{
    public class StackedBarChart
    {
        public Chart Chart { get; private set; }

        public StackedBarChart(string title, string[] xLabels, double[] thisYearValues, double[] lastYearValues)
        {
            Chart = new Chart();

            // 設置標題
            Chart.Titles.Add(title);
            Chart.Titles[0].ForeColor = Color.Black;
            Chart.Titles[0].Font = new Font("Arial", 14f, FontStyle.Bold);
            Chart.Titles[0].Alignment = ContentAlignment.TopCenter;

            // 配置圖表區域
            ChartArea chartArea = new ChartArea
            {
                BackColor = Color.Transparent,
                AxisX = {
                Interval = 1,
                Title = "Categories",
                TitleFont = new Font("Arial", 10f),
                LineColor = Color.Gray,
                LabelStyle = { ForeColor = Color.Black, Font = new Font("Arial", 10f) }
            },
                AxisY = {
                Title = "Values",
                TitleFont = new Font("Arial", 10f),
                LineColor = Color.Gray,
                LabelStyle = { ForeColor = Color.Black, Font = new Font("Arial", 10f) }
            }
            };
            Chart.ChartAreas.Add(chartArea);

            // 添加今年數據的 Series
            Series thisYearSeries = new Series
            {
                Name = "今年",
                ChartType = SeriesChartType.StackedColumn, // 堆疊柱狀圖
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                Color = Color.Blue // 顏色
            };
            thisYearSeries.Points.DataBindXY(xLabels, thisYearValues);
            Chart.Series.Add(thisYearSeries);

            // 添加去年數據的 Series
            Series lastYearSeries = new Series
            {
                Name = "去年",
                ChartType = SeriesChartType.StackedColumn, // 堆疊柱狀圖
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                Color = Color.Red // 顏色
            };
            lastYearSeries.Points.DataBindXY(xLabels, lastYearValues);
            Chart.Series.Add(lastYearSeries);

            // 添加圖例
            Legend legend = new Legend
            {
                Title = "Year Comparison",
                TitleFont = new Font("Arial", 10f, FontStyle.Bold),
                BackColor = Color.Transparent,
                Font = new Font("Arial", 9f),
                ForeColor = Color.Black
            };
            Chart.Legends.Add(legend);
        }
    }
}
