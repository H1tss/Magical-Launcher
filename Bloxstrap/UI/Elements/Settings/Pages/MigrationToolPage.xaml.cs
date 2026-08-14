using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class MigrationToolPage : UiPage
    {
        public MigrationToolPage()
        {
            InitializeComponent();
        }

        private void ImportBloxstrap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string bloxstrapPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Bloxstrap");

                if (!Directory.Exists(bloxstrapPath))
                {
                    ShowStatus("Bloxstrap installation not found.", true);
                    return;
                }

                string settingsFile = Path.Combine(bloxstrapPath, "Settings.json");
                if (File.Exists(settingsFile))
                {
                    string destDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "MagicalLauncher");
                    if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

                    File.Copy(settingsFile, Path.Combine(destDir, "Settings.json"), true);
                    ShowStatus("Bloxstrap settings imported!", false);
                }
                else
                {
                    ShowStatus("No Bloxstrap settings file found.", true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Import failed: {ex.Message}", true);
            }
        }

        private void ImportFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog { Filter = "JSON files (*.json)|*.json" };
                if (dialog.ShowDialog() == true)
                {
                    string destDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "MagicalLauncher");
                    if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

                    File.Copy(dialog.FileName, Path.Combine(destDir, "Settings.json"), true);
                    ShowStatus("Settings imported!", false);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Import failed: {ex.Message}", true);
            }
        }

        private void ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string settingsFile = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MagicalLauncher", "Settings.json");

                if (!File.Exists(settingsFile))
                {
                    ShowStatus("No settings file found.", true);
                    return;
                }

                var dialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = "MagicalLauncherSettings.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    File.Copy(settingsFile, dialog.FileName, true);
                    ShowStatus("Settings exported!", false);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Export failed: {ex.Message}", true);
            }
        }

        private void ShowStatus(string msg, bool isError)
        {
            MigrationStatus.Text = msg;
            MigrationStatus.Foreground = isError ? Brushes.Red : Brushes.LimeGreen;
            MigrationStatus.Visibility = Visibility.Visible;
        }
    }
}
