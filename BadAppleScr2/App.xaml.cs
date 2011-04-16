using System;
using System.Windows;
using System.Diagnostics;

namespace BadAppleScr2
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        internal static Config Config = Config.Open(string.Format("{0}{1}BadAppleScr{1}config.xml", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.DirectorySeparatorChar));

        internal void BadAppleScr2_Startup(object sender, StartupEventArgs e)
        {
            ProcessCommandLineArgs(e.Args);
        }

        internal void ProcessCommandLineArgs(string[] args)
        {
            if (args.Length > 0)
            {
                // Get the 2 character command line argument 
                string arg = args[0].ToUpperInvariant().Trim().Substring(0, 2);
                switch (arg)
                {
                    case "/C":
                        // Show the options dialog
                        ShowConfig();
                        break;
                    case "/P":
                        // Don't do anything for preview 
                        Application.Current.Shutdown();
                        break;
                    case "/S":
                        // Show screensaver form 
                        ShowScreensaver();
                        break;
                    default:
                        Application.Current.Shutdown();
                        break;
                }
            }
            else
            {
                // If no arguments were passed in, show the screensaver 
                ShowConfig();
            }
        }

        /// <summary>
        /// Shows screen saver by creating one instance of ScreenSaverWindow for each monitor.
        /// 
        /// Note: uses WinForms's Screen class to get monitor info.
        /// </summary>
        internal void ShowScreensaver()
        {
            Point scale = Interop.GetVisualScale();
            Debug.WriteLine(scale);

            //creates window on every screens
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                ScreenSaverWindow window = new ScreenSaverWindow();
                window.WindowStartupLocation = WindowStartupLocation.Manual;

                //covers entire monitor
                Debug.WriteLine(screen.Bounds);
                window.Left = screen.Bounds.X / scale.X;
                window.Top = screen.Bounds.Y / scale.Y;
                window.Width = screen.Bounds.Width / scale.X;
                window.Height = screen.Bounds.Height / scale.Y;

                //set volume and stretch method
                window.VideoElement.Source = App.Config.Video;
                window.VideoElement.IsMuted = screen != System.Windows.Forms.Screen.PrimaryScreen;
                window.VideoElement.Volume = App.Config.Volume;
                window.VideoElement.Stretch = App.Config.Stretch;
                //window.VideoEffect.Chrominance = App.Config.Chrominance;

                window.Show();
            }
        }

        internal void ShowConfig()
        {
            ConfigWindow window = new ConfigWindow();
            window.ShowDialog();
        }
    }
}
