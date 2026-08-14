using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class BanAsyncPage : UiPage
    {
        private string _backupGuid = string.Empty;

        public BanAsyncPage()
        {
            InitializeComponent();
            LoadCurrentInfo();
        }

        private void LoadCurrentInfo()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
                var guid = key?.GetValue("MachineGuid")?.ToString();
                CurrentGuid.Text = guid ?? "Not found";
                _backupGuid = guid ?? string.Empty;
            }
            catch { CurrentGuid.Text = "Access denied"; }

            try
            {
                var nic = NetworkInterface.GetAllNetworkInterfaces();
                if (nic.Length > 0)
                {
                    CurrentMAC.Text = nic[0].GetPhysicalAddress().ToString();
                    AdapterCombo.ItemsSource = nic;
                    AdapterCombo.DisplayMemberPath = "Name";
                    if (nic.Length > 0) AdapterCombo.SelectedIndex = 0;
                }
            }
            catch { CurrentMAC.Text = "Not detected"; }
        }

        private void Log(string msg) => ActivityLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {msg}");

        private void CleanTraces_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                int cleaned = 0;

                string[] dirs = {
                    Path.Combine(localAppData, "Roblox", "logs"),
                    Path.Combine(localAppData, "Roblox", "http"),
                    Path.Combine(localAppData, "Temp", "Roblox"),
                };

                foreach (var dir in dirs)
                {
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                        cleaned++;
                    }
                }

                if (WipeInstalls.IsChecked == true)
                {
                    string versions = Path.Combine(localAppData, "Roblox", "Versions");
                    if (Directory.Exists(versions))
                    {
                        Directory.Delete(versions, true);
                        cleaned++;
                    }
                }

                Log($"Trace cleanup complete: {cleaned} items removed");
            }
            catch (Exception ex) { Log($"Cleanup error: {ex.Message}"); }
        }

        private void SpoofMAC_Click(object sender, RoutedEventArgs e)
        {
            Log("MAC spoofing requires admin privileges. Run as administrator.");
            System.Windows.MessageBox.Show("MAC spoofing requires running as administrator.", "Admin Required", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RevertMAC_Click(object sender, RoutedEventArgs e)
        {
            Log("MAC revert requires admin privileges.");
        }

        private void RandomizeGuid_Click(object sender, RoutedEventArgs e)
        {
            if (AckRisk.IsChecked != true)
            {
                System.Windows.MessageBox.Show("Please acknowledge the risks first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newGuid = Guid.NewGuid().ToString();
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true);
                key?.SetValue("MachineGuid", newGuid);
                CurrentGuid.Text = newGuid;
                Log($"MachineGuid randomized");
            }
            catch (Exception ex) { Log($"Guid randomization error: {ex.Message}"); }
        }

        private void RestoreGuid_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_backupGuid)) { Log("No backup available"); return; }
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true);
                key?.SetValue("MachineGuid", _backupGuid);
                CurrentGuid.Text = _backupGuid;
                Log("MachineGuid restored from backup");
            }
            catch (Exception ex) { Log($"Restore error: {ex.Message}"); }
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e) => ActivityLog.Items.Clear();
    }
}