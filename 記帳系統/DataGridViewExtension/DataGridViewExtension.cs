using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using 記帳系統.Models;
using 記帳系統.Presenters;
using 記帳系統.Repository;

    namespace 記帳系統.DataGridViewExtension
    {
    // 改完做addform MVP，用擴充完成表格顯示功能
    
    public static class DataGridViewExtension
        {

            // 定義刪除事件委派
            public delegate void DeleteActionHandler(object sender, DeleteNoteModel model);

            // 刪除事件
            public static event DeleteActionHandler OnDeleteAction;


            public static void SetupDataColumns(this DataGridView dataGridView1,List<NoteModel> lists)
            {
                dataGridView1.Columns["CompressedPicture1Path"].Visible = false;
                dataGridView1.Columns["CompressedPicture2Path"].Visible = false;
                dataGridView1.Columns["originalPicture1Path"].Visible = false;
                dataGridView1.Columns["originalPicture2Path"].Visible = false;

                dataGridView1.Columns[0].HeaderText = "帳目類型";
                dataGridView1.Columns[3].HeaderText = "細項";
                dataGridView1.Columns[4].HeaderText = "支付方式";
                dataGridView1.Columns[5].HeaderText = "金額";

                // 帳目名稱下拉選單
                dataGridView1.Columns[1].HeaderText = "帳目名稱";
                dataGridView1.Columns[1].CellTemplate = new DataGridViewTextBoxCell();

                // 帳目類型下拉選單
                dataGridView1.Columns[2].HeaderText = "日期";
                dataGridView1.Columns[2].CellTemplate = new DataGridViewTextBoxCell();


                DataGridViewImageColumn iconColumn1 = new DataGridViewImageColumn();
                DataGridViewImageColumn iconColumn2 = new DataGridViewImageColumn();
                DataGridViewImageColumn iconColumn3 = new DataGridViewImageColumn();
                iconColumn1.HeaderText = "圖片一";
                iconColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;
                iconColumn2.HeaderText = "圖片二";
                iconColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom;
                iconColumn3.HeaderText = "刪除";
                iconColumn3.ImageLayout = DataGridViewImageCellLayout.Zoom;

                dataGridView1.Columns.Insert(8, iconColumn1);
                dataGridView1.Columns.Insert(9, iconColumn2);
                dataGridView1.Columns.Insert(10, iconColumn3);

            string csvPath1 = "";

                if (dataGridView1.Rows.Count > 0)
                {
                    csvPath1 = lists[0].originalPicture1Path;
                }

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    // 先去讀csvImagePath1,2的資料
                    Bitmap bmp1 = new Bitmap(lists[row].CompressedPicture2Path);
                    Bitmap bmp2 = new Bitmap(lists[row].CompressedPicture2Path);
                    Bitmap junkImage = new Bitmap("C:\\CSharp練習\\記帳系統\\記帳系統\\Resources\\Images\\junk.png");
                    ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[8]).Value = bmp1;
                    ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[9]).Value = bmp2;
                    ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[10]).Value = junkImage;
                // 存四張圖，2張原圖縮小(50*50封面)並略調畫質，另外兩張點進去看到的是壓縮檔圖，約300-500kb
            }
            }

            // 創造項目名稱的選單
            public static void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
            {
                DataGridView dataGridView = sender as DataGridView;

                if (e.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell accountName = new DataGridViewComboBoxCell();
                    accountName.DataSource = new List<string> { "用餐", "交通", "租金", "治裝", "娛樂", "學習", "投資" }; // Your dropdown options
                    dataGridView.Rows[e.RowIndex].Cells[1] = accountName;
                }
            }

            public static DataGridViewComboBoxCell GetAccountTypeComboBox(string accountName)
            {
                DataGridViewComboBoxCell accountType = new DataGridViewComboBoxCell();

                switch (accountName)
                {
                    case "用餐":
                        accountType.DataSource = DropDownModel.GetFoodItems();
                        break;
                    case "交通":
                        accountType.DataSource = DropDownModel.GetTrafficItems();
                        break;
                    case "租金":
                        accountType.DataSource = DropDownModel.GetRentItems();
                        break;
                    case "治裝":
                        accountType.DataSource = DropDownModel.GetDressItems();
                        break;
                    case "娛樂":
                        accountType.DataSource = DropDownModel.GetEntertainmentItems();
                        break;
                    case "學習":
                        accountType.DataSource = DropDownModel.GetLearningItems();
                        break;
                    case "投資":
                        accountType.DataSource = DropDownModel.GetInvestmentItems();
                        break;
                    default:
                        accountType.DataSource = new List<string>(); // 默認為空
                        break;
                }
                return accountType;
            }

            // 修改欄位內容
        public static void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count || e.ColumnIndex != 1) return; // Ensure valid row index and correct column

                // Get the account name value safely
                string accountName = dataGridView.Rows[e.RowIndex].Cells[1]?.Value?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(accountName))
                {
                    DataGridViewComboBoxCell accountTypeCell = GetAccountTypeComboBox(accountName);
                    dataGridView.Rows[e.RowIndex].Cells[2] = accountTypeCell; // Set the ComboBox cell to the appropriate row

                    if (accountTypeCell.Items.Count > 0)
                    {
                        accountTypeCell.Value = accountTypeCell.Items[0]; // Set default value if items exist
                    }
                    else
                    {
                        accountTypeCell.Value = null; // Set null if there are no items
                    }
                }
            }

        public static void SetupAccountDataColumns(this DataGridView dataGridView1, List<AccountModel> lists, List<string> conditionTypes, List<string> analyzeTypes) 
        {
            // 根據 conditionTypes 篩選
            var filteredData = conditionTypes.Count > 0 ? lists.Where(item => conditionTypes.Contains(item.AccountType)).ToList() : lists;

            // 是否進入分析模式
            bool isAnalysisMode = analyzeTypes.Count > 0;

            if (isAnalysisMode)
            {
                var groupedData = filteredData.GroupBy(item =>
                    analyzeTypes.Contains("帳目類型") ? item.AccountType :
                    analyzeTypes.Contains("用途") ? item.Detail :
                    analyzeTypes.Contains("支付方式") ? item.PaymentMethod : null)
                .Select(group =>
                {
                    // 根據分組條件設置值
                    string key = group.Key;
                    return new AccountModel
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

                UpdateDataGridViewColumns(dataGridView1, groupedData, isAnalysisMode);
            }
            else
            {
                UpdateDataGridViewColumns(dataGridView1, filteredData, isAnalysisMode);
            }

        }

        private static void UpdateDataGridViewColumns(DataGridView dataGridView, List<AccountModel> data, bool isAnalysisMode)
        {
            // 清除現有欄位
            dataGridView.Columns.Clear();
            dataGridView.DataSource = null;

            // 設定新的資料來源
            dataGridView.DataSource = data;

            if (isAnalysisMode)
            {
                dataGridView.Columns["AccountName"].Visible = false;
                dataGridView.Columns["Date"].Visible = false;
                dataGridView.Columns["Picture1Path"].Visible = false;
                dataGridView.Columns["Picture2Path"].Visible = false;
                dataGridView.Columns["CompressedPicture1Path"].Visible = false;
                dataGridView.Columns["CompressedPicture2Path"].Visible = false;


                // 確定當前是按哪個分析類型分組
                bool isAccountTypeGrouping = data.All(x => x.AccountType != null && string.IsNullOrWhiteSpace(x.Detail) && string.IsNullOrWhiteSpace(x.PaymentMethod));
                bool isDetailGrouping = data.All(x => x.Detail != null && string.IsNullOrWhiteSpace(x.AccountType) && string.IsNullOrWhiteSpace(x.PaymentMethod));
                bool isPaymentMethodGrouping = data.All(x => x.PaymentMethod != null && string.IsNullOrWhiteSpace(x.AccountType) && string.IsNullOrWhiteSpace(x.Detail));

                // 根據分組類型調整顯示的欄位和標題
                if (isAccountTypeGrouping) // 按 "帳目類型" 分析
                {
                    dataGridView.Columns["Detail"].Visible = false;
                    dataGridView.Columns["PaymentMethod"].Visible = false;
                    dataGridView.Columns["AccountType"].HeaderText = "帳目類型";
                }
                else if (isDetailGrouping) // 按 "用途" 分析
                {
                    dataGridView.Columns["AccountType"].Visible = false;
                    dataGridView.Columns["PaymentMethod"].Visible = false;
                    dataGridView.Columns["Detail"].HeaderText = "用途";
                }
                else if (isPaymentMethodGrouping) // 按 "支付方式" 分析
                {
                    dataGridView.Columns["AccountType"].Visible = false;
                    dataGridView.Columns["Detail"].Visible = false;
                    dataGridView.Columns["PaymentMethod"].HeaderText = "支付方式";
                }
            }

        }
    }
    }
