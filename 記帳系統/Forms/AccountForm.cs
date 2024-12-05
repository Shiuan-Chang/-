using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Contract;
using 記帳系統.DataGridViewExtension;
using 記帳系統.Models;
using 記帳系統.Presenters;

namespace 記帳系統.Forms
{
    [DisplayName("帳戶")]
    public partial class AccountForm : Form, INoteView
    {
        private INotePresenter notePresenter;//依賴注入打包?
        List<string> conditionTypes = new List<string>();
        List<string> analyzeTypes = new List<string>();
        public AccountForm()
        {
            // timer要隔一段時間後，才去做button要做的事
            // button 永遠去呼叫debounce做的事情，因此，會有一個debounce方法，debouce會更改及清空timer的時間(也就是說timer一定會在debounce中)
            InitializeComponent();
            startPicker.Value = DateTime.Today.AddDays(-30);
            notePresenter = new NotePresenter(this);
        }


        private void AccountForm_Load(object sender, EventArgs e)
        {
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
                if (items.Key == "AnalyzeType")
                {
                    itemType.Tag = "analyze";
                }
                OptionPanel.Controls.Add(itemType);

                // 寫一個方法觸發選擇資訊丟到list

                foreach (var option in items.Value)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Width = 90;
                    checkBox.Text = option;
                    checkBox.CheckedChanged += CheckBox_CheckedChanged; ;
                    OptionPanel.Controls.Add(checkBox);

                }
                ConditionPanel.Controls.Add(OptionPanel);

            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox item = (CheckBox)sender;
            if (item.Checked)
            { conditionTypes.Add(item.Text); }
            else
            { conditionTypes.Remove(item.Text); }
        }

        private void ItemType_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox itemType = (CheckBox)sender;
            FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)itemType.Parent;
            foreach (var item in flowLayoutPanel.Controls.OfType<CheckBox>())
            {
                item.Checked = itemType.Checked;
            }
            //if (itemType.Checked)
            //{
            //    FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)itemType.Parent;
            //    foreach(CheckBox option in flowLayoutPanel.Controls) 
            //    {
            //        option.Checked = true;
            //    }
            //}
            //else 
            //{
            //    FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)itemType.Parent;
            //    foreach (CheckBox option in flowLayoutPanel.Controls)
            //    {
            //        option.Checked = false;
            //    }
            //}
        }



        //一開始把計時器歸零，然後再做延遲計算，防止使用者不斷重複按查詢造成記憶體爆炸
        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(() => SearchData(), 1000);
        }

        public void Reload()
        {
            SearchData();
        }

        private void SearchData()
        {
            notePresenter.LoadData(startPicker.Value, endPicker.Value);
        }

        public void ClearDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();
        }

        public void UpdateDataView(List<AccountingModel> lists)
        {
            //// 根據 lists的rawdata 還有 conditionType的 List 還有 analyzeType的List 去篩選和群組資料
            //// 提示: group by的條件有可能會沒有，所以需要動態調整group by的欄位
            //// 所以如果group by 是空的 就相等於沒有做group by 一樣顯示raw data (condition type 也是相同原理)

            ClearDataGridView();

            // 根據 lists 的 raw data 以及 conditionType 和 analyzeType 篩選和群組
            var filteredData = conditionTypes.Count > 0 ? lists.Where(item => conditionTypes.Contains(item.accountType)).ToList() : lists;

            // 根據選擇的分析方式進行群組和篩選
            bool isAnalysisMode = analyzeTypes.Count > 0;
            if (isAnalysisMode)
            {
                var groupedData = filteredData.GroupBy(item =>
                    analyzeTypes.Contains("帳目類型") ? item.accountType :// 如果包含 "用途"，就按 item.detail（詳細用途）分組
                    analyzeTypes.Contains("用途") ? item.detail :
                    analyzeTypes.Contains("支付方式") ? item.paymentMethod : null)
                .Select(group => new AccountingModel
                {
                    accountType = group.Key,
                    detail = group.Key,
                    paymentMethod = group.Key,
                    amount = group.Sum(x => long.Parse(x.amount)).ToString() // 累加同類型的金額
                }).ToList();

                UpdateDataGridViewColumns(groupedData, isAnalysisMode);
            }
            else
            {
                UpdateDataGridViewColumns(filteredData, isAnalysisMode);
            }
        }

        private void UpdateDataGridViewColumns(List<AccountingModel> data, bool isAnalysisMode)
        {
            dataGridView1.DataSource = data;

            // 只顯示選擇的分析方式對應的欄位和金額欄位
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (!isAnalysisMode)
                {
                    column.Visible = true; // 顯示所有欄位
                }
                else
                {
                    // 如果分析方式選擇的是 "帳目類型"，只顯示 "accountType" 和 "amount" 欄位
                    if (analyzeTypes.Contains("帳目類型"))
                    {
                        column.Visible = column.Name == "accountType" || column.Name == "amount";
                    }
                    // 如果分析方式選擇的是 "用途"，只顯示 "detail" 和 "amount" 欄位
                    else if (analyzeTypes.Contains("用途"))
                    {
                        column.Visible = column.Name == "detail" || column.Name == "amount";
                    }
                    // 如果分析方式選擇的是 "支付方式"，只顯示 "paymentMethod" 和 "amount" 欄位
                    else if (analyzeTypes.Contains("支付方式"))
                    {
                        column.Visible = column.Name == "paymentMethod" || column.Name == "amount";
                    }
                }
            }
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//帳目類型
        {
            UpdateAnalyzeType("帳目類型", checkBox1.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//用途
        {
            UpdateAnalyzeType("用途", checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)//支付方式
        {
            UpdateAnalyzeType("支付方式", checkBox3.Checked);
        }


        private void UpdateAnalyzeType(string type, bool isChecked)
        {
            if (isChecked)
            {
                if (!analyzeTypes.Contains(type))
                    analyzeTypes.Add(type);
            }
            else
            {
                if (analyzeTypes.Contains(type))
                    analyzeTypes.Remove(type);
            }

        }
    }
}
