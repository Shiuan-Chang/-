    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using 記帳系統.Models;

    namespace 記帳系統.DataGridViewExtension
    {
        // 改完做addform MVP，用擴充完成表格顯示功能

        public static class DataGridViewExtension
        {
            public static void SetupDataColumns(this DataGridView dataGridView1,List<AccountingModel> lists)
            {
                dataGridView1.Columns["compressImagePath1"].Visible = false;
                dataGridView1.Columns["compressImagePath2"].Visible = false;
                dataGridView1.Columns["csvImagePath1"].Visible = false;
                dataGridView1.Columns["csvImagePath2"].Visible = false;

                dataGridView1.Columns[0].HeaderText = "日期";
                dataGridView1.Columns[3].HeaderText = "細項";
                dataGridView1.Columns[4].HeaderText = "支付方式";
                dataGridView1.Columns[5].HeaderText = "金額";

                // 帳目名稱下拉選單
                dataGridView1.Columns[1].HeaderText = "帳目名稱";
                dataGridView1.Columns[1].CellTemplate = new DataGridViewTextBoxCell();

                // 帳目類型下拉選單
                dataGridView1.Columns[2].HeaderText = "帳目類型";
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

                string csvPath1 = "";

                if (dataGridView1.Rows.Count > 0)
                {
                    csvPath1 = lists[0].compressImagePath1;
                }

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    // 先去讀csvImagePath1,2的資料
                    Bitmap bmp1 = new Bitmap(lists[row].csvImagePath1);
                    Bitmap bmp2 = new Bitmap(lists[row].csvImagePath2);
                    Bitmap junkImage = new Bitmap("C:\\CSharp練習\\記帳系統\\記帳系統\\Resources\\Images\\junk.png");
                    ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[8]).Value = bmp1;
                    ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[9]).Value = bmp2;
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
            public static void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {
                    DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell
                    {
                        Value = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                    };
                    dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] = textBoxCell;
                }

                string endEditData = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //string dateTime = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Split(' ')[0];//結果同285行
                string date = DateTime.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()).ToString("yyyy-MM-dd");
                string dateWithHours = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", date);
                string csvReadPath = Path.Combine(folderPath, $"data.csv");

                List<AccountingModel> List = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
                foreach (AccountingModel item in List)
                {
                    if (item.date.ToString() == dateWithHours)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            item.accountName = endEditData;
                        }
                        if (e.ColumnIndex == 2)
                        {
                            item.accountType = endEditData;
                        }

                        if (e.ColumnIndex == 5)
                        {
                            item.amount = endEditData;
                        }
                    }
                }

                File.Delete(csvReadPath);
                CSVHelper.CSV.WriteCSV(csvReadPath, List);
            }
        }
    }
