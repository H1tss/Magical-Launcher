using System.Windows;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class ModMarketplacePage : UiPage
    {
        public ModMarketplacePage()
        {
            InitializeComponent();
        }

        private void InstallMod_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string modId)
            {
                System.Windows.MessageBox.Show($"Mod {modId} will be available soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
