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
            MediaElement media = (MediaElement)sender;
            media.Position = TimeSpan.Zero;
        }

        private void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            CompositeEffect fx2 = new CompositeEffect();

            /*
            MediaElement media1 = new MediaElement()
            {
                Source = new Uri(@"I:\Dropbox\Public\video\vibo-telecomlife.mp4"),
                IsMuted = true,
                Stretch = Stretch.Uniform
            };
            media1.MediaEnded += new RoutedEventHandler(VideoElement_MediaEnded);
            fx2.Tex1 = new VisualBrush() { Visual = media1 };

            MediaElement media2 = new MediaElement()
            {
                Source = new Uri(@"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv"),
                IsMuted = true,
                Stretch = Stretch.Uniform
            };
            media2.MediaEnded += new RoutedEventHandler(VideoElement_MediaEnded);
            fx2.Tex2 = new VisualBrush() { Visual = media2 };
            */
            fx2.Tex1 = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri(@"C:\Users\Public\Pictures\Sample Pictures\Chrysanthemum.jpg")));
            fx2.Tex2 = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg")));

            VideoElement.Effect = fx2;
        }

        private void bBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            using (System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog())
            {
                dlg.Filter = "Video files|*.mp4;*.m4v;*.mp4v;*.3gp;*.3gpp;*.3g2;*.3gp2;*.m2ts;*.m2t;*.mts;*.ts;*.tts;*.mov;*.asf;*.wm;*.wmv;*.avi;*.mpeg;*.mpg;*.m1v;*.m2v;*.mpe;*.ifo;*.vob|All files|*.*"; // Filter files by extension
                if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
                {
                    vFile.Text = dlg.FileName;
                    VideoElement.Source = new Uri(dlg.FileName);
                }
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
            //fx.Chrominance = vChrominance.Value;
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
