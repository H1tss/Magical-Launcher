using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class AutoUpdaterPage : UiPage
    {
        private const string GitHubApi = "https://api.github.com/repos/Halo-tuff/Magical-Launcher/releases/latest";
        private string _downloadUrl = "";

        public AutoUpdaterPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentVersion.Text = $"v{App.Version}";
        }

        private async void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdateBtn.IsEnabled = false;
            ShowStatus("Checking for updates...", isError: false);

            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "MagicalLauncher");
                string json = await http.GetStringAsync(GitHubApi);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;

                string tagName = root.TryGetProperty("tag_name", out var tag) ? tag.GetString() ?? "" : "";
                string body = root.TryGetProperty("body", out var b) ? b.GetString() ?? "" : "";
                _downloadUrl = root.TryGetProperty("html_url", out var url) ? url.GetString() ?? "" : "";

                LatestVersion.Text = tagName;
                ReleaseNotes.Text = string.IsNullOrWhiteSpace(body) ? "No release notes." : body;

                if (tagName.TrimStart('v') != App.Version)
                {
                    ShowStatus($"Update available: {tagName}", isError: false);
                    DownloadUpdateBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    ShowStatus("You're up to date!", isError: false);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Failed to check: {ex.Message}", isError: true);
            }

            CheckUpdateBtn.IsEnabled = true;
        }

        private void DownloadUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_downloadUrl))
                Process.Start(new ProcessStartInfo(_downloadUrl) { UseShellExecute = true });
        }

        private void ShowStatus(string msg, bool isError)
        {
            UpdateStatus.Text = msg;
            UpdateStatus.Foreground = isError ? Brushes.Red : Brushes.LimeGreen;
            UpdateStatus.Visibility = Visibility.Visible;
        }
    }
}
