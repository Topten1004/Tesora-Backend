// <copyright company="Chris McGorty" author="Chris McGorty">
//     Copyright (c) 2020 All Rights Reserved
// </copyright>
using SkiaSharp.QrCode;
using SkiaSharp;
using System.Reflection;


namespace NFTWallet.Engine
{
    /// <summary>
    /// QRCode
    /// </summary>
    public static class QRCode
    {
        /// <summary>
        /// QR Image - string is in Base64
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static byte[] GenerateCode(int width, int height, string text, string icon)
        {
            using (var generator = new QRCodeGenerator())
            {
                // Generate the QR Code
                var qr = generator.CreateQrCode(text, ECCLevel.H);

                // Render to canvas
                var info = new SKImageInfo(width, height);

                using var surface = SKSurface.Create(info);
                var canvas = surface.Canvas;

                // Render the QR Code on the surface
                canvas.Render(qr, width, height);

                if (icon != null)
                {
                    // Get the overlay image
                    var local = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var folder = $"Images/{icon}.png";
                    var image = Path.Combine(local, folder);

                    SKBitmap bitmap = SKBitmap.Decode(image);
                    var offsetH = (width - bitmap.Width) / 2;
                    var offsetV = (height - bitmap.Height) / 2;
                    canvas.DrawBitmap(bitmap, SKRect.Create(offsetH, offsetV, bitmap.Width, bitmap.Height));
                }

                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var ms = new MemoryStream())
                {
                    data.SaveTo(ms);

                    return ms.ToArray();
                }
            }
        }
    }
}
