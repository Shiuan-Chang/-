using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Models;


namespace 記帳系統.Forms
{
    // 了解一下記憶體外洩以及下載3G，5G高清圖片

    [DisplayName("筆記本")]
    public partial class NoteForm : Form
    {
        public NoteForm()
        {
            // timer要隔一段時間後，才去做button要做的事
            // button 永遠去呼叫debounce做的事情，因此，會有一個debounce方法，debouce會更改及清空timer的時間(也就是說timer一定會在debounce中)
            InitializeComponent();
            dataGridView1.CellClick += CellClick;
            startPicker.Value = DateTime.Today.AddDays(-30);
        }

        private void NoteForm_Load(object sender, EventArgs e)
        {

        }

        //設定全局只有一個定時器存在
        private static System.Threading.Timer debounceTimer;
        private static object timerLock = new object();
        private bool isLoading = false;

        //一開始把計時器歸零，然後再做延遲計算
        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(LoadData, 1000);
        }

        private void LoadData()
        {
            isLoading = true;
            List<AccountingModel> Lists = new List<AccountingModel>();
            DateTime startDate = startPicker.Value;
            TimeSpan dateSpan = endPicker.Value - startPicker.Value;
            int timePeriod = dateSpan.Days;
            string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(csvSearchPath, startDate.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<AccountingModel> periodList = CSVHelper.CSV.ReadCSV<AccountingModel>(folderPath, true);
                    Lists.AddRange(periodList);
                }
            }
            UpdateDataGridView(Lists);
            isLoading = false;
        }


        private DataGridViewComboBoxCell GetAccountTypeComboBox(string accountName)
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


        private void UpdateDataGridView(List<AccountingModel> lists)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();

            dataGridView1.DataSource = lists;
            SetupDataGridViewColumns(lists);
        }

        private void SetupDataGridViewColumns(List<AccountingModel> lists)
        {
            // Configure columns here
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
            dataGridView1.Columns.Insert(10, iconColumn3);

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
                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[10]).Value = junkImage;
                // 存四張圖，2張原圖縮小(50*50封面)並略調畫質，另外兩張點進去看到的是壓縮檔圖，約300-500kb
            }
        }

        // 用form作控管，在每一次生命週期結束(關閉原圖視窗後)，應該要跟著回收記憶體(回到原來的值)。另外，addform也要把記憶體回收，
        // 可能會有回收不乾淨的行為(不能只用gc回收)，可能有其他地方造成gc處理不乾淨→一行行執行看哪一行程式碼造成記憶體增加→gc不知道要回收哪個


        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8 || e.ColumnIndex == 9)
            {              
                AccountingModel selectedImage = (AccountingModel)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                string imagePath = (e.ColumnIndex == 8) ? selectedImage.compressImagePath1 : selectedImage.compressImagePath2;
                ImageForm viewer = new ImageForm(imagePath);
                viewer.ShowDialog();
            }

            if (e.ColumnIndex == 10)
            {
                // Get the date from the DataGridView and construct the path for the CSV file.
                string date = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()).ToString("yyyy-MM-dd");
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", date);
                string csvReadPath = Path.Combine(folderPath, "data.csv");

                // Read the CSV file into a list of AccountingModel.
                List<AccountingModel> list = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);

                // Remove the item at the specified index.
                if (list.Count > e.RowIndex)  // Ensure the index is valid.
                {
                    list.RemoveAt(e.RowIndex);
                    CSVHelper.CSV.WriteCSV(csvReadPath, list);  // Write the updated list back to the CSV.
                }

                // Optionally delete the entire folder if it's now empty.
                if (list.Count == 0 && Directory.Exists(folderPath))
                {
                    System.Threading.Thread.Sleep(1000);
                    Directory.Delete(folderPath, true);  // Delete the folder and all contents.
                }

                UpdateDataGridView(list);
            }
        }

        // 創造項目名稱的選單
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1) 
            {
                DataGridViewComboBoxCell accountName = new DataGridViewComboBoxCell();
                accountName.DataSource = new List<string> { "用餐", "交通", "租金", "治裝", "娛樂", "學習", "投資" }; // Your dropdown options
                dataGridView1.Rows[e.RowIndex].Cells[1] = accountName; 
            }
        }        

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading) return; // 避免初次加載時觸發

            if (e.ColumnIndex == 1) // 帳目名稱欄位
            {
                string accountName = dataGridView1.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                DataGridViewComboBoxCell accountTypeCell = GetAccountTypeComboBox(accountName);

                dataGridView1.Rows[e.RowIndex].Cells[2] = accountTypeCell;
                if (accountTypeCell.Items.Count > 0)
                {
                    accountTypeCell.Value = accountTypeCell.Items[0]; // 確保 Items 不為空
                }
                else
                {
                    accountTypeCell.Value = null; // 或其他適當處理
                }
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) 
            {
                DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell
                {
                    Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                };
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] = textBoxCell; 
            }

            string endEditData = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //string dateTime = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Split(' ')[0];//結果同285行
            string date = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()).ToString("yyyy-MM-dd");
            string dateWithHours = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void navBar1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void endPicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void startPicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

//lock (timerLock)
//{
//    // 如果計時器已經存在，則更改其計時延遲
//    if (debounceTimer != null)
//    {
//        debounceTimer.Change(1000, Timeout.Infinite); 
//        return; // 结束方法
//    }

//    // 如果計時器不存在，則創建一個新的計時器
//    debounceTimer = new System.Threading.Timer(LoadData, null, 1000, Timeout.Infinite);
//}

//不要這樣用，這樣會有很多個timer
//Timer debounceTimer = new Timer();

//debounceTimer.Tick += (s, args) =>
//{
//    debounceTimer.Stop();//停止計時器防止多次觸發
//    debounceTimer.Dispose(); // 釋放計時器資源
//    LoadData();
//};

//debounceTimer.Interval = 1000; // 延遲時間
//debounceTimer.Start();

//private void LoadData(object state)
//{
//    var form = (NoteForm)Application.OpenForms["NoteForm"];
//    // 把datasource指向null
//    //HW825:可以拿到起訖之間的資料，注意檢查之間日期是否有資料夾，沒有就跳過，否則會報錯
//    // 拿到piker 日期相減的值(span)，檢查該日期期間是否存在資料夾逐一做檢查
//    if (form != null)
//    {
//        form.Invoke(new Action(() =>
//        {
//            List<AccountingModel> Lists = new List<AccountingModel>();
//            DateTime startDate = startPicker.Value;
//            TimeSpan dateSpan = endPicker.Value - startPicker.Value;
//            int timePeriod = dateSpan.Days;
//            //找到路徑中日期存在相符的資料夾
//            string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
//            for (int i = 0; i <= timePeriod; i++)
//            {
//                string folderPath = Path.Combine(csvSearchPath, startDate.AddDays(i).ToString("yyyy-MM-dd"), $"data.csv");
//                if (File.Exists(folderPath))
//                {
//                    List<AccountingModel> periodList = CSVHelper.CSV.ReadCSV<AccountingModel>(folderPath, true);
//                    Lists.AddRange(periodList);
//                }
//            }

//            //為了做到記憶體管理
//            dataGridView1.DataSource = null;

//            // 清空所有columns的欄位
//            dataGridView1.Columns.Clear();

//            // 記憶體回收
//            GC.Collect();

//            //下方程式碼再載入時會造成過多記憶體的損耗以及增加讀取的時間，即使有記憶體回收仍會造成該狀況，因此因從源頭處理，把撈資料的期間限縮
//            //string csvReadPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", "csvDataPath", $"data.csv");
//            //List<AccountingModel> readList = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
//            dataGridView1.DataSource = Lists;

//            dataGridView1.Columns["compressImagePath1"].Visible = false;
//            dataGridView1.Columns["compressImagePath2"].Visible = false;
//            dataGridView1.Columns["csvImagePath1"].Visible = false;
//            dataGridView1.Columns["csvImagePath2"].Visible = false;
//            //DataGridViewImageColumn

//            dataGridView1.Columns[0].HeaderText = "日期";
//            dataGridView1.Columns[1].HeaderText = "帳目名稱";
//            dataGridView1.Columns[2].HeaderText = "帳目類型";
//            dataGridView1.Columns[3].HeaderText = "細項";
//            dataGridView1.Columns[4].HeaderText = "支付方式";
//            dataGridView1.Columns[5].HeaderText = "金額";

//            DataGridViewImageColumn iconColumn1 = new DataGridViewImageColumn();
//            DataGridViewImageColumn iconColumn2 = new DataGridViewImageColumn();
//            iconColumn1.HeaderText = "圖片一";
//            iconColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;

//            iconColumn2.HeaderText = "圖片二";
//            iconColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom;

//            dataGridView1.Columns.Insert(8, iconColumn1);
//            dataGridView1.Columns.Insert(9, iconColumn2);

//            string csvPath1 = "";

//            if (dataGridView1.Rows.Count > 0)
//            {
//                // Print or store the first row's csvImagePath1 for debugging or other uses
//                csvPath1 = Lists[0].compressImagePath1;
//            }

//            for (int row = 0; row < dataGridView1.Rows.Count; row++)
//            {
//                // 先去讀csvImagePath1,2的資料
//                Bitmap bmp1 = new Bitmap(Lists[row].csvImagePath1);
//                Bitmap bmp2 = new Bitmap(Lists[row].csvImagePath2);
//                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[8]).Value = bmp1;
//                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[9]).Value = bmp2;
//                // 存四張圖，2張原圖縮小(50*50封面)並略調畫質，另外兩張點進去看到的是壓縮檔圖，約300-500kb
//            }
//        }));
//    }
//}