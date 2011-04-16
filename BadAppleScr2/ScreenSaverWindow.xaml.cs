using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;

namespace BadAppleScr2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ScreenSaverWindow : Window
    {
        private static readonly ShaderEffect fx = new GrayscaleEffect { Chrominance = App.Config.Chrominance };

        public ScreenSaverWindow()
        {
            InitializeComponent();

            //load settings
            VideoElement.Source = App.Config.Video;
            VideoElement.Volume = App.Config.Volume;
            VideoElement.Stretch = App.Config.Stretch;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
#else
            ShowInTaskbar = false;
            Cursor = Cursors.None;
            Topmost = true;
#endif
        }

        void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement me = sender as MediaElement;
            me.Position = TimeSpan.Zero;
        }

        void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            MediaElement me = sender as MediaElement;
            me.Effect = fx;
        }
    }
}
