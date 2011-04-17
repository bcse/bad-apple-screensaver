using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BadAppleScr2
{
    /// <summary>
    /// ConfigWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private GrayscaleEffect fx = new GrayscaleEffect { Chrominance = App.Config.Chrominance };

        public ConfigWindow()
        {
            InitializeComponent();
            Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            bLink.Text = string.Format("© \"Bad Apple!!\" Screensaver {0}.{1}", ver.Major, ver.Minor);

            //load settings
            vFile.Text = App.Config.Video.LocalPath;
            vVolume.Value = App.Config.Volume;
            vStretch.SelectedIndex = (int)App.Config.Stretch;
            vChrominance.Value = App.Config.Chrominance;

            VideoElement.Source = App.Config.Video;
            VideoElement.Volume = App.Config.Volume;
            VideoElement.Stretch = App.Config.Stretch;
        }

        private void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoElement.Position = TimeSpan.Zero;
        }

        private void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            VideoElement.Effect = fx;
        }

        private void bBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mp4"; // Default file extension
            dlg.Filter = "Video files|*.mp4;*.m4v;*.mp4v;*.3gp;*.3gpp;*.3g2;*.3gp2;*.m2ts;*.m2t;*.mts;*.ts;*.tts;*.mov;*.asf;*.wm;*.wmv;*.avi;*.mpeg;*.mpg;*.m1v;*.m2v;*.mpe;*.ifo;*.vob"; // Filter files by extension

            // Process open file dialog box results
            if (dlg.ShowDialog() == true)
            {
                vFile.Text = dlg.FileName;
                VideoElement.Source = new Uri(dlg.FileName);
            }
        }

        private void vVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoElement.Volume = vVolume.Value;
        }

        private void vStretch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoElement.Stretch = (Stretch)vStretch.SelectedIndex;
        }

        private void vChrominance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            fx.Chrominance = vChrominance.Value;
        }

        private void bLink_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://blog.bcse.tw/bad-apple-screensaver-2");
        }

        private void bOK_Click(object sender, RoutedEventArgs e)
        {
            //save settings
            App.Config.Video = new Uri(vFile.Text);
            App.Config.Volume = vVolume.Value;
            App.Config.Stretch = (Stretch)vStretch.SelectedIndex;
            App.Config.Chrominance = vChrominance.Value;
            App.Config.Save();

            //close window
            Application.Current.Shutdown();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
