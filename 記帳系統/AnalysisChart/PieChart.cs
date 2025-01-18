using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace 記帳系統.AnalysisChart
{
    public class PieChart
    {
        //public Chart Chart { get; private set; }

        //public PieChart(string title, string[] xValues, double[] yValues)
        //{
        //    Chart = new Chart();

        //    // 設置標題
        //    Chart.Titles.Add(title);
        //    Chart.Titles[0].ForeColor = Color.Blue;
        //    Chart.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
        //    Chart.Titles[0].Alignment = ContentAlignment.TopCenter;

        //    // 配置背景
        //    Chart.BackColor = Color.Transparent;
        //    Chart.ChartAreas.Add(new ChartArea());
        //    Chart.ChartAreas[0].BackColor = Color.Transparent;
        //    Chart.ChartAreas[0].BorderColor = Color.Transparent;

        //    // 配置圖例
        //    Legend legend = new Legend
        //    {
        //        Title = "圖例",
        //        BackColor = Color.Transparent,
        //        ForeColor = Color.Blue,
        //        Font = new Font("微软雅黑", 8f, FontStyle.Regular)
        //    };
        //    Chart.Legends.Add(legend);

        //    // 配置 Series
        //    Series series = new Series
        //    {
        //        ChartType = SeriesChartType.Pie, // 圓餅圖
        //        XValueType = ChartValueType.String,
        //        IsValueShownAsLabel = true,
        //        LabelForeColor = Color.Blue,
        //        CustomProperties = "PieLabelStyle=Outside",
        //        Label = "#VALX:(#PERCENT)" // 同時顯示 X 軸標籤、數值和百分比
        //    };
        //    series.Points.DataBindXY(xValues, yValues);

        //    // 設置顏色
        //    series.Palette = ChartColorPalette.BrightPastel;
        //    Chart.Series.Add(series);
        //}
    }
    }
