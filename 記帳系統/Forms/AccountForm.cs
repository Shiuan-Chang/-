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
            ClearDataGridView();
            //dataGridView1.DataSource = lists;
            //dataGridView1.SetupDataColumns(lists);

            // 根據 lists的rawdata 還有 conditionType的 List 還有 analyzeType的List 去篩選和群組資料
            // 提示: group by的條件有可能會沒有，所以需要動態調整group by的欄位
            // 所以如果group by 是空的 就相等於沒有做group by 一樣顯示raw data (condition type 也是相同原理)

            var filteredData = lists.Where(item => conditionTypes.Contains(item.accountType))
                        .ToList();

            if (analyzeTypes.Count > 0)
            {
                var groupedData = filteredData.GroupBy(item => new
                {
                    GroupKey = analyzeTypes.Contains("SomeCondition") ? item.amount : null,
                    // 添加其他 group by 的條件
                })
                .Select(group => new { group.Key, Items = group.ToList() })
                .ToList();
            }
            else
            {
                // 沒有 group by 條件就顯示原始資料
                dataGridView1.DataSource = filteredData;
            }
        }

        private void ConditionPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GroupTypeChanged(object sender, EventArgs e)
        {
            CheckBox item = (CheckBox)sender;
            if (item.Checked)
            { analyzeTypes.Add(item.Text); }
            else
            { analyzeTypes.Remove(item.Text); }
        }
        //public AccountForm()
        //{
        //    InitializeComponent();
        //}

    }
}
