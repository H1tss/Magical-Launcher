using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class LogViewerPage : UiPage
    {
        private static readonly string LogsDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "Logs");

        public LogViewerPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLogs();
        }

        private void LoadLogs()
        {
            LogList.Items.Clear();

            if (!Directory.Exists(LogsDir))
            {
                LogContent.Text = "No logs directory found.";
                return;
            }

            var files = Directory.GetFiles(LogsDir, "*.log").OrderByDescending(f => f).ToArray();
            foreach (var file in files)
            {
                LogList.Items.Add(new LogFile { FileName = Path.GetFileName(file), FullPath = file });
            }
        }

        private void LogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogList.SelectedItem is LogFile logFile)
            {
                try
                {
                    LogContent.Text = File.ReadAllText(logFile.FullPath);
                }
                catch (Exception ex)
                {
                    LogContent.Text = $"Failed to read log: {ex.Message}";
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadLogs();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(LogsDir))
                Process.Start("explorer.exe", LogsDir);
        }

        private class LogFile
        {
            public string FileName { get; set; } = "";
            public string FullPath { get; set; } = "";
        }
    }
}
