using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace StarBinder.LevelEditor.Utils
{
    public static class BitmapHelper
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        public static BitmapSource LoadBitmapSourceFromFile(this string filePath, double scale = 1)
        {
            using (var bmp = (Bitmap)Image.FromFile(filePath))
            {
                using (var resized = new Bitmap(bmp, new System.Drawing.Size((int)(bmp.Width * scale), (int)(bmp.Height * scale))))
                {
                    resized.SetResolution(96, 96);
                    return resized.ToBitmapSource();
                }
            }
        }
    }
}
