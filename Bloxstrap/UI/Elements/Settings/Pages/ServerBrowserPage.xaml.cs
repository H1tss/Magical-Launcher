using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ServerBrowserPage : UiPage
    {
        private readonly HttpClient _http = new();
        private string _cursor = string.Empty;
        private string _currentPlaceId = string.Empty;

        public ServerBrowserPage()
        {
            InitializeComponent();
        }

        private async void LoadServers_Click(object sender, RoutedEventArgs e)
        {
            var input = PlaceIdInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            _currentPlaceId = ExtractPlaceId(input);
            if (string.IsNullOrEmpty(_currentPlaceId)) { StatusText.Text = "Invalid Place ID"; return; }

            StatusText.Text = "Loading servers...";
            ServerList.ItemsSource = null;
            _cursor = string.Empty;

            try
            {
                var url = $"https://games.roblox.com/v1/games/{_currentPlaceId}/servers/Public?sortOrder=Asc&limit=100&cursor={_cursor}";
                var response = await _http.GetStringAsync(url);
                var doc = JsonSerializer.Deserialize<JsonElement>(response);

                var servers = new List<ServerEntry>();
                if (doc.TryGetProperty("data", out var data))
                {
                    foreach (var server in data.EnumerateArray())
                    {
                        servers.Add(new ServerEntry
                        {
                            Players = server.GetProperty("playing").GetInt32(),
                            MaxPlayers = server.GetProperty("maxPlayers").GetInt32(),
                            Ping = server.TryGetProperty("ping", out var p) ? p.GetInt32() : 0,
                            FPS = 0,
                            Region = "Unknown",
                            Id = server.GetProperty("id").GetString() ?? ""
                        });
                    }
                }

                if (doc.TryGetProperty("nextPageCursor", out var nextCursor))
                    _cursor = nextCursor.GetString() ?? "";

                ServerList.ItemsSource = servers;
                StatusText.Text = $"Loaded {servers.Count} servers";
            }
            catch (Exception ex) { StatusText.Text = $"Error: {ex.Message}"; }
        }

        private void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_cursor))
                LoadServers_Click(sender, e);
        }

        private void JoinEmpty_Click(object sender, RoutedEventArgs e)
        {
            if (ServerList.ItemsSource is List<ServerEntry> servers && servers.Count > 0)
            {
                var empty = servers.FindAll(s => s.Players < 5);
                if (empty.Count > 0)
                    JoinServer(empty[0]);
                else
                    JoinServer(servers[0]);
            }
        }

        private void JoinServer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is ServerEntry server)
                JoinServer(server);
        }

        private void JoinServer(ServerEntry server)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"roblox://placeId={_currentPlaceId}&gameInstanceId={server.Id}",
                UseShellExecute = true
            });
            StatusText.Text = $"Joining server {server.Id}...";
        }

        private string ExtractPlaceId(string input)
        {
            if (long.TryParse(input, out _)) return input;

            if (input.Contains("roblox.com"))
            {
                var parts = input.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "games" && i + 1 < parts.Length)
                        return parts[i + 1];
                }
            }
            return string.Empty;
        }

        private class ServerEntry
        {
            public int Players { get; set; }
            public int MaxPlayers { get; set; }
            public int Ping { get; set; }
            public int FPS { get; set; }
            public string Region { get; set; } = "";
            public string Id { get; set; } = "";
        }
    }
}