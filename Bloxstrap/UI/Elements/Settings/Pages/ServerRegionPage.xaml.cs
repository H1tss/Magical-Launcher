using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ServerRegionPage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "ServerRegion.json");

        public ServerRegionPage()
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
                    if (root.TryGetProperty("region", out var r))
                    {
                        string region = r.GetString() ?? "auto";
                        foreach (ComboBoxItem item in RegionCombo.Items)
                        {
                            if (item.Tag?.ToString() == region)
                            {
                                RegionCombo.SelectedItem = item;
                                break;
                            }
                        }
                    }
                    if (root.TryGetProperty("force", out var f)) ForceRegionToggle.IsChecked = f.GetBoolean();
                }
            }
            catch { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string region = "auto";
                if (RegionCombo.SelectedItem is ComboBoxItem selected && selected.Tag != null)
                    region = selected.Tag.ToString() ?? "auto";

                var config = new { region, force = ForceRegionToggle.IsChecked == true };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                RegionStatus.Text = "Saved!";
                RegionStatus.Foreground = Brushes.LimeGreen;
                RegionStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                RegionStatus.Text = $"Error: {ex.Message}";
                RegionStatus.Foreground = Brushes.Red;
                RegionStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
