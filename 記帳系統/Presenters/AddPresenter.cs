using AutoMapper;
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
        public IAddView addView;
        public IRepository repository;
        public IMapper mapper;

        public AddPresenter(IAddView view, IRepository repository, IMapper mapper)
        {
            addView = view;
            this.repository = repository;
            this.mapper = mapper;
        }

        // presenter 這邊主要負責處理商業邏輯、資料轉換(DTO，DAO之間的轉換)

        public void SaveData(AddModel model)
        {
            var dto = mapper.Map<AddFormRawDataDTO>(model);

            string baseFolderPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";

            // 如果 dto.Date 是 DateTime，直接格式化
            string formattedDate = dto.Date.ToString("yyyy-MM-dd");
            string folderPath = Path.Combine(baseFolderPath, formattedDate);


            string picture1Path = Utility.SaveImage.SaveCompressedImage(dto.Picture1, baseFolderPath, formattedDate, $"Picture1_{Guid.NewGuid()}.jpg", 50L);
            string picture2Path = Utility.SaveImage.SaveCompressedImage(dto.Picture2, baseFolderPath, formattedDate, $"Picture2_{Guid.NewGuid()}.jpg", 50L);


            // 將圖片路徑更新到 DAO
            var dao = mapper.Map<AddFormRawDataDAO>(dto);
            dao.Picture1Path = picture1Path;
            dao.Picture2Path = picture2Path;


            // 儲存資料到儲存庫
            bool result = repository.AddData(dao);

            // 返回操作結果給前端
            if (result)
            {
                addView.ShowMessage("儲存成功");
            }
        }
    }
}
