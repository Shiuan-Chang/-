using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Utility
{
    public class SaveImage
    {
        public static string SaveCompressedImage(Image image, string baseFolderPath, string dateString, string fileName, long quality)
        {
            if (image == null) return null;

            // 檢查日期格式是否合法
            if (!DateTime.TryParse(dateString, out var parsedDate))
            {
                throw new FormatException($"Invalid date format: {dateString}");
            }

            // 根據日期建立對應的資料夾
            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
            string folderPath = Path.Combine(baseFolderPath, formattedDate);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 設定圖片檔案路徑
            string filePath = Path.Combine(folderPath, fileName);

            // 壓縮並保存圖片
            ImageCompressionUtility.CompressAndSaveImage(image, filePath, quality);

            return filePath;
        }

    }
}
