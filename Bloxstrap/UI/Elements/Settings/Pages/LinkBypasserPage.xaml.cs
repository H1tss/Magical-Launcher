using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class LinkBypasserPage : UiPage
    {
        private readonly HttpClient _http = new();
        private string _apiKey = string.Empty;
        private string _bypassedUrl = string.Empty;

        public LinkBypasserPage()
        {
            InitializeComponent();
            LoadApiKey();
        }

        private void LoadApiKey()
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MagicalLauncher", "BypassKey.txt");
            if (System.IO.File.Exists(path))
            {
                _apiKey = System.IO.File.ReadAllText(path);
                ApiKeyInput.Text = _apiKey;
            }
        }

        private void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            _apiKey = ApiKeyInput.Text.Trim();
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MagicalLauncher", "BypassKey.txt");
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)!);
            System.IO.File.WriteAllText(path, _apiKey);
            StatusText.Text = "API key saved";
        }

        private async void Bypass_Click(object sender, RoutedEventArgs e)
        {
            var link = LinkInput.Text.Trim();
            if (string.IsNullOrEmpty(link)) { StatusText.Text = "Enter a link first"; return; }
            if (string.IsNullOrEmpty(_apiKey)) { StatusText.Text = "Enter an API key first"; return; }

            StatusText.Text = "Bypassing link...";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.bypass.tools/bypass?url={Uri.EscapeDataString(link)}");
                request.Headers.Add("Authorization", _apiKey);

                var response = await _http.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var doc = JsonSerializer.Deserialize<JsonElement>(content);
                    _bypassedUrl = doc.GetProperty("destination").GetString() ?? "";
                    ResultUrl.Text = _bypassedUrl;
                    ResultCard.Visibility = Visibility.Visible;
                    StatusText.Text = "Link bypassed successfully!";
                }
                else
                {
                    StatusText.Text = $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex) { StatusText.Text = $"Error: {ex.Message}"; }
        }

        private void CopyUrl_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(_bypassedUrl);

        private void OpenUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_bypassedUrl))
            {
                Process.Start(new ProcessStartInfo { FileName = _bypassedUrl, UseShellExecute = true });
            }
        }
    }
}