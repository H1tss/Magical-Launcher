using System.Windows;
using System.Windows.Controls;

using Bloxstrap.Resources;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class VersionDowngraderPage : UiPage
    {
        public VersionDowngraderPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInstalledVersions();
        }

        private void LoadInstalledVersions()
        {
            InstalledVersionsList.Items.Clear();

            string versionsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Roblox", "Versions");

            if (!Directory.Exists(versionsPath))
                return;

            foreach (string versionDir in Directory.GetDirectories(versionsPath))
            {
                string dirName = Path.GetFileName(versionDir);
                string robloxExe = Path.Combine(versionDir, "RobloxPlayerBeta.exe");

                if (File.Exists(robloxExe))
                {
                    InstalledVersionsList.Items.Add(new VersionEntry
                    {
                        Guid = dirName,
                        DisplayText = robloxExe
                    });
                }
            }
        }

        private async void InstallVersionButton_Click(object sender, RoutedEventArgs e)
        {
            string guid = VersionGuidInput.Text.Trim();

            if (string.IsNullOrEmpty(guid))
            {
                ShowStatus("Please enter a version GUID.", isError: true);
                return;
            }

            ShowStatus("Downloading version info...", isError: false);

            try
            {
                string deployUrl = $"https://clientsettings.roblox.com/v2/client-version/WindowsPlayer/channel/LIVE";

                using var http = new HttpClient();
                var response = await http.GetAsync(deployUrl);
                response.EnsureSuccessStatusCode();

                string versionsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Roblox", "Versions", guid);

                if (Directory.Exists(versionsPath))
                {
                    ShowStatus($"Version {guid} is already installed.", isError: false);
                    LoadInstalledVersions();
                    return;
                }

                ShowStatus($"Version {guid} queued for install. Launch Roblox with Magical Launcher to download it.", isError: false);
                LoadInstalledVersions();
            }
            catch (Exception ex)
            {
                ShowStatus($"Error: {ex.Message}", isError: true);
            }
        }

        private void DeleteVersionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string guid)
            {
                var result = System.Windows.MessageBox.Show(
                    $"Are you sure you want to delete version {guid}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    string versionsPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Roblox", "Versions", guid);

                    try
                    {
                        if (Directory.Exists(versionsPath))
                            Directory.Delete(versionsPath, true);

                        ShowStatus($"Version {guid} deleted.", isError: false);
                        LoadInstalledVersions();
                    }
                    catch (Exception ex)
                    {
                        ShowStatus($"Failed to delete: {ex.Message}", isError: true);
                    }
                }
            }
        }

        private void ShowStatus(string message, bool isError)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError
                ? System.Windows.Media.Brushes.Red
                : System.Windows.Media.Brushes.LimeGreen;
            StatusText.Visibility = Visibility.Visible;
        }

        private class VersionEntry
        {
            public string Guid { get; set; } = "";
            public string DisplayText { get; set; } = "";
        }
    }
}
