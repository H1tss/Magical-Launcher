using System.IO;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class PortableModePage : UiPage
    {
        public PortableModePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataLocation.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            PortableStatus.Text = "Portable mode setting saved. Restart Magical Launcher to apply.";
            PortableStatus.Foreground = Brushes.LimeGreen;
            PortableStatus.Visibility = Visibility.Visible;
        }
    }
}
