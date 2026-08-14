using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class AutoClosePage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "AutoClose.json");

        public AutoClosePage()
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
                    if (root.TryGetProperty("enabled", out var en)) AutoCloseToggle.IsChecked = en.GetBoolean();
                    if (root.TryGetProperty("delay", out var d)) DelayInput.Text = d.GetInt32().ToString();
                }
            }
            catch { }
        }

        private void DelayPreset_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string delay)
                DelayInput.Text = delay;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(DelayInput.Text, out int delay);
                var config = new { enabled = AutoCloseToggle.IsChecked == true, delay };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                AutoCloseStatus.Text = "Saved!";
                AutoCloseStatus.Foreground = Brushes.LimeGreen;
                AutoCloseStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                AutoCloseStatus.Text = $"Error: {ex.Message}";
                AutoCloseStatus.Foreground = Brushes.Red;
                AutoCloseStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
