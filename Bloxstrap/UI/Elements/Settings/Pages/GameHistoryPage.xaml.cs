using System.IO;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class GameHistoryPage : UiPage
    {
        private static readonly string HistoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "GameHistory.json");

        public GameHistoryPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                HistoryList.Items.Clear();

                if (!File.Exists(HistoryPath))
                {
                    HistoryStatus.Text = "No game history yet.";
                    return;
                }

                string json = File.ReadAllText(HistoryPath);
                var doc = System.Text.Json.JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var item in doc.RootElement.EnumerateArray())
                    {
                        string gameName = item.TryGetProperty("gameName", out var gn) ? gn.GetString() ?? "Unknown" : "Unknown";
                        string placeId = item.TryGetProperty("placeId", out var pid) ? pid.GetString() ?? "" : "";
                        string timestamp = item.TryGetProperty("timestamp", out var ts) ? ts.GetString() ?? "" : "";
                        string serverType = item.TryGetProperty("serverType", out var st) ? st.GetString() ?? "" : "";

                        string timeAgo = "";
                        if (DateTime.TryParse(timestamp, out var dt))
                            timeAgo = GetTimeAgo(dt);

                        HistoryList.Items.Add(new HistoryEntry
                        {
                            GameName = gameName,
                            PlaceId = $"Place: {placeId}",
                            Details = serverType,
                            TimeAgo = timeAgo
                        });
                    }
                }

                HistoryStatus.Text = $"{HistoryList.Items.Count} games played.";
            }
            catch
            {
                HistoryStatus.Text = "Failed to load history.";
            }
        }

        private static string GetTimeAgo(DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            return $"{(int)span.TotalDays}d ago";
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Clear all game history?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try { if (File.Exists(HistoryPath)) File.Delete(HistoryPath); } catch { }
                LoadHistory();
            }
        }

        private class HistoryEntry
        {
            public string GameName { get; set; } = "";
            public string PlaceId { get; set; } = "";
            public string Details { get; set; } = "";
            public string TimeAgo { get; set; } = "";
        }
    }
}
