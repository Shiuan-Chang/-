using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Contract;
using 記帳系統.Forms;
using 記帳系統.Models;
using 記帳系統.Repository;
using 記帳系統.Utility;

namespace 記帳系統.Presenters
{
    public class AddPresenter : IAddPresenter
    {
        private IAddView addView;

        public AddPresenter(IAddView view)
        {
            addView = view;
        }

        public void Initialize()
        {
            // 獲取 AccountName 列表
            List<string> accountNames = DropdownRepository.GetAccountNames();

        }

        public void SaveData(AddModel model) 
        {
            string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", model.Date.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

            AccountingModel transaction = new AccountingModel
            (
                model.Date.ToString("yyyy-MM-dd HH:mm"),
                model.SelectedAccountName,
                model.SelectedAccountType,
                model.Detail,
                model.Payment,
                model.Amount,
                Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
                Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
                Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
                Path.Combine(folderPath, $"{Guid.NewGuid()}.png")
            );

            List<AccountingModel> transactions = new List<AccountingModel> { transaction };

            using (Bitmap compressedImage1 = ImageCompressionUtility.CompressImage(model.Picture1, 50L))
            {
                if (File.Exists(transaction.compressImagePath1))
                {
                    File.Delete(transaction.compressImagePath1);
                }
                compressedImage1.Save(transaction.compressImagePath1, ImageFormat.Jpeg);
            }

            using (Bitmap compressedImage2 = ImageCompressionUtility.CompressImage(model.Picture2, 50L))
            {
                if (File.Exists(transaction.compressImagePath2))
                {
                    File.Delete(transaction.compressImagePath2);
                }
                compressedImage2.Save(transaction.compressImagePath2, ImageFormat.Jpeg);
            }

            this.addView.ShowMessage();

        }
    }
}
