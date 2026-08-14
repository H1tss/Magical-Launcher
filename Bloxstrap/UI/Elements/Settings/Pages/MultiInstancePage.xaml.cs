using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class MultiInstancePage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "MultiInstance.json");

        public MultiInstancePage()
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
                    if (root.TryGetProperty("enabled", out var en)) MultiInstanceToggle.IsChecked = en.GetBoolean();
                    if (root.TryGetProperty("maxInstances", out var mi)) MaxInstancesInput.Text = mi.GetInt32().ToString();
                }
            }
            catch { }
        }

        private void MaxInstance_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string max)
                MaxInstancesInput.Text = max;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(MaxInstancesInput.Text, out int maxInstances);
                var config = new { enabled = MultiInstanceToggle.IsChecked == true, maxInstances };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                MultiStatus.Text = "Saved!";
                MultiStatus.Foreground = Brushes.LimeGreen;
                MultiStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MultiStatus.Text = $"Error: {ex.Message}";
                MultiStatus.Foreground = Brushes.Red;
                MultiStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
