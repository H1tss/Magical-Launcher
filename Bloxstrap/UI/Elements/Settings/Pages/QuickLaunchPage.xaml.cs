using System.Diagnostics;
using System.IO;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class QuickLaunchPage : UiPage
    {
        private static readonly string PinnedPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "PinnedGames.json");

        private List<PinnedGame> _games = new();

        public QuickLaunchPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPinnedGames();
        }

        private void LoadPinnedGames()
        {
            _games.Clear();
            PinnedGamesList.Items.Clear();

            try
            {
                if (File.Exists(PinnedPath))
                {
                    string json = File.ReadAllText(PinnedPath);
                    var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var item in doc.RootElement.EnumerateArray())
                        {
                            string placeId = item.TryGetProperty("placeId", out var pid) ? pid.GetString() ?? "" : "";
                            string gameName = item.TryGetProperty("gameName", out var gn) ? gn.GetString() ?? "Unknown" : "Unknown";
                            _games.Add(new PinnedGame { PlaceId = placeId, GameName = gameName });
                        }
                    }
                }
            }
            catch { }

            foreach (var game in _games)
                PinnedGamesList.Items.Add(game);

            QuickLaunchStatus.Text = $"{_games.Count} pinned games.";
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            string placeId = PlaceIdInput.Text.Trim();
            string gameName = GameNameInput.Text.Trim();

            if (string.IsNullOrEmpty(placeId)) return;
            if (string.IsNullOrEmpty(gameName)) gameName = $"Game {placeId}";

            if (_games.Any(g => g.PlaceId == placeId))
            {
                QuickLaunchStatus.Text = "Already pinned.";
                return;
            }

            _games.Add(new PinnedGame { PlaceId = placeId, GameName = gameName });
            SavePinnedGames();
            LoadPinnedGames();
            PlaceIdInput.Text = "";
            GameNameInput.Text = "";
        }

        private void RemoveGame_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string placeId)
            {
                _games.RemoveAll(g => g.PlaceId == placeId);
                SavePinnedGames();
                LoadPinnedGames();
            }
        }

        private void SavePinnedGames()
        {
            try
            {
                string dir = Path.GetDirectoryName(PinnedPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(PinnedPath, System.Text.Json.JsonSerializer.Serialize(_games, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            }
            catch { }
        }

        private class PinnedGame
        {
            public string PlaceId { get; set; } = "";
            public string GameName { get; set; } = "";
        }
    }
}
