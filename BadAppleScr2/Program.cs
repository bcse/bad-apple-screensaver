using System;
using System.Windows;
using WindowsRecipes.TaskbarSingleInstance;
using WindowsRecipes.TaskbarSingleInstance.Wpf;

namespace BadAppleScr2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (SingleInstanceManager manager = SingleInstanceManager.Initialize(GetSingleInstanceManagerSetup()))
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }

        private static SingleInstanceManagerSetup GetSingleInstanceManagerSetup()
        {
            return new SingleInstanceManagerSetup("BC5EEA71-7BFD-4E9B-9B10-2581EE09CBFF")
            {
                ArgumentsHandler = args => ((App)Application.Current).ProcessCommandLineArgs(args),
                ArgumentsHandlerInvoker = new ApplicationDispatcherInvoker(),
                DelivaryFailureNotification = ex => MessageBox.Show(ex.Message, "An error occured"),
            };
        }
    }
}
