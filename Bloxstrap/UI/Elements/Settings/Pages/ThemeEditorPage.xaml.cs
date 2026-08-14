using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ThemeEditorPage : UiPage
    {
        public ThemeEditorPage()
        {
            InitializeComponent();
        }

        private void PresetTheme_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string theme)
            {
                CurrentTheme.Text = theme.Replace("_", " ");
                ShowStatus($"Theme set to {theme.Replace("_", " ")}");
            }
        }

        private void ExportTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = "MagicalLauncherTheme.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    var theme = new
                    {
                        name = CurrentTheme.Text,
                        version = "1.0",
                        exported = DateTime.UtcNow.ToString("o")
                    };
                    File.WriteAllText(dialog.FileName, System.Text.Json.JsonSerializer.Serialize(theme, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
                    ShowStatus("Theme exported!");
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Export failed: {ex.Message}");
            }
        }

        private void ImportTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    string json = File.ReadAllText(dialog.FileName);
                    var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("name", out var name))
                    {
                        CurrentTheme.Text = name.GetString() ?? "Imported";
                        ShowStatus($"Theme imported: {name.GetString()}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Import failed: {ex.Message}");
            }
        }

        private void ShowStatus(string msg)
        {
            ThemeStatus.Text = msg;
            ThemeStatus.Foreground = Brushes.LimeGreen;
            ThemeStatus.Visibility = Visibility.Visible;
        }
    }
}
