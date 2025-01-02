using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using 記帳系統.Utility;
using CSVHelper;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace 記帳系統.Repository
{
    // 只留下新增、修改、刪除、群組資料
    public class CSVRepository : IRepository
    {
        // Add Form 使用
        public bool AddData(AddFormRawDataDAO dao)
        {
            if (!DateTime.TryParse(dao.Date, out var parsedDate))
            {
                throw new FormatException($"Invalid date format: {dao.Date}");
            }

            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
            string baseFolderPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";

            // 建立資料夾
            string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", formattedDate);

            // 設定 CSV 檔案路徑
            string filePath = Path.Combine(folderPath, "data.csv");

            // 寫入到 CSV
            CSVHelper.CSV.WriteCSV(filePath, new List<AddFormRawDataDAO> { dao });
            return true;
        }

        // NoteForm 日期資料選取
        public List<NotFormRawDataDAO> GetDatasByDate(DateTime start, DateTime end)
        {
            List<NotFormRawDataDAO> lists = new List<NotFormRawDataDAO>();
            TimeSpan dateSpan = end - start;
            int timePeriod = dateSpan.Days;

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", start.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<NotFormRawDataDAO> periodList = CSVHelper.CSV.ReadCSV<NotFormRawDataDAO>(folderPath, true);
                    lists.AddRange(periodList);
                }
            }
            return lists;
        }

        // AccountForm 日期資料選取
        public List<AccountRawDataDAO> accountFormGetDatasByDate(DateTime start, DateTime end)
        {
            List<AccountRawDataDAO> lists = new List<AccountRawDataDAO>();
            TimeSpan dateSpan = end - start;
            int timePeriod = dateSpan.Days;

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", start.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<AccountRawDataDAO> periodList = CSVHelper.CSV.ReadCSV<AccountRawDataDAO>(folderPath, true);
                    lists.AddRange(periodList);
                }
            }
            return lists;
        }

        public bool ModifyData(RawData data)
        {
            return true;
        }

        // NoteForm 使用
        public bool RemoveData(string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                throw new FormatException($"Invalid date format: {date}");
            }

            string folderPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
            bool dataRemoved = false;

            if (Directory.Exists(folderPath))
            {
                var dataFolders = Directory.GetDirectories(folderPath); // 找到所有日期資料夾
                foreach (var dataFolder in dataFolders)
                {
                    string filePath = Path.Combine(dataFolder, "data.csv");
                    if (File.Exists(filePath))
                    {
                        // 讀取 CSV 中的所有資料
                        List<NotFormRawDataDAO> folderData = CSVHelper.CSV.ReadCSV<NotFormRawDataDAO>(filePath, true);

                        // 過濾出不匹配的資料
                        int initialCount = folderData.Count;
                        folderData.RemoveAll(item =>
                        {
                            if (DateTime.TryParse(item.Date, out var itemDate))
                            {
                                Console.WriteLine($"Item Date: {item.Date}, Parsed Date: {parsedDate.Date}");
                                return itemDate.Date == parsedDate.Date; // 刪除匹配的日期資料
                            }
                            return false;
                        });

                        // 如果資料被修改（有資料被刪除）
                        if (folderData.Count < initialCount)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Truncate))
                            {
                                fileStream.SetLength(0); // 清空文件
                            }

                            // 寫入更新後的資料
                            CSVHelper.CSV.WriteCSV(filePath, folderData);
                            Console.WriteLine($"File updated: {filePath}");
                            dataRemoved = true;
                        }
                    }
                }
            }
            return dataRemoved;
        }

        // piechart使用的資料組

        public List<AnalysisRawDataDAO> GetPieChartDatas(DateTime start, DateTime end)
        {
            List<AnalysisRawDataDAO> lists = new List<AnalysisRawDataDAO>();
            TimeSpan dateSpan = end - start;
            int timePeriod = dateSpan.Days;

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", start.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<AnalysisRawDataDAO> periodList = CSVHelper.CSV.ReadCSV<AnalysisRawDataDAO>(folderPath, true);
                    lists.AddRange(periodList);
                }
            }
            return lists;
        }
    }
}

//public void SaveData(AddModel model)
//{
//    string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", model.Date.ToString("yyyy-MM-dd"));
//    if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

//    AccountingModel transaction = new AccountingModel
//    (
//        model.Date.ToString("yyyy-MM-dd HH:mm"),
//        model.SelectedAccountName,
//        model.SelectedAccountType,
//        model.Detail,
//        model.Payment,
//        model.Amount,
//        Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
//        Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
//        Path.Combine(folderPath, $"{Guid.NewGuid()}.png"),
//        Path.Combine(folderPath, $"{Guid.NewGuid()}.png")
//    );

//    List<AccountingModel> transactions = new List<AccountingModel> { transaction };


// 壓縮應該要在presenter做
//    using (Bitmap compressedImage1 = ImageCompressionUtility.CompressImage(model.Picture1, 50L))
//    {
//        if (File.Exists(transaction.compressImagePath1))
//        {
//            File.Delete(transaction.compressImagePath1);
//        }
//        compressedImage1.Save(transaction.compressImagePath1, ImageFormat.Jpeg);
//    }

//    using (Bitmap compressedImage2 = ImageCompressionUtility.CompressImage(model.Picture2, 50L))
//    {
//        if (File.Exists(transaction.compressImagePath2))
//        {
//            File.Delete(transaction.compressImagePath2);
//        }
//        compressedImage2.Save(transaction.compressImagePath2, ImageFormat.Jpeg);
//    }
//}