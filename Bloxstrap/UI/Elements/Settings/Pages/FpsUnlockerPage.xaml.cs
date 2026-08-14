using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class FpsUnlockerPage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "FpsUnlocker.json");

        public FpsUnlockerPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var doc = System.Text.Json.JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("enabled", out var en)) FpsUnlockToggle.IsChecked = en.GetBoolean();
                    if (root.TryGetProperty("targetFps", out var fps)) CustomFpsInput.Text = fps.GetInt32().ToString();
                }
            }
            catch { }
        }

        private void FpsPreset_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string fps)
                CustomFpsInput.Text = fps;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(CustomFpsInput.Text, out int targetFps);
                var config = new { enabled = FpsUnlockToggle.IsChecked == true, targetFps };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                FpsStatus.Text = "Saved!";
                FpsStatus.Foreground = Brushes.LimeGreen;
                FpsStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                FpsStatus.Text = $"Error: {ex.Message}";
                FpsStatus.Foreground = Brushes.Red;
                FpsStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
