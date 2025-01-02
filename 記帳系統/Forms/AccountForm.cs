using AutoMapper;
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
using 記帳系統.Mappings;
using 記帳系統.Presenters;
using 記帳系統.Repository;

namespace 記帳系統.Forms
{
    [DisplayName("帳戶")]
    public partial class AccountForm : Form, IAccountView
    {
        private AccountPresenter accountPresenter;
        List<string> conditionTypes = new List<string>();
        List<string> analyzeTypes = new List<string>();
        public AccountForm()
        {
            // timer要隔一段時間後，才去做button要做的事
            // button 永遠去呼叫debounce做的事情，因此，會有一個debounce方法，debouce會更改及清空timer的時間(也就是說timer一定會在debounce中)
            InitializeComponent();
            startPicker.Value = DateTime.Today.AddDays(-30);

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = config.CreateMapper();
            accountPresenter = new AccountPresenter(this, mapper);
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            // 產生左邊的選項
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
        }

        //一開始把計時器歸零，然後再做延遲計算，防止使用者不斷重複按查詢造成記憶體爆炸
        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(() => {
                accountPresenter.LoadData(startPicker.Value, endPicker.Value, conditionTypes, analyzeTypes);
            }, 1000);
        }

        public void ClearDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();
        }


        public void UpdateDataView(List<AccountModel> lists)
        {
            ClearDataGridView();
            dataGridView1.DataSource = lists;
            dataGridView1.SetupAccountDataColumns(lists, conditionTypes, analyzeTypes);

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
