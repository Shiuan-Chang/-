using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Forms;
using 記帳系統.Models;

namespace 記帳系統.Components
{
    public partial class NavBar : UserControl
    {

        //獨體模式：透過static，讓instance去控制，而不是一直new instance
        public NavBar()
        {
            InitializeComponent();
        }


        private void ChangePage(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Enum.TryParse(clickedButton.Tag.ToString(), out FormType form);
            Form currentForm = SingletonForm.CreateForm(form);
            currentForm.Show();
        }

        // 資深工程師：透過泛型、反射、委派撰寫library
        private void NavBar_Load(object sender, EventArgs e)
        {
            // 反射，抓到每個表單
            var formTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Form)));
            int count = 0;

            foreach (var type in formTypes)
            {
                if (type.Name == "ImageForm")
                    continue;

                count++;

                var titleAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
                string formTitle = titleAttribute?.DisplayName ?? "未命名";

                Button navButton = new Button
                {
                    Width = 100,
                    Height = 110,
                    Tag = type.Name,
                    TextImageRelation = TextImageRelation.ImageAboveText, // 圖片在文字上方
                    Padding = new Padding(0, 5, 0, 0), // 增加文字與圖片之間的距離
                    Text = formTitle,
                    Font = new Font("Microsoft JhengHei", 12, FontStyle.Regular)
                };

                string imagePath = Path.Combine(Application.StartupPath, "Resources", "Images", type.Name + ".png");

                if (File.Exists(imagePath))
                {
                    navButton.Image = Image.FromFile(imagePath);
                    navButton.TextAlign = ContentAlignment.BottomCenter;
                }
                navButton.Click += ChangePage;
                flowLayoutPanel1.Controls.Add(navButton);

                if (SingletonForm.CurrentFormType() == type.Name)
                {
                    navButton.Enabled = false;
                }
            }

            this.Width = count * 110;
            this.flowLayoutPanel1.Width = this.Width;
        }
         
        // 
        public void DisableButton(FormType formType)
        {
           foreach(Button button in this.flowLayoutPanel1.Controls)
            {
                if(button.Tag.ToString() == formType.ToString())
                {
                    button.Enabled = false;
                }
            }
                
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
