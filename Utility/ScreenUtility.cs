using Microsoft.ML.Data;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace D2AutoFisher.Utility
{
    public static class ScreenUtility
    {
        /// <returns>The contents of the currently active screen as a <see cref="Bitmap"/></returns>
        public static List<Bitmap> GetScreensAsBitmap()
        {
            List<Bitmap> output = new List<Bitmap>();
            foreach (Screen screen in Screen.AllScreens)
            {
                Bitmap captureBitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height, PixelFormat.Format32bppArgb);
                using Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top, 0, 0, captureBitmap.Size);
                output.Add(captureBitmap);
            }
            return output;
        }

        /// <returns>The contents of the currently active screen as a <see cref="Bitmap"/></returns>
        public static Bitmap GetScreenAsBitmap()
        {
            Bitmap captureBitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height, PixelFormat.Format32bppArgb);
            using Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top, 0, 0, captureBitmap.Size);
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
