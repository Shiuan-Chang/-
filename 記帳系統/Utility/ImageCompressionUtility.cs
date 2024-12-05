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
        public static Bitmap CompressImage(Image image, long quality)
        {
            // 获取 JPEG 编码器
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // 创建编码器参数对象，设定图像质量
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            // 创建一个新的 Bitmap 以存储压缩后的图像
            Bitmap compressedBitmap = new Bitmap(image.Width, image.Height);
            try
            {
                using (Graphics g = Graphics.FromImage(compressedBitmap))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 绘制原始图像到新的 Bitmap 上
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }

                // 保存到内存流以应用压缩参数
                using (var tempStream = new MemoryStream())
                {
                    compressedBitmap.Save(tempStream, jpgEncoder, myEncoderParameters);
                    tempStream.Seek(0, SeekOrigin.Begin);

                    // 返回压缩后的图像
                    return new Bitmap(tempStream);
                }
            }
            finally
            {
                // 确保 compressedBitmap 在使用后被正确释放
                compressedBitmap.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
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
