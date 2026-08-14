using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Bloxstrap.Resources;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class VersionMatcherPage : UiPage
    {
        private const string WeaoApiBase = "https://weao.xyz/api";
        private const string UserAgent = "WEAO-3PService";

        private List<ExploitEntry> _allExploits = new();

        public VersionMatcherPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAllVersions();
            LoadBootstrapperVersion();
            await LoadExploits();
        }

        private async Task LoadAllVersions()
        {
            ShowStatus("Fetching versions from WEAO API...", isError: false);

            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                var currentTask = http.GetStringAsync($"{WeaoApiBase}/versions/current");
                var futureTask = http.GetStringAsync($"{WeaoApiBase}/versions/future");
                var pastTask = http.GetStringAsync($"{WeaoApiBase}/versions/past");

                await Task.WhenAll(currentTask, futureTask, pastTask);

                var currentJson = System.Text.Json.JsonDocument.Parse(await currentTask);
                var futureJson = System.Text.Json.JsonDocument.Parse(await futureTask);
                var pastJson = System.Text.Json.JsonDocument.Parse(await pastTask);

                var current = currentJson.RootElement;
                var future = futureJson.RootElement;
                var past = pastJson.RootElement;

                CurrentWindows.Text = GetJsonProperty(current, "Windows") ?? "N/A";
                CurrentMac.Text = GetJsonProperty(current, "Mac") ?? "N/A";
                CurrentAndroid.Text = GetJsonProperty(current, "Android") ?? "N/A";
                CurrentIOS.Text = GetJsonProperty(current, "iOS") ?? "N/A";

                FutureWindows.Text = GetJsonProperty(future, "Windows") ?? "N/A";
                FutureMac.Text = GetJsonProperty(future, "Mac") ?? "N/A";

                PastWindows.Text = GetJsonProperty(past, "Windows") ?? "N/A";
                PastMac.Text = GetJsonProperty(past, "Mac") ?? "N/A";

                StatusText.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ShowStatus($"Failed to fetch versions: {ex.Message}", isError: true);
            }
        }

        private void LoadBootstrapperVersion()
        {
            BootstrapperVersion.Text = App.Version;
        }

        private async Task LoadExploits()
        {
            ExploitsStatus.Text = "Loading exploits...";
            ExploitsStatus.Visibility = Visibility.Visible;

            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                string json = await http.GetStringAsync($"{WeaoApiBase}/status/exploits");
                var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;

                _allExploits.Clear();

                if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var item in root.EnumerateArray())
                    {
                        string platform = GetJsonProperty(item, "platform") ?? "";
                        string extype = GetJsonProperty(item, "extype") ?? "";

                        if (!platform.Equals("Windows", StringComparison.OrdinalIgnoreCase))
                            continue;

                        if (!extype.Equals("wexecutor", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string title = GetJsonProperty(item, "title") ?? "Unknown";
                        string version = GetJsonProperty(item, "version") ?? "?";
                        bool updateStatus = item.TryGetProperty("updateStatus", out var us) && us.GetBoolean();
                        bool detected = item.TryGetProperty("detected", out var det) && det.GetBoolean();
                        bool free = item.TryGetProperty("free", out var fr) && fr.GetBoolean();
                        int unc = item.TryGetProperty("uncPercentage", out var uncEl) ? uncEl.GetInt32() : 0;
                        string rbxVersion = GetJsonProperty(item, "rbxversion") ?? "N/A";
                        string updatedDate = GetJsonProperty(item, "updatedDate") ?? "N/A";

                        _allExploits.Add(new ExploitEntry
                        {
                            Title = title,
                            Version = version,
                            IsUpdated = updateStatus,
                            StatusText = updateStatus ? "Updated" : "Outdated",
                            Detected = detected,
                            DetectedText = detected ? "Yes" : "No",
                            UncPercentage = unc,
                            UncText = unc > 0 ? $"{unc}%" : "-",
                            IsFree = free,
                            FreeText = free ? "Yes" : "No",
                            RobloxVersion = rbxVersion,
                            UpdatedDate = updatedDate
                        });
                    }
                }

                ApplyExploitFilter();
                ExploitsStatus.Text = $"Loaded {_allExploits.Count} Windows exploits.";
            }
            catch (Exception ex)
            {
                ExploitsStatus.Text = $"Failed to load exploits: {ex.Message}";
            }
        }

        private void ApplyExploitFilter()
        {
            string filter = ExploitFilterInput.Text.Trim().ToLower();
            bool updatedOnly = ShowUpdatedOnly.IsChecked == true;
            bool freeOnly = ShowFreeOnly.IsChecked == true;

            var filtered = _allExploits.Where(e =>
                (string.IsNullOrEmpty(filter) || e.Title.ToLower().Contains(filter)) &&
                (!updatedOnly || e.IsUpdated) &&
                (!freeOnly || e.IsFree)
            ).ToList();

            ExploitsList.ItemsSource = filtered;
            ExploitsStatus.Text = $"Showing {filtered.Count} of {_allExploits.Count} Windows exploits.";
        }

        private void MatchVersionButton_Click(object sender, RoutedEventArgs e)
        {
            string version = VersionInput.Text.Trim();

            if (string.IsNullOrEmpty(version))
            {
                ShowMatchStatus("Please enter a version string.", isError: true);
                return;
            }

            try
            {
                string settingsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MagicalLauncher", "Settings.json");

                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new();
                    dict["VersionMatcher"] = version;
                    string updatedJson = System.Text.Json.JsonSerializer.Serialize(dict, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(settingsPath, updatedJson);
                    ShowMatchStatus($"Locked to version: {version}", isError: false);
                }
                else
                {
                    ShowMatchStatus("Settings file not found.", isError: true);
                }
            }
            catch (Exception ex)
            {
                ShowMatchStatus($"Error: {ex.Message}", isError: true);
            }
        }

        private async void RefreshExploitsButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadExploits();
        }

        private void ExploitFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyExploitFilter();
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            ApplyExploitFilter();
        }

        private static string? GetJsonProperty(System.Text.Json.JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value))
            {
                return value.GetString();
            }
            return null;
        }

        private void ShowStatus(string message, bool isError)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError ? Brushes.Red : Brushes.LimeGreen;
            StatusText.Visibility = Visibility.Visible;
        }

        private void ShowMatchStatus(string message, bool isError)
        {
            MatchStatus.Text = message;
            MatchStatus.Foreground = isError ? Brushes.Red : Brushes.LimeGreen;
            MatchStatus.Visibility = Visibility.Visible;
        }

        private class ExploitEntry
        {
            public string Title { get; set; } = "";
            public string Version { get; set; } = "";
            public bool IsUpdated { get; set; }
            public string StatusText { get; set; } = "";
            public bool Detected { get; set; }
            public string DetectedText { get; set; } = "";
            public int UncPercentage { get; set; }
            public string UncText { get; set; } = "";
            public bool IsFree { get; set; }
            public string FreeText { get; set; } = "";
            public string RobloxVersion { get; set; } = "";
            public string UpdatedDate { get; set; } = "";
        }
    }
}
