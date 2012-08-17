using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace BadAppleScr2
{
    internal class Interop
    {
        public static Point GetVisualScale(Visual visual)
        {
            const double DEFAULT_DPI = 96d; 
            Matrix m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new Point(m.M11 * DEFAULT_DPI, m.M22 * DEFAULT_DPI);
        }

        public static Point GetVisualScale()
        {
            const double DEFAULT_DPI = 96d;
            IntPtr h = NativeMethods.GetDC(IntPtr.Zero);
            return new Point((double)NativeMethods.GetDeviceCaps(h, 88) / DEFAULT_DPI, (double)NativeMethods.GetDeviceCaps(h, 90) / DEFAULT_DPI);
        }

        public static string GetDesktopWallpaper()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            return (string)regkey.GetValue("Wallpaper");
        }

        public static Stretch GetWallpaperStretch()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            string style = (string)regkey.GetValue("WallpaperStyle");
            switch (style)
            {
                case "2":
                    return Stretch.Fill;
                case "6":
                    return Stretch.Uniform;
                case "10":
                default:
                    return Stretch.UniformToFill;
            }
        }
    }

    internal class NativeMethods
    {
        [DllImport("gdi32.dll")]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hWnd);
    }
}
