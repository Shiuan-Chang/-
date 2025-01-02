using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace 記帳系統.AnalysisChart
{
    public class LineChart
    {
        public Chart Chart { get; private set; }

        public LineChart(string title, string[] xValues, double[] yValues, double[] lastYearValues)
        {
            Chart = new Chart();

            // 設置標題
            Chart.Titles.Add(title);
            Chart.Titles[0].ForeColor = Color.Blue;
            Chart.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
            Chart.Titles[0].Alignment = ContentAlignment.TopCenter;

            // 配置背景
            Chart.BackColor = Color.Transparent;
            Chart.ChartAreas.Add(new ChartArea());
            Chart.ChartAreas[0].BackColor = Color.Transparent;
            Chart.ChartAreas[0].BorderColor = Color.Transparent;

            // 配置 X 軸
            Chart.ChartAreas[0].AxisX.Interval = 1;
            Chart.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            Chart.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a");
            Chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;

            // 配置 Y 軸
            Chart.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            Chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;

            // 配置圖例
            Legend legend = new Legend("#VALX")
            {
                Title = "圖例",
                BackColor = Color.Transparent,
                ForeColor = Color.Blue,
                Font = new Font("微软雅黑", 8f, FontStyle.Regular)
            };
            Chart.Legends.Add(legend);

            // 配置今年數據的 Series
            Series thisYearSeries = new Series
            {
                ChartType = SeriesChartType.Line, // 折線圖
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Blue,
                Name = "今年"
            };
            thisYearSeries.Points.DataBindXY(xValues, yValues);

            // 設置今年數據顏色
            thisYearSeries.Color = Color.LimeGreen;
            Chart.Series.Add(thisYearSeries);

            // 配置去年數據的 Series
            Series lastYearSeries = new Series
            {
                ChartType = SeriesChartType.Line, // 折線圖
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Red,
                Name = "去年"
            };
            lastYearSeries.Points.DataBindXY(xValues, lastYearValues);

            // 設置去年數據顏色
            lastYearSeries.Color = Color.Red;
            Chart.Series.Add(lastYearSeries);
        }
    }
}
