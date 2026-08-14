using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class LanguageSelectorPage : UiPage
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MagicalLauncher", "Language.txt");

        public LanguageSelectorPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string lang = File.ReadAllText(ConfigPath).Trim();
                    foreach (ComboBoxItem item in LanguageCombo.Items)
                    {
                        if (item.Tag?.ToString() == lang)
                        {
                            LanguageCombo.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void LanguageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageCombo.SelectedItem is ComboBoxItem selected && selected.Tag != null)
            {
                try
                {
                    string dir = Path.GetDirectoryName(ConfigPath)!;
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    File.WriteAllText(ConfigPath, selected.Tag.ToString());

                    LanguageStatus.Text = "Language saved. Restart to apply.";
                    LanguageStatus.Foreground = Brushes.LimeGreen;
                    LanguageStatus.Visibility = Visibility.Visible;
                }
                catch { }
            }
        }
    }
}
