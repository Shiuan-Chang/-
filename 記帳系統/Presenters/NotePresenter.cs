using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Contract;
using 記帳系統.Models;

namespace 記帳系統.Presenters
{
    public class NotePresenter : INotePresenter
    {
        private bool isLoading;
        private string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
        private INoteView viewUpdater;

        public NotePresenter(INoteView updater)
        {
            viewUpdater = updater;
        }

        public void LoadData(DateTime startDate, DateTime endDate)
        {
            isLoading = true;
            List<AccountingModel> lists = new List<AccountingModel>();
            TimeSpan dateSpan = endDate - startDate;
            int timePeriod = dateSpan.Days;

            for (int i = 0; i <= timePeriod; i++)
            {
                string folderPath = Path.Combine(csvSearchPath, startDate.AddDays(i).ToString("yyyy-MM-dd"), "data.csv");
                if (File.Exists(folderPath))
                {
                    List<AccountingModel> periodList = CSVHelper.CSV.ReadCSV<AccountingModel>(folderPath, true);
                    lists.AddRange(periodList);
                }
            }
            isLoading = false;
            //通知view的程式
            viewUpdater.UpdateDataView(lists);

        }
        public void UpdateData(UpdateNoteModel model)
        {
            string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", model.NoteDate);
            string csvReadPath = Path.Combine(folderPath, $"data.csv");

            List<AccountingModel> list = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
            foreach (AccountingModel item in list)
            {
                if (item.date.ToString() == model.NoteHour)
                {
                    switch(model.ColumnIndex)
                    {
                        case 1:
                            item.accountName = model.UpdateData;
                            break;
                        case 2:
                            item.accountType = model.UpdateData;
                            break;
                        case 3:
                            item.detail = model.UpdateData;
                            break;
                        case 5:
                            item.amount = model.UpdateData;
                            break;
                    }
                }
            }
            File.Delete(csvReadPath);
            CSVHelper.CSV.WriteCSV(csvReadPath, list);

            this.viewUpdater.Reload();
        }

        public void DeleteData(DeleteNoteModel model)
        {
            DateTime targetDate = DateTime.Parse(model.NoteDate);
            string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", targetDate.ToString("yyyy-MM-dd"));
            string csvReadPath = Path.Combine(folderPath, "data.csv");

            if (File.Exists(csvReadPath))
            {                
                List<AccountingModel> list = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
            
                list.RemoveAll(item =>
                {
                    DateTime itemDate;
                    if (DateTime.TryParse(item.date, out itemDate))
                    {
                        return itemDate.Date == targetDate.Date;
                    }
                    return false;
                });

                // 寫回 CSV 文件
                if (list.Count > 0)
                {
                    CSVHelper.CSV.WriteCSV(csvReadPath, list);
                }

                // 更新 view
                this.viewUpdater.Reload();
            }
        }
    }
}

