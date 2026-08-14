using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class AltGenPage : UiPage
    {
        private readonly HttpClient _http = new();
        private string _apiKey = string.Empty;
        private string _generatedUser = string.Empty;
        private string _generatedPass = string.Empty;
        private string _generatedCookie = string.Empty;

        public AltGenPage()
        {
            InitializeComponent();
            LoadApiKey();
        }

        private void LoadApiKey()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MagicalLauncher", "BloxGenKey.txt");
            if (File.Exists(path))
            {
                _apiKey = File.ReadAllText(path);
                ApiKeyInput.Text = _apiKey;
            }
        }

        private void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            _apiKey = ApiKeyInput.Text.Trim();
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MagicalLauncher", "BloxGenKey.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, _apiKey);
            StatusText.Text = "API key saved";
        }

        private async void GenerateAlt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_apiKey)) { StatusText.Text = "Enter an API key first"; return; }

            StatusText.Text = "Generating account...";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.bloxgen.net/generate");
                request.Headers.Add("Authorization", _apiKey);

                var response = await _http.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var doc = JsonSerializer.Deserialize<JsonElement>(content);
                    _generatedUser = doc.GetProperty("username").GetString() ?? "";
                    _generatedPass = doc.GetProperty("password").GetString() ?? "";
                    _generatedCookie = doc.GetProperty("cookie").GetString() ?? "";

                    ResultUsername.Text = _generatedUser;
                    ResultPassword.Text = _generatedPass;
                    ResultCookie.Text = _generatedCookie;
                    ResultCard.Visibility = Visibility.Visible;
                    StatusText.Text = "Account generated successfully!";
                }
                else
                {
                    StatusText.Text = $"Error: {response.StatusCode} - {content}";
                }
            }
            catch (Exception ex) { StatusText.Text = $"Error: {ex.Message}"; }
        }

        private void CopyUser_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(_generatedUser);
        private void CopyPass_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(_generatedPass);
        private void CopyCookie_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(_generatedCookie);
    }
}