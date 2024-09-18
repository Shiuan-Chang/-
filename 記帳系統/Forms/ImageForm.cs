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
                this.pictureBox.Image = new Bitmap(imagePath);
            }
            else
            {
                MessageBox.Show("Image file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
