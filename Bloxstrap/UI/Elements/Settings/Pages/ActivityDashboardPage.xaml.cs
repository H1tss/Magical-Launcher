using System.IO;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ActivityDashboardPage : UiPage
    {
        private static readonly string ActivityPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "Activity.json");

        public ActivityDashboardPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadActivity();
        }

        private void LoadActivity()
        {
            try
            {
                ActivityList.Items.Clear();

                if (!File.Exists(ActivityPath))
                {
                    ActivityStatus.Text = "No activity recorded yet.";
                    TotalSessions.Text = "0";
                    TotalPlayTime.Text = "0h";
                    UniqueGames.Text = "0";
                    return;
                }

                string json = File.ReadAllText(ActivityPath);
                var doc = System.Text.Json.JsonDocument.Parse(json);

                int sessions = 0;
                double totalMinutes = 0;
                var uniqueGameIds = new HashSet<string>();

                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var item in doc.RootElement.EnumerateArray())
                    {
                        string gameName = item.TryGetProperty("gameName", out var gn) ? gn.GetString() ?? "Unknown" : "Unknown";
                        string placeId = item.TryGetProperty("placeId", out var pid) ? pid.GetString() ?? "" : "";
                        double minutes = item.TryGetProperty("minutes", out var m) ? m.GetDouble() : 0;
                        string timestamp = item.TryGetProperty("timestamp", out var ts) ? ts.GetString() ?? "" : "";

                        sessions++;
                        totalMinutes += minutes;
                        uniqueGameIds.Add(placeId);

                        string timeAgo = "";
                        if (DateTime.TryParse(timestamp, out var dt))
                        {
                            var span = DateTime.Now - dt;
                            if (span.TotalMinutes < 1) timeAgo = "Just now";
                            else if (span.TotalMinutes < 60) timeAgo = $"{(int)span.TotalMinutes}m ago";
                            else if (span.TotalHours < 24) timeAgo = $"{(int)span.TotalHours}h ago";
                            else timeAgo = $"{(int)span.TotalDays}d ago";
                        }

                        string duration = minutes >= 60 ? $"{(int)(minutes / 60)}h {(int)(minutes % 60)}m" : $"{(int)minutes}m";

                        ActivityList.Items.Add(new ActivityEntry
                        {
                            Description = $"{gameName} - {duration}",
                            TimeAgo = timeAgo
                        });
                    }
                }

                TotalSessions.Text = sessions.ToString();
                TotalPlayTime.Text = totalMinutes >= 60 ? $"{(int)(totalMinutes / 60)}h" : $"{(int)totalMinutes}m";
                UniqueGames.Text = uniqueGameIds.Count.ToString();
                ActivityStatus.Text = $"{sessions} sessions recorded.";
            }
            catch
            {
                ActivityStatus.Text = "Failed to load activity.";
            }
        }

        private class ActivityEntry
        {
            public string Description { get; set; } = "";
            public string TimeAgo { get; set; } = "";
        }
    }
}
