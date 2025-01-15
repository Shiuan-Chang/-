using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.Repository;

namespace StackColumnChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //1.取得原始資料
            //2.設定圖表類型
            //3.根據圖表類型群組/篩選資料
            //4.創建Series
            //5.根據圖表類型與Series創建圖表

            IRepository repository = new CSVRepository();

            // 獲取原始資料
            var rawDataList = repository.GetPieChartDatas(new DateTime(2025,1,2), new DateTime(2025, 2, 10));


            // 設置圖表區域
            var chartArea = new ChartArea();

            // 配置 X 軸
            chartArea.AxisX.Title = "月份";
            chartArea.AxisX.TitleFont = new Font("Arial", 10f);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LineColor = Color.Gray;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 10f);

            // 配置 Y 軸
            chartArea.AxisY.Title = "金額";
            chartArea.AxisY.TitleFont = new Font("Arial", 10f);
            chartArea.AxisY.LineColor = Color.Gray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 10f);

            chart1.ChartAreas.Add(chartArea);


            var types = rawDataList.GroupBy(x=>x.AccountType).Select(x=>x.Key).ToList();    
            foreach(string type in types)
            {
                var series = new Series(type)
                {
                    ChartType = SeriesChartType.StackedColumn,
                    XValueType = ChartValueType.String,
                    IsValueShownAsLabel = true, // 顯示標籤
                    LabelForeColor = Color.Black // 標籤文字顏色
                };

                var datas = rawDataList.Where(x=>x.AccountType == type).GroupBy(x => DateTime.Parse(x.Date).ToString("yyyy-MM")).Select(x=> new DataPoint()
                {
                    Label = type,
                    AxisLabel = x.Key,
                    YValues = new[] { (double)x.Sum(y => int.Parse(y.Amount)) }
                }).ToArray();

                foreach (var data in datas)
                    series.Points.Add(data);
                chart1.Series.Add(series);
            }

            //string dateLabel = rawDataList.GroupBy(x => DateTime.Parse(x.Date).ToString("yyyy-MM")).Select(x => x.Key).First();
            //var datas = rawDataList.Where(x => DateTime.Parse(x.Date).ToString("yyyy-MM") == dateLabel).GroupBy(x => new { Date =DateTime.Parse(x.Date).ToString("yyyy-MM"),x.AccountType }).Select(x => new DataPoint
            //{
            //    AxisLabel = x.Key.Date,
            //    Label = x.Key.AccountType,
            //    YValues = new[] { (double)x.Sum(y => int.Parse(y.Amount)) }
            //}).ToArray();

            //foreach ( var data in datas )
            //    series.Points.Add(data);
            //chart1.Series.Add(series);
        }
    }
}
