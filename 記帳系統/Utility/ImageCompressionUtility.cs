using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Utility
{
    public static class ImageCompressionUtility
    {
        // 只做壓縮，不做儲存，返回壓縮後的 Bitmap
        public static void CompressAndSaveImage(Image image, string outputPath, long quality)
        {
            // 確保圖片不是 null
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (string.IsNullOrEmpty(outputPath)) throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));

            // 獲取 JPEG 編碼器
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            if (jpgEncoder == null) throw new InvalidOperationException("JPEG encoder not found");

            // 設定圖像品質參數
            System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(qualityEncoder, quality);
            encoderParameters.Param[0] = encoderParameter;

            // 使用 Graphics 繪製壓縮圖片
            using (Bitmap compressedBitmap = new Bitmap(image.Width, image.Height))
            {
                using (Graphics g = Graphics.FromImage(compressedBitmap))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 繪製原始圖像到新 Bitmap
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }

                // 儲存壓縮圖片到指定路徑
                compressedBitmap.Save(outputPath, jpgEncoder, encoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
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
