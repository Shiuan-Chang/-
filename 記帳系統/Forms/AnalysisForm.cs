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
using System.Globalization;

namespace 記帳系統.Forms
{
    [DisplayName("圖表分析")]
    public partial class AnalysisForm : Form, IAnalysisView
    {
        private AnalysisPresenter analysisPresenter;
        private List<string> conditionTypes = new List<string>();
        private List<string> analyzeTypes = new List<string>();
        IRepository repository = new CSVRepository();


        
        public AnalysisForm()
        {
            InitializeComponent();
            startPicker.Value = DateTime.Today.AddDays(-30);
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = config.CreateMapper();
            analysisPresenter = new AnalysisPresenter(this, mapper);
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

        public List<string> GetConditionTypes()
        {
            return new List<string>(conditionTypes);
        }

        public List<string> GetAnalyzeTypes()
        {
            return new List<string>(analyzeTypes);
        }

        private void SearchData()
        {
            analysisPresenter.LoadData(startPicker.Value, endPicker.Value);
        }

        private static System.Threading.Timer debounceTimer;
        private static object timerLock = new object();
        private bool isLoading = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(() => {
                analysisPresenter.LoadData(startPicker.Value, endPicker.Value);
            }, 1000);
        }

        public void ClearDataGridView()
        {
          
            GC.Collect();
        }

        public void UpdateDataView(List<AnalysisModel> lists)
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

            if (comboBox1.SelectedItem == null) return;
            string chartType = comboBox1.SelectedItem.ToString();
            analysisPresenter.CreateChart(chartType,startPicker.Value,endPicker.Value);
        }
        
        public void DisplayChart(Chart chart)
        {
            RenderChart(chart);
        }

        private void RenderChart(Chart chart)
        {
            ChartLayout.Controls.Clear();
            chart.Dock = DockStyle.Fill;
            ChartLayout.Controls.Add(chart);
        }

    }
}
