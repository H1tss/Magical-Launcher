using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class NotificationPrefsPage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "Notifications.json");

        public NotificationPrefsPage()
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
                    if (root.TryGetProperty("updates", out var u)) UpdateNotifToggle.IsChecked = u.GetBoolean();
                    if (root.TryGetProperty("errors", out var er)) ErrorNotifToggle.IsChecked = er.GetBoolean();
                    if (root.TryGetProperty("launch", out var l)) LaunchNotifToggle.IsChecked = l.GetBoolean();
                    if (root.TryGetProperty("discord", out var d)) DiscordNotifToggle.IsChecked = d.GetBoolean();
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
                    updates = UpdateNotifToggle.IsChecked == true,
                    errors = ErrorNotifToggle.IsChecked == true,
                    launch = LaunchNotifToggle.IsChecked == true,
                    discord = DiscordNotifToggle.IsChecked == true
                };
                string dir = Path.GetDirectoryName(ConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                NotifStatus.Text = "Saved!";
                NotifStatus.Foreground = Brushes.LimeGreen;
                NotifStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                NotifStatus.Text = $"Error: {ex.Message}";
                NotifStatus.Foreground = Brushes.Red;
                NotifStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
