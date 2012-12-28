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

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion
    }
}
