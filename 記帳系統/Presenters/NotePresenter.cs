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
            UpdateDataGridView(lists);
            isLoading = false;

            //連貫model和view之間的溝通

        }
        private void UpdateDataGridView(List<AccountingModel> lists)
        {
            viewUpdater.ClearDataGridView();
            viewUpdater.SetDataSource(lists);
            SetupDataGridViewColumns(lists);
        }

        private void SetupDataGridViewColumns(List<AccountingModel> lists)
        {
            viewUpdater.HideColumns(new string[] { "compressImagePath1", "compressImagePath2", "csvImagePath1", "csvImagePath2" });

            viewUpdater.SetColumnHeaderText(0, "日期");
            viewUpdater.SetColumnHeaderText(3, "細項");
            viewUpdater.SetColumnHeaderText(4, "支付方式");
            viewUpdater.SetColumnHeaderText(5, "金額");

            // 帳盤名稱下拉選單
            viewUpdater.SetColumnHeaderText(1, "帳盤名稱");
            viewUpdater.SetColumnCellTemplate(1, new DataGridViewTextBoxCell());

            // 帳盤類型下拉選單
            viewUpdater.SetColumnHeaderText(2, "帳盤類型");
            viewUpdater.SetColumnCellTemplate(2, new DataGridViewTextBoxCell());

            viewUpdater.AddImageColumn("圖片一", DataGridViewImageCellLayout.Zoom);
            viewUpdater.AddImageColumn("圖片二", DataGridViewImageCellLayout.Zoom);
            viewUpdater.AddImageColumn("刪除", DataGridViewImageCellLayout.Zoom);

            if (lists.Count > 0)
            {
                for (int row = 0; row < lists.Count; row++)
                {
                    Bitmap bmp1 = new Bitmap(lists[row].csvImagePath1);
                    Bitmap bmp2 = new Bitmap(lists[row].csvImagePath2);
                    Bitmap junkImage = new Bitmap("C:\\CSharp練習\\記帳系統\\記帳系統\\Resources\\Images\\junk.png");
                    viewUpdater.SetRowImageValues(row, new Bitmap[] { bmp1, bmp2, junkImage });
                }
            }
        }
    }
}

