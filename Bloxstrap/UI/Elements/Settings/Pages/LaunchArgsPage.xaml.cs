using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class LaunchArgsPage : UiPage
    {
        private static readonly string ArgsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "LaunchArgs.txt");

        public LaunchArgsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(ArgsPath))
                    ArgsInput.Text = File.ReadAllText(ArgsPath);
            }
            catch { }
        }

        private void QuickArg_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string arg)
            {
                string current = ArgsInput.Text.Trim();
                if (!current.Contains(arg))
                {
                    ArgsInput.Text = string.IsNullOrEmpty(current) ? arg : current + "\n" + arg;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dir = Path.GetDirectoryName(ArgsPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ArgsPath, ArgsInput.Text);

                ArgsStatus.Text = "Arguments saved!";
                ArgsStatus.Foreground = Brushes.LimeGreen;
                ArgsStatus.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ArgsStatus.Text = $"Error: {ex.Message}";
                ArgsStatus.Foreground = Brushes.Red;
                ArgsStatus.Visibility = Visibility.Visible;
            }
        }
    }
}
