using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 記帳程式.Components
{
    internal class NavBarExtension
    {
        //反射方式，根據有多少Form建立對應的NavBar button
        public static void CreateButtons(FlowLayoutPanel buttonPanel, EventHandler eventHandler)
        {
            var formTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Form)));

            foreach (var type in formTypes)
            {
                var form = (Form)Activator.CreateInstance(type);

                Button navButton = new Button
                {
                    Width = 105,
                    Height = 105,
                    Text = form.Text,
                };

                string imagePath = Path.Combine(Application.StartupPath, "Resources", "Images", type.Name + ".png");
                if (File.Exists(imagePath))
                {
                    navButton.Image = Image.FromFile(imagePath);
                    navButton.TextAlign = ContentAlignment.BottomCenter;
                }

                navButton.Text = type.Name;
                navButton.Click += eventHandler;
                buttonPanel.AutoSize = true;
                buttonPanel.Controls.Add(navButton);
            }
        }
    }

}
