using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳程式.Forms;
using 記帳系統.Models;

namespace 記帳程式.Components
{
    public partial class NavBar : UserControl
    {
        private FlowLayoutPanel buttonPanel;

        public NavBar()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            this.Controls.Add(buttonPanel);
            NavBarExtension.CreateButtons(buttonPanel, ChangePage);
        }

        private void ChangePage(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Enum.TryParse(clickedButton.Tag.ToString(), out FormType form);
            Form currentForm = SingletonForm.CreateForm(form);
            currentForm.Show();
        }

        private void NavBar_Load(object sender, EventArgs e)
        {

        }
    }
}
