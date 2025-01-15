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

        public StackedBarChart(string title, List<string> months, Dictionary<string, double[]> categoryData)
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
                BackColor = Color.Transparent
            };
            // 配置 AxisX
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.Title = "月份";
            chartArea.AxisX.TitleFont = new Font("Arial", 10f);
            chartArea.AxisX.LineColor = Color.Gray;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 10f);

            // 配置 AxisY
            chartArea.AxisY.Title = "金額";
            chartArea.AxisY.TitleFont = new Font("Arial", 10f);
            chartArea.AxisY.LineColor = Color.Gray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 10f);

            Chart.ChartAreas.Add(chartArea);

            // 為每個分類添加一個系列
            foreach (var category in categoryData.Keys)
            {
                Series series = new Series
                {
                    Name = category,
                    ChartType = SeriesChartType.StackedColumn, // 堆疊柱狀圖
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };

                // 將數據綁定到系列
                series.Points.DataBindXY(months, categoryData[category]);
                Chart.Series.Add(series);
            }

            // 添加圖例
            Legend legend = new Legend
            {
                Title = "分類",
                TitleFont = new Font("Arial", 10f, FontStyle.Bold),
                BackColor = Color.Transparent,
                Font = new Font("Arial", 9f),
                ForeColor = Color.Black
            };
            Chart.Legends.Add(legend);
        }
    }
}
