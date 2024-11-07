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
using 記帳系統.Models;

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
            List<string> accountNamelist = new List<string>
        {
            "用餐",
            "交通",
            "租金",
            "治裝",
            "娛樂",
            "學習",
            "投資"
        };
            addView.AccountNameList = accountNamelist;
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

            // 保存 CSV 檔案
            string csvPath = Path.Combine(folderPath, "data.csv");
            CSVHelper.CSV.WriteCSV(csvPath, transactions);

            // 壓縮並保存圖片
            SaveCompressedImage(model.Picture1, transaction.compressImagePath1, 50L);
            SaveCompressedImage(model.Picture2, transaction.compressImagePath2, 50L);

            // 保存縮圖
            using (Bitmap image1 = new Bitmap(Image.FromFile(transaction.compressImagePath1), new Size(50, 50)))
            {
                image1.Save(transaction.csvImagePath1);
            }

            using (Bitmap image2 = new Bitmap(Image.FromFile(transaction.compressImagePath2), new Size(50, 50)))
            {
                image2.Save(transaction.csvImagePath2);
            }

            addView.ShowMessage("已經成功上傳");
            addView.ResetForm();
        }

        private void SaveCompressedImage(Image image, string outputPath, long quality)//long quality 壓縮品質，0(最低品質，最高壓縮)-100(最高品質，最低壓縮)
        {

            // Get a JPEG codec
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);//根據傳入的圖片格式（在這個例子中是 JPEG），返回對應的編碼器。

            // Create an Encoder object based on the Quality parameter category
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;//對應於品質調整的編碼器參數

            // Create an EncoderParameters object
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level set by 'quality' parameter
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            // Save the image to the specified path
            image.Save(outputPath, jpgEncoder, myEncoderParameters);//.Save()進行格式轉換，所以png依然可以轉換後保留成為png，而非jpeg
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            // Return the codec with the corresponding format
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }




    }
    
}
