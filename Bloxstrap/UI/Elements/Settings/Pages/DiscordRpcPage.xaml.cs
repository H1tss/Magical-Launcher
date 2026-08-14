using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class DiscordRpcPage : UiPage
    {
        private static readonly string RpcConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "DiscordRpc.json");

        public DiscordRpcPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists(RpcConfigPath))
                {
                    string json = File.ReadAllText(RpcConfigPath);
                    var doc = System.Text.Json.JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("enabled", out var en)) EnableRpcToggle.IsChecked = en.GetBoolean();
                    if (root.TryGetProperty("details", out var det)) DetailsInput.Text = det.GetString() ?? "";
                    if (root.TryGetProperty("state", out var st)) StateInput.Text = st.GetString() ?? "";
                    if (root.TryGetProperty("largeImage", out var li)) LargeImageInput.Text = li.GetString() ?? "";
                    if (root.TryGetProperty("smallImage", out var si)) SmallImageInput.Text = si.GetString() ?? "";
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
                    enabled = EnableRpcToggle.IsChecked == true,
                    details = DetailsInput.Text,
                    state = StateInput.Text,
                    largeImage = LargeImageInput.Text,
                    smallImage = SmallImageInput.Text
                };

                string dir = Path.GetDirectoryName(RpcConfigPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(RpcConfigPath, System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                RpcStatus.Text = "Saved!";
                RpcStatus.Foreground = Brushes.LimeGreen;
                RpcStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                RpcStatus.Text = $"Error: {ex.Message}";
                RpcStatus.Foreground = Brushes.Red;
                RpcStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
