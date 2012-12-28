using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BadAppleScr2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ScreenSaverWindow : Window
    {
        private static readonly ShaderEffect fx = new GrayscaleEffect
        {
            Chrominance = App.Config.Chrominance,
            Negative = App.Config.Negative,
            LeaveBlack = App.Config.LeaveBlack
        };

        public ScreenSaverWindow()
        {
            InitializeComponent();

            //load settings
            VideoElement.Source = App.Config.Video;
            VideoElement.Volume = App.Config.Volume;
            VideoElement.Stretch = App.Config.Stretch;
        }

        void ScreenSaverWindow_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            ShowInTaskbar = true;
            Topmost = false;
#else
            // Add WS_EX_TOOLWINDOW window style
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            int exStyle = (int)NativeMethods.GetWindowLong(wndHelper.Handle, (int)NativeMethods.GetWindowLongFields.GWL_EXSTYLE);
            exStyle |= (int)NativeMethods.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLong(wndHelper.Handle, (int)NativeMethods.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

            Mouse.OverrideCursor = Cursors.None;
            MouseMove += new MouseEventHandler(ScreenSaverWindow_MouseMove);
            MouseDown += new MouseButtonEventHandler(ScreenSaverWindow_MouseDown);
            KeyDown += new KeyEventHandler(ScreenSaverWindow_KeyDown);
#endif
        }

        void ScreenSaverWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void ScreenSaverWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        bool isActive;
        Point mousePosition;
        void ScreenSaverWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.MouseDevice.GetPosition(this);
            // Set IsActive and MouseLocation only the first time this event is called.
            if (!isActive)
            {
                mousePosition = currentPosition;
                isActive = true;
            }
            else
            {
                // If the mouse has moved significantly since first call, close.
                if ((Math.Abs(mousePosition.X - currentPosition.X) > 10) ||
                    (Math.Abs(mousePosition.Y - currentPosition.Y) > 10))
                {
                    Application.Current.Shutdown();
                }
            }
        }

        void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoElement.Position = TimeSpan.Zero;
        }

        void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            VideoElement.Effect = fx;
        }

        private void VideoElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // Show warning
            Background = new SolidColorBrush(Colors.Black);
            VideoElement.Visibility = Visibility.Hidden;
            WarningMessage.Visibility = Visibility.Visible;
        }
    }
}
