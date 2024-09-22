using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
            InitializeComponent();
            dataGridView1.CellClick += imageClick;
            startPicker.Value = DateTime.Today.AddDays(-30);
            // timer要隔一段時間後，才去做button要做的事
            // button 永遠去呼叫debounce做的事情，因此，會有一個debounce方法，debouce會更改及清空timer的時間(也就是說timer一定會在debounce中)
        }


        private void NoteForm_Load(object sender, EventArgs e)
        {

        }

        //一開始把計時器歸零，然後再做延遲計算
        private void button1_Click(object sender, EventArgs e)
        {
            Timer debounceTimer = new Timer();

            debounceTimer.Tick += (s, args) =>
            {
                debounceTimer.Stop();//停止計時器防止多次觸發
                debounceTimer.Dispose(); // 釋放計時器資源
                LoadData();
            };

            debounceTimer.Interval = 1000; // 延遲時間
            debounceTimer.Start();
        }

        private void LoadData()
        {
            // 把datasource指向null
            //HW825:可以拿到起訖之間的資料，注意檢查之間日期是否有資料夾，沒有就跳過，否則會報錯
            // 拿到piker 日期相減的值(span)，檢查該日期期間是否存在資料夾逐一做檢查
            List<AccountingModel> Lists = new List<AccountingModel>();
            DateTime startDate = startPicker.Value;
            TimeSpan dateSpan = endPicker.Value - startPicker.Value;
            int timePeriod = dateSpan.Days;
            //找到路徑中日期存在相符的資料夾
            string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(csvSearchPath, startDate.AddDays(i).ToString("yyyy-MM-dd"), $"data.csv");
                if (File.Exists(folderPath))
                {
                    List<AccountingModel> periodList = CSVHelper.CSV.ReadCSV<AccountingModel>(folderPath, true);
                    Lists.AddRange(periodList);
                }
            }

            //為了做到記憶體管理
            dataGridView1.DataSource = null;

            // 清空所有columns的欄位
            dataGridView1.Columns.Clear();

            // 記憶體回收
            GC.Collect();

            //下方程式碼再載入時會造成過多記憶體的損耗以及增加讀取的時間，即使有記憶體回收仍會造成該狀況，因此因從源頭處理，把撈資料的期間限縮
            //string csvReadPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", "csvDataPath", $"data.csv");
            //List<AccountingModel> readList = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
            dataGridView1.DataSource = Lists;

            dataGridView1.Columns["compressImagePath1"].Visible = false;
            dataGridView1.Columns["compressImagePath2"].Visible = false;
            dataGridView1.Columns["csvImagePath1"].Visible = false;
            dataGridView1.Columns["csvImagePath2"].Visible = false;
            //DataGridViewImageColumn

            dataGridView1.Columns[0].HeaderText = "日期";
            dataGridView1.Columns[1].HeaderText = "帳目名稱";
            dataGridView1.Columns[2].HeaderText = "帳目類型";
            dataGridView1.Columns[3].HeaderText = "細項";
            dataGridView1.Columns[4].HeaderText = "支付方式";
            dataGridView1.Columns[5].HeaderText = "金額";

            DataGridViewImageColumn iconColumn1 = new DataGridViewImageColumn();
            DataGridViewImageColumn iconColumn2 = new DataGridViewImageColumn();
            iconColumn1.HeaderText = "圖片一";
            iconColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            iconColumn2.HeaderText = "圖片二";
            iconColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            dataGridView1.Columns.Insert(8, iconColumn1);
            dataGridView1.Columns.Insert(9, iconColumn2);

            string csvPath1 = "";

            if (dataGridView1.Rows.Count > 0)
            {
                // Print or store the first row's csvImagePath1 for debugging or other uses
                csvPath1 = Lists[0].compressImagePath1;
            }

            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                // 先去讀csvImagePath1,2的資料
                Bitmap bmp1 = new Bitmap(Lists[row].csvImagePath1);
                Bitmap bmp2 = new Bitmap(Lists[row].csvImagePath2);
                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[8]).Value = bmp1;
                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[9]).Value = bmp2;
                // 存四張圖，2張原圖縮小(50*50封面)並略調畫質，另外兩張點進去看到的是壓縮檔圖，約300-500kb

            }

        }


        // HW915:用form作控管，在每一次生命週期結束(關閉原圖視窗後)，應該要跟著回收記憶體(回到原來的值)。另外，addform也要把記憶體回收，
        // 可能會有回收不乾淨的行為(不能只用gc回收)，可能有其他地方造成gc處理不乾淨→一行行執行看哪一行程式碼造成記憶體增加→gc不知道要回收哪個
        private void imageClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8 || e.ColumnIndex == 9)
            {
                // Obtain the currently clicked AccountingModel object
                AccountingModel selectedImage = (AccountingModel)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                // Determine the correct image path based on the clicked column
                string imagePath = (e.ColumnIndex == 8) ? selectedImage.compressImagePath1 : selectedImage.compressImagePath2;

                // Display the image in a new ImageViewerForm
                ImageForm viewer = new ImageForm(imagePath);

                viewer.ShowDialog();

            }
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
