using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class AccentColorPage : UiPage
    {
        private static readonly string ColorFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "AccentColor.txt");

        public AccentColorPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string saved = LoadColor();
            ApplyColor(saved);
        }

        private void PresetColor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string color)
            {
                ApplyColor(color);
                SaveColor(color);
                ShowStatus($"Accent color set to {color}");
            }
        }

        private void ApplyCustomColor_Click(object sender, RoutedEventArgs e)
        {
            string input = CustomColorInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;
            if (!input.StartsWith("#")) input = "#" + input;
            if (input.Length != 7 && input.Length != 9) { ShowStatus("Invalid color format. Use #RRGGBB"); return; }

            ApplyColor(input);
            SaveColor(input);
            ShowStatus($"Accent color set to {input}");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ApplyColor("#FF69B4");
            SaveColor("#FF69B4");
            ShowStatus("Reset to default pink");
        }

        private void ApplyColor(string hex)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(hex);
                var brush = new SolidColorBrush(color);
                CurrentColorBorder.Background = brush;
                ColorPreview.Background = brush;
            }
            catch { ShowStatus("Invalid color value"); }
        }

        private void SaveColor(string hex)
        {
            try
            {
                string dir = Path.GetDirectoryName(ColorFilePath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(ColorFilePath, hex);
            }
            catch { }
        }

        private static string LoadColor()
        {
            try
            {
                if (File.Exists(ColorFilePath))
                    return File.ReadAllText(ColorFilePath).Trim();
            }
            catch { }
            return "#FF69B4";
        }

        private void ShowStatus(string msg)
        {
            ColorStatus.Text = msg;
            ColorStatus.Foreground = Brushes.LimeGreen;
            ColorStatus.Visibility = Visibility.Visible;
        }
    }
}
