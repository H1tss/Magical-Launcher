using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ConnectionDiagnosticsPage : UiPage
    {
        public ConnectionDiagnosticsPage()
        {
            InitializeComponent();
        }

        private async void RunDiagnostics_Click(object sender, RoutedEventArgs e)
        {
            RunDiagnosticsBtn.IsEnabled = false;
            ResultsList.Items.Clear();
            DiagStatus.Text = "Running diagnostics...";

            await RunTest("Google DNS (8.8.8.8)", "8.8.8.8");
            await RunTest("Cloudflare DNS (1.1.1.1)", "1.1.1.1");
            await RunHttpTest("Roblox CDN", "https://clientsettingscdn.roblox.com");
            await RunHttpTest("Roblox API", "https://clientsettings.roblox.com");
            await RunHttpTest("WEAO API", "https://weao.xyz/api/versions/current");

            DiagStatus.Text = "Diagnostics complete.";
            RunDiagnosticsBtn.IsEnabled = true;
        }

        private async Task RunTest(string testName, string host)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(host, 5000);
                if (reply.Status == IPStatus.Success)
                {
                    ResultsList.Items.Add(new DiagResult
                    {
                        TestName = testName,
                        ResultText = "PASS",
                        IsPassed = true,
                        Details = $"{reply.RoundtripTime}ms"
                    });
                }
                else
                {
                    ResultsList.Items.Add(new DiagResult
                    {
                        TestName = testName,
                        ResultText = "FAIL",
                        IsPassed = false,
                        Details = reply.Status.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ResultsList.Items.Add(new DiagResult
                {
                    TestName = testName,
                    ResultText = "FAIL",
                    IsPassed = false,
                    Details = ex.Message
                });
            }
        }

        private async Task RunHttpTest(string testName, string url)
        {
            try
            {
                using var http = new HttpClient();
                http.Timeout = TimeSpan.FromSeconds(5);
                var sw = Stopwatch.StartNew();
                var response = await http.GetAsync(url);
                sw.Stop();
                ResultsList.Items.Add(new DiagResult
                {
                    TestName = testName,
                    ResultText = response.IsSuccessStatusCode ? "PASS" : "FAIL",
                    IsPassed = response.IsSuccessStatusCode,
                    Details = $"{sw.ElapsedMilliseconds}ms - {(int)response.StatusCode}"
                });
            }
            catch (Exception ex)
            {
                ResultsList.Items.Add(new DiagResult
                {
                    TestName = testName,
                    ResultText = "FAIL",
                    IsPassed = false,
                    Details = ex.Message
                });
            }
        }

        private class DiagResult
        {
            public string TestName { get; set; } = "";
            public string ResultText { get; set; } = "";
            public bool IsPassed { get; set; }
            public string Details { get; set; } = "";
        }
    }
}
