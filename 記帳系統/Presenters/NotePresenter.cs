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

            viewUpdater.DataGenerator(lists);

        }
        
    }
}

