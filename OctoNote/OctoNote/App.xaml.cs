//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OctoNote
{
    using System.Configuration;
    using System.Windows;
    using JayWpf.Windows;

    /// <summary>Interaction logic for App XAML.</summary>
    public partial class App : Application
    {
        WpfWindow mainWindow;

        /// <summary>Startup event of the main App.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">StartupEventArgs 'e'.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            mainWindow = new WpfWindow("Views/MainPage.xaml");
            mainWindow.Title = "Octonote";
            mainWindow.IconText = "|⣿|";
            mainWindow.Closing += mainWindow_Closing;

            mainWindow.Show();
        }

        void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OctoNote.Views.MainPage page = this.mainWindow.MainFrame.Content as OctoNote.Views.MainPage;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
           
            config.AppSettings.Settings["tab"].Value = page.SelectedTabIndex.ToString();
            config.AppSettings.Settings["layout"].Value = page.SelectedLayout;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            page.SaveNotes();

            page.ArchiveNotes();
        }
    }
}
