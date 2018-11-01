using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using WpQrCode.Infrastructure.Exceptions;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace WpQrCode.Helpers
{
    public class QrCodeHandler
    {
        //https://rajeeshmenoth.wordpress.com/2017/05/11/qr-code-generator-in-asp-net-core-using-zxing-net/
        public byte[] GenerateQr(string token, int width, int height)
        {
            try
            {
                var qrCodeWriter = new BarcodeWriterPixelData()
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions()
                    {
                        Height = height,
                        Width = width,
                        Margin = 0,
                    }
                };

                var result = qrCodeWriter.Write(token);

                using (var bitmap = new Bitmap(result.Width, result.Height, PixelFormat.Format32bppRgb))
                {
                    using (var ms = new MemoryStream())
                    {
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, result.Width, 
                            result.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                        try
                        { 
                            Marshal.Copy(result.Pixels, 0, bitmapData.Scan0, result.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }

                        bitmap.Save(ms, ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                throw new QrCodeException("Não foi possóvel gerar o código QR.", e);
            }
        }
    }
}
