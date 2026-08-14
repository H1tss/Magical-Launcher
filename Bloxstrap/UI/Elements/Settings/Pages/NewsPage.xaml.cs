using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class NewsPage : UiPage
    {
        private readonly HttpClient _http = new();

        public NewsPage()
        {
            InitializeComponent();
            LoadReleases();
        }

        private void RefreshNews_Click(object sender, RoutedEventArgs e)
        {
            NewsList.Items.Clear();
            NewsList.Items.Add(new { Title = "Loading...", Excerpt = "Fetching news..." });

            _ = LoadDevForumNews();
        }

        private async System.Threading.Tasks.Task LoadDevForumNews()
        {
            try
            {
                var response = await _http.GetStringAsync("https://devforum.roblox.com.json");
                var doc = System.Text.Json.JsonDocument.Parse(response);

                NewsList.Items.Clear();
                if (doc.RootElement.TryGetProperty("topic_list", out var topics))
                {
                    foreach (var topic in topics.GetProperty("topics").EnumerateArray())
                    {
                        var title = topic.GetProperty("title").GetString() ?? "No title";
                        var excerpt = topic.TryGetProperty("excerpt", out var ex) ? ex.GetString() ?? "" : "";
                        NewsList.Items.Add(new { Title = title, Excerpt = excerpt });
                    }
                }
            }
            catch
            {
                NewsList.Items.Clear();
                NewsList.Items.Add(new { Title = "Failed to load news", Excerpt = "Check your internet connection" });
            }
        }

        private void LoadReleases()
        {
            ReleaseList.Items.Clear();
            ReleaseList.Items.Add(new { Version = "v1.0.0", Notes = "Initial release of Magical Launcher" });
            ReleaseList.Items.Add(new { Version = "v1.0.1", Notes = "Added 20 new features from ExploitStrap" });
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Hit-man710/Magical-Launcher/releases",
                UseShellExecute = true
            });
        }

        private void OpenDevForum_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://devforum.roblox.com",
                UseShellExecute = true
            });
        }
    }
}