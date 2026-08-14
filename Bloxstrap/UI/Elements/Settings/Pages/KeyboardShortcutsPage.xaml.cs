using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class KeyboardShortcutsPage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "Shortcuts.json");

        public KeyboardShortcutsPage()
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
                    if (root.TryGetProperty("launch", out var l)) LaunchKey.Text = l.GetString() ?? "";
                    if (root.TryGetProperty("settings", out var s)) SettingsKey.Text = s.GetString() ?? "";
                    if (root.TryGetProperty("kill", out var k)) KillKey.Text = k.GetString() ?? "";
                    if (root.TryGetProperty("fps", out var f)) FpsKey.Text = f.GetString() ?? "";
                }
            }
            catch { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = new
                {
                    launch = LaunchKey.Text,
                    settings = SettingsKey.Text,
                    kill = KillKey.Text,
                    fps = FpsKey.Text
                };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                ShortcutStatus.Text = "Shortcuts saved!";
                ShortcutStatus.Foreground = Brushes.LimeGreen;
                ShortcutStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShortcutStatus.Text = $"Error: {ex.Message}";
                ShortcutStatus.Foreground = Brushes.Red;
                ShortcutStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
