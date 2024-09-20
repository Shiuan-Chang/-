using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 記帳系統.Forms
{
    [DisplayName("圖片視窗")]
    public partial class ImageForm : Form
    {
        private PictureBox pictureBox;
        private Bitmap image;
        public ImageForm(string imagePath)
        {
            InitializeComponent();
            this.pictureBox = new PictureBox();
            // Configure PictureBox
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(this.pictureBox);

            // Configure Form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 600);
            LoadImage(imagePath);
        }


        private void LoadImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                this.image = new Bitmap(imagePath);
                this.pictureBox.Image = this.image;
            }
            else
            {
                MessageBox.Show("Image file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //覆寫基類（Form）的 OnFormClosed 方法，以便實現特定的清理或其他終止操作。
        protected override void OnFormClosed(FormClosedEventArgs e) // onformcolsed是處理視窗關閉時觸發的事件
        {
            //將 PictureBox 的圖片返回null
            pictureBox.Image = null;

            //釋放圖檔
            image?.Dispose();
            image = null;

            //進行回收
            GC.Collect();
            base.OnFormClosed(e);
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
