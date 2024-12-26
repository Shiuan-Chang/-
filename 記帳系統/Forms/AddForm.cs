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
using AutoMapper;
using CSVHelper;
using 記帳系統.Contract;
using 記帳系統.Mappings;
using 記帳系統.Models;
using 記帳系統.Presenters;
using 記帳系統.Repository;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace 記帳系統.Forms
{
    [DisplayName("增一筆")]
    public partial class AddForm : Form, IAddView
    {
        private AddPresenter addPresenter;
        public AddForm()
        {
            InitializeComponent();
            // 初始化 IRepository 和 IMapper
            IRepository repository = new CSVRepository();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = config.CreateMapper();

            // 將實例傳遞給 AddPresenter
            addPresenter = new AddPresenter(this, repository, mapper);
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

        public void ResetForm()
        {
            AccountNameBox.SelectedIndex = 0 ;
            AccountTypeBox.SelectedIndex = 0;
            DetailBox.Text = "";
            PaymentBox.SelectedIndex = 0;
            AmountBox.Text = "";
            DateBox.Value = DateTime.Now ;
            pictureBox1.Image = null;
            pictureBox2.Image= null;
        }

        public void ShowMessage(string message) 
        {
            MessageBox.Show("已成功上傳");
            ResetForm();
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
            AddModel addModel = new AddModel();
            addModel.SelectedAccountType = AccountTypeBox.SelectedItem?.ToString();
            addModel.SelectedAccountName= AccountNameBox.SelectedItem?.ToString();
            addModel.Detail = DetailBox.Text;
            addModel.Date = DateBox.Value;
            addModel.Payment = PaymentBox.Text;
            addModel.Amount = AmountBox.Text;
            addModel.Picture1 = pictureBox1.Image;
            addModel.Picture2 = pictureBox2.Image;
            this.Debounce(() => addPresenter.SaveData(addModel), 1000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string csvReadPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", "csvDataPath", $"data.csv");
            List<AccountingModel> readList = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath,true);

        }

        private void UploadFile(object sender, EventArgs e)
        {
            // 確保 sender 是 PictureBox
            if (sender is PictureBox pictureBox)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.Image = image;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }
    }
}
//Guid.NewGuid() 是 C# 中的一個方法，用來生成一個全新的全局唯一識別符（GUID）。