using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Utilities
{
    public class QRCodeMona
    {
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public static async Task<string> GetQrImagePath(string code, IHttpContextAccessor httpContextAccessor)
        {
            return await Task.Run(() =>
            {
                string host = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value;
                string fileQrCodeImgPath = string.Empty;
                string fileName = Guid.NewGuid().ToString() + "_qrCode.png";
                string folderPath = Path.Combine(CoreContants.UPLOAD_FOLDER_NAME + "/" + CoreContants.QR_CODE_FOLDER_NAME);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                Url url = new Url(code);
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                byte[] contentQrCodeImage = BitmapToByteArray(qrCodeImage);
                FileUtilities.CreateDirectory(folderPath);
                FileUtilities.SaveToPath(Path.Combine(folderPath, fileName), contentQrCodeImage);
                fileQrCodeImgPath = Path.Combine(CoreContants.UPLOAD_FOLDER_NAME + "/" + CoreContants.QR_CODE_FOLDER_NAME + "/" + fileName);
                return host + "/" + fileQrCodeImgPath;
            });
        }
    }
}
