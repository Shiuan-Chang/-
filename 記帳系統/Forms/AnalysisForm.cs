using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Mappings;
using System.Windows.Forms;
using 記帳系統.Contract;
using 記帳系統.Models;
using 記帳系統.Presenters;
using 記帳系統.DataGridViewExtension;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳系統.AnalysisChart;
using System.Reflection.Emit;
using 記帳系統.Repository;

namespace 記帳系統.Forms
{
    [DisplayName("圖表分析")]
    public partial class AnalysisForm : Form, INoteView
    {
        private NotePresenter notePresenter;
        List<string> conditionTypes = new List<string>();
        List<string> analyzeTypes = new List<string>();
        IRepository repository = new CSVRepository();
        PieChartModel pieChartModel = new PieChartModel();
        
        public AnalysisForm()
        {
            InitializeComponent();
            startPicker.Value = DateTime.Today.AddDays(-30);
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = config.CreateMapper();
            notePresenter = new NotePresenter(this, mapper);


        }

        private void AnalysisForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            Dictionary<string, List<string>> types = DropDownModel.Types;
            foreach (var items in types)
            {
                FlowLayoutPanel OptionPanel = new FlowLayoutPanel();
                OptionPanel.Width = ConditionPanel.Width;
                OptionPanel.Height = 30;
                CheckBox itemType = new CheckBox();
                itemType.CheckedChanged += ItemType_CheckedChanged;
                itemType.Text = items.Key.ToString();
                itemType.Tag = "condition";
                OptionPanel.Tag = "condition";
                OptionPanel.Controls.Add(itemType);

                // 寫一個方法觸發選擇資訊丟到list
                foreach (var option in items.Value)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Tag = "condition";
                    checkBox.Width = 90;
                    checkBox.Text = option;
                    checkBox.CheckedChanged += CheckBox_CheckedChanged; ;
                    OptionPanel.Controls.Add(checkBox);
                }
                ConditionPanel.Controls.Add(OptionPanel);
            }
        }

        private void ItemType_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox itemType = (CheckBox)sender;
            FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)itemType.Parent;
            foreach (var item in flowLayoutPanel.Controls.OfType<CheckBox>())
            {
                item.Checked = itemType.Checked;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox item = (CheckBox)sender;
            string tag = item.Tag?.ToString();

            if (tag == "condition")
            {
                if (item.Checked) conditionTypes.Add(item.Text);
                else conditionTypes.Remove(item.Text);
            }
            else if (tag == "analyze")
            {
                if (item.Checked) analyzeTypes.Add(item.Text);
                else analyzeTypes.Remove(item.Text);
            }
        }

        public void Reload()
        {
            SearchData();
        }

        private void SearchData()
        {
            notePresenter.LoadData(startPicker.Value, endPicker.Value);
        }

        private static System.Threading.Timer debounceTimer;
        private static object timerLock = new object();
        private bool isLoading = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(() => {
                notePresenter.LoadData(startPicker.Value, endPicker.Value);
            }, 1000);
        }

        public void ClearDataGridView()
        {
          
            GC.Collect();
        }


        public void UpdateDataView(List<NoteModel> lists)
        {
            ClearDataGridView();
           
        }

        

        private void navBar1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空現有的圖表內容
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.Titles.Clear();



            string[] xValues = { "用餐", "租屋", "學習" };
            double[] yValues = { 110, 10000, 2000 };
            double[] thisYearValues = { 110, 10000, 2000 }; // 今年數據
            double[] lastYearValues = { 90, 9500, 2500 };

            if (comboBox1.SelectedItem == null) return;
            string selectItem = comboBox1.SelectedItem.ToString();

            switch (selectItem)
            {
                case "圓餅圖":

                    CreatePieChart();

                    break;

                case "推疊圖":
                    {
                        
                        string[] xLabels = { "用餐", "租屋", "學習" };
                        StackedBarChart stackedBarChart = new StackedBarChart("Yearly Comparison", xLabels, thisYearValues, lastYearValues);
                        chart1.ChartAreas.Clear();
                        chart1.Series.Clear();
                        chart1.Legends.Clear();

                        
                        chart1.ChartAreas.Add(stackedBarChart.Chart.ChartAreas[0]);

                        
                        foreach (var series in stackedBarChart.Chart.Series)
                        {
                            chart1.Series.Add(series);
                        }

                        
                        if (stackedBarChart.Chart.Legends.Count > 0) // legends 圖例
                        {
                            chart1.Legends.Add(stackedBarChart.Chart.Legends[0]);
                        }
                        break;
                    }

                case "折線圖(跟去年同期相比)":
                    LineChart lineChart = new LineChart("折線圖分析", xValues, thisYearValues, lastYearValues);
                   
                    chart1.ChartAreas.Clear();
                    chart1.ChartAreas.Add(lineChart.Chart.ChartAreas[0]);
                    chart1.Series.Clear();
                    foreach (var series in lineChart.Chart.Series)
                    {
                        chart1.Series.Add(series);
                    }
                    break;
            }
        }


        public static List<AnalysisModel> GetPieChartData(List<AnalysisRawDataDAO> lists, List<string> conditionTypes, List<string> analyzeTypes)
        {
            // 根據 conditionTypes 篩選
            var filteredData = conditionTypes.Count > 0
                ? lists.Where(item => conditionTypes.Contains(item.AccountType)).ToList()
                : lists;

            // 是否進入分析模式
            bool isAnalysisMode = analyzeTypes.Count > 0;

            if (isAnalysisMode)
            {
                return filteredData.GroupBy(item =>
                    analyzeTypes.Contains("帳目類型") ? item.AccountType :
                    analyzeTypes.Contains("用途") ? item.Detail :
                    analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null)
                .Select(group =>
                {
                    string key = group.Key;
                    return new AnalysisModel
                    {
                        AccountType = analyzeTypes.Contains("帳目類型") ? key : null,
                        Detail = analyzeTypes.Contains("用途") ? key : null,
                        PaymentMethod = analyzeTypes.Contains("支付方式") ? key : null,
                        Amount = group.Sum(x =>
                        {
                            if (long.TryParse(x.Amount, out var parsedAmount)) { return parsedAmount; }
                            else { return 0; }
                        }).ToString()
                    };
                }).ToList();
            }
            else
            {
                return filteredData.Select(item => new AnalysisModel
                {
                    AccountType = item.AccountType,
                    Detail = item.Detail,
                    PaymentMethod = item.PaymentMethod,
                    Amount = item.Amount
                }).ToList();
            }
        }


        private void CreatePieChart() 
        {
            // 獲取原始資料
            var rawDataList = repository.GetPieChartDatas(startPicker.Value, endPicker.Value);

            // 根據條件篩選資料
            var pieChartData = GetPieChartData(rawDataList, conditionTypes, analyzeTypes);

            // 清除舊的圖表數據
            chart1.Series.Clear();

            // 建立新的圓餅圖
            var series = new Series("PieChart")
            {
                ChartType = SeriesChartType.Pie,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Blue,
                CustomProperties = "PieLabelStyle=Outside",
                Label = "#VALX: #VALY (#PERCENT)" // 顯示類別名稱、金額和百分比
            };

            // 添加數據到圓餅圖
            foreach (var data in pieChartData)
            {
                if (decimal.TryParse(data.Amount, out var amount))
                {
                    series.Points.AddXY(data.AccountType ?? data.Detail ?? data.PaymentMethod, amount);
                }
            }

            chart1.Series.Add(series);
        }
    }
}
