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
        // 
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



        public List<RawData> GetDatasByDate(DateTime start, DateTime end)
        {
            List<RawData> lists = new List<RawData>();
            TimeSpan dateSpan = end - start;
            int timePeriod = dateSpan.Days;

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", start.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<RawData> periodList = CSVHelper.CSV.ReadCSV<RawData>(folderPath, true);
                    lists.AddRange(periodList);
                }
            }
            return lists;
        }

        // GroupingData 應該要包含conditionTypes、analyzeTypes以及lists(就是rawData)


        public List<GroupingData> GetGroupByDatas(GroupbyModel model)
        {
            // 过滤数据
            var filteredData = model.ConditionTypes.Count > 0 ? model.RawData.Where(item => model.ConditionTypes.Contains(item.accountType)).ToList() : model.RawData;

      
            bool isAnalysisMode = model.AnalyzeTypes.Count > 0;
            List<GroupingData> groupedData;
            if (isAnalysisMode)
            {
                groupedData = filteredData.GroupBy(item =>
                    model.AnalyzeTypes.Contains("帳目類型") ? item.accountType :
                    model.AnalyzeTypes.Contains("用途") ? item.detail :
                    model.AnalyzeTypes.Contains("支付方式") ? item.paymentMethod : null)
                .Select(group => new GroupingData
                {
                    AccountType = model.AnalyzeTypes.Contains("帳目類型") ? group.Key : null,
                    Detail = model.AnalyzeTypes.Contains("用途") ? group.Key : null,
                    PaymentMethod = model.AnalyzeTypes.Contains("支付方式") ? group.Key : null,
                    Amount = group.Sum(x => int.Parse(x.amount)).ToString() // 注意，这里假设 amount 已经是数值类型，如果是字符串需要先转换
                }).ToList();
            }
            else
            {
                groupedData = filteredData.Select(item => new GroupingData
                {
                    AccountType = item.accountType,
                    Detail = item.detail,
                    PaymentMethod = item.paymentMethod,
                    Amount = item.amount
                }).ToList();
            }

            return groupedData;
        }

        public bool ModifyData(RawData data)
        {

            return true;
        }

        public bool RemoveData(string date)
        {
            return true;
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