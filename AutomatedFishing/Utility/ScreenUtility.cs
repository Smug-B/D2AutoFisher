using Microsoft.ML.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AutomatedFishing.Utility
{
    public static class ScreenUtility
    {
        /// <returns>The contents of the currently active screen as a <see cref="Bitmap"/></returns>
        public static Bitmap GetScreenAsBitmap()
        {
            Bitmap captureBitmap = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            using Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(1920, 1080));
            return captureBitmap;
        }

        public static MLImage BitmapToMLImage(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var length = bmpData.Stride * height;
            byte[] bytes = new byte[length];
            Marshal.Copy(bmpData.Scan0, bytes, 0, length);
            bitmap.UnlockBits(bmpData);

            return MLImage.CreateFromPixels(width, height, MLPixelFormat.Rgba32, bytes);
        }
    }
}
