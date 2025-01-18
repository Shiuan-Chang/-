using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Contract;
using 記帳系統.Forms;
using 記帳系統.Models;
using 記帳系統.Repository;

namespace 記帳系統.Presenters
{
    public class NotePresenter : INotePresenter
    {
        private bool isLoading;
        private string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
        public INoteView noteView;
        public IRepository repository;
        public IMapper mapper;
        private AnalysisForm analysisForm;
        public INoteView analysisView;

        public NotePresenter(INoteView view, IMapper mapper)
        {
            noteView = view;
            repository = new CSVRepository();
            this.mapper = mapper;
        }

        public NotePresenter(INoteView view, AnalysisForm analysisForm, IMapper mapper)
        {
            this.analysisForm = analysisForm;
            analysisView = view;
            repository = new CSVRepository();
            this.mapper = mapper;
        }

        private List<NoteModel> ConvertToNoteModel(List<NotFormRawDataDAO> rawDataList)
        {
            return mapper.Map<List<NoteModel>>(rawDataList);
        }

        public void LoadData(DateTime startDate, DateTime endDate)
        {
            isLoading = true;
            var rawDataList = repository.GetDatasByDate(startDate, endDate);
            var noteModelList = ConvertToNoteModel(rawDataList);
            isLoading = false;
            //通知view的程式
            noteView.UpdateDataView(noteModelList);
        }

        //更改note上面的既存資料
        public void UpdateData(UpdateNoteModel model)
        {
            string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", model.NoteDate);
            string csvReadPath = Path.Combine(folderPath, $"data.csv");

            List<AccountingModel> list = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);
            foreach (AccountingModel item in list)
            {
                if (item.date.ToString() == model.NoteHour)
                {
                    switch (model.ColumnIndex)
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

            this.noteView.Reload();
        }

        public void DeleteData(DeleteNoteModel model)
        {
            bool isDeleted = repository.RemoveData(model.DeleteDataDate);
            if (isDeleted)
            {
                // 更新視圖
                noteView.Reload();
            }
        }
    }
}

//DateTime targetDate = DateTime.Parse(model.NoteDate);
//string folderPath = Path.Combine(@"C:\Users\icewi\OneDrive\桌面\testCSV", targetDate.ToString("yyyy-MM-dd"));
//string csvReadPath = Path.Combine(folderPath, "data.csv");

//if (File.Exists(csvReadPath))
//{                
//    List<AccountingModel> list = CSVHelper.CSV.ReadCSV<AccountingModel>(csvReadPath, true);

//    list.RemoveAll(item =>
//    {
//        DateTime itemDate;
//        if (DateTime.TryParse(item.date, out itemDate))
//        {
//            return itemDate.Date == targetDate.Date;
//        }
//        return false;
//    });

//    // 寫回 CSV 文件
//    if (list.Count > 0)
//    {
//        CSVHelper.CSV.WriteCSV(csvReadPath, list);
//    }

//    // 更新 view
//    this.noteView.Reload();
//}

