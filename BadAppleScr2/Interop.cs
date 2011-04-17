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
            IntPtr h = GetDC(IntPtr.Zero);
            return new Point((double)GetDeviceCaps(h, 88) / DEFAULT_DPI, (double)GetDeviceCaps(h, 90) / DEFAULT_DPI);
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
    }
}
