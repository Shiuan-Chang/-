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
using CSVHelper;
using 記帳系統.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace 記帳系統.Forms
{
    [DisplayName("增一筆")]
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            List<string> accountNamelist = new List<string>()
            {
                "用餐",
                "交通",
                "租金",
                "治裝",
                "娛樂",
                "學習",
                "投資"
            };

            AccountNameBox.DataSource = accountNamelist;
            string imagePath = Path.Combine(Application.StartupPath, "Resources", "Images", "upload.png");
            pictureBox1.Load(imagePath);
            pictureBox2.Load(imagePath);


         
        }

        private void AccountDetail_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AccountNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AccountNameBox.SelectedValue.ToString())
            {
                case "用餐":
                    AccountTypeBox.DataSource = DropDownModel.GetFoodItems();
                    break;
                case "交通":
                    AccountTypeBox.DataSource = DropDownModel.GetTrafficItems();
                    break;
                case "租金":
                    AccountTypeBox.DataSource = DropDownModel.GetRentItems();
                    break;
                case "治裝":
                    AccountTypeBox.DataSource = DropDownModel.GetDressItems();
                    break;
                case "娛樂":
                    AccountTypeBox.DataSource = DropDownModel.GetEntertainmentItems();
                    break;
                case "學習":
                    AccountTypeBox.DataSource = DropDownModel.GetLearningItems();
                    break;
                case "投資":
                    AccountTypeBox.DataSource = DropDownModel.GetInvestmentItems();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //AccountingModel transaction = new AccountingModel
            //(
            //  DateBox.Value.ToString("yyyy-MM-dd HH:mm"),
            //  AccountNameBox.Text,
            //  AccountTypeBox.Text,
            //  DetailBox.Text,
            //  PaymentBox.Text,
            //  AmountBox.Text,
             
            ////圖檔上傳要另存並拿到圖檔路徑，不要拿原始路徑(設定好路徑再另存新黨)
            ////image 
            //  Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", $"{DateBox.Value.ToString("yyyy-MM-dd")}", $"{Guid.NewGuid()}.png"),
            //  Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", $"{DateBox.Value.ToString("yyyy-MM-dd")}", $"{Guid.NewGuid()}.png"),
            //  Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", $"{DateBox.Value.ToString("yyyy-MM-dd")}", $"{Guid.NewGuid()}.png"),
            //  Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", $"{DateBox.Value.ToString("yyyy-MM-dd")}", $"{Guid.NewGuid()}.png")
            //);
            //List<AccountingModel> transactions = new List<AccountingModel> { transaction };

            //// CSVHeler->CSV.Write & CSV.Read StreamWriter & StremReader 讀/寫CSV
            //string csvPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", $"{DateBox.Value.ToString("yyyy-MM-dd")}", $"data.csv");
            //string folderName = DateBox.Value.ToString("yyyy-MM-dd");
            //string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", folderName);
            //if( !Directory.Exists( folderPath )) { Directory.CreateDirectory( folderPath ); }
            //CSVHelper.CSV.WriteCSV(csvPath, transactions);


            //SaveCompressedImage(pictureBox1.Image, transaction.compressImagePath1, 50L);
            //SaveCompressedImage(pictureBox2.Image, transaction.compressImagePath2, 50L);

           
            //using (Bitmap image1 = new Bitmap(Image.FromFile(transaction.compressImagePath1), new Size(50, 50)))
            //{ image1.Save(transaction.csvImagePath1); }

            //using (Bitmap image2 = new Bitmap(Image.FromFile(transaction.compressImagePath2), new Size(50, 50)))
            //{ image2.Save(transaction.csvImagePath2); }

            //MessageBox.Show("已經成功上傳");
        }




        private void button2_Click(object sender, EventArgs e)
        {
            string csvReadPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", "csvDataPath", $"data.csv");
            List<AccountingModel> readList = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath,true);

        }

        private void UploadFile(object sender, EventArgs e)
        {
            // 上傳圖檔時就已經被壓縮，accountingmodel會新增2個縮圖參數
            PictureBox pictureBox = sender as PictureBox;   
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(openFileDialog.FileName);
                pictureBox.Load(openFileDialog.FileName);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            
        }
    }
}
//Guid.NewGuid() 是 C# 中的一個方法，用來生成一個全新的全局唯一識別符（GUID）。