using System.Windows;
using System.Windows.Input;

using Bloxstrap.UI.ViewModels.Settings;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for FastFlagsPage.xaml
    /// </summary>
    public partial class FastFlagsPage : UiPage
    {
        private bool _initialLoad = false;

        private FastFlagsViewModel _viewModel = null!;

        public FastFlagsPage()
        {
            SetupViewModel();
            InitializeComponent();
        }

        private void SetupViewModel()
        {
            _viewModel = new FastFlagsViewModel();

            _viewModel.OpenFlagEditorEvent += OpenFlagEditor;
            _viewModel.RequestPageReloadEvent += (_, _) => SetupViewModel();

            DataContext = _viewModel;
        }

        private void OpenFlagEditor(object? sender, EventArgs e)
        {
            if (Window.GetWindow(this) is INavigationWindow window)
                window.Navigate(typeof(FastFlagEditorPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh datacontext on page load to synchronize with editor page.
            // The first load is skipped so we don't recreate the VM twice.
            if (!_initialLoad)
            {
                _initialLoad = true;
                return;
            }

            SetupViewModel();
        }

        private void LoadAllowlistedButton_Click(object sender, RoutedEventArgs e)
        {
            var json = @"{""DFIntCSGLevelOfDetailSwitchingDistance"":""0"",""DFIntCSGLevelOfDetailSwitchingDistanceL12"":""0"",""DFIntCSGLevelOfDetailSwitchingDistanceL23"":""0"",""DFIntCSGLevelOfDetailSwitchingDistanceL34"":""0"",""FFlagHandleAltEnterFullscreenManually"":""True"",""DFFlagTextureQualityOverrideEnabled"":""False"",""DFIntTextureQualityOverride"":""3"",""FIntDebugForceMSAASamples"":""4"",""DFFlagDisableDPIScale"":""False"",""FFlagDebugGraphicsPreferD3D11"":""False"",""FFlagDebugSkyGray"":""False"",""DFFlagDebugPauseVoxelizer"":""False"",""DFIntDebugFRMQualityLevelOverride"":""10"",""FIntFRMMaxGrassDistance"":""1000"",""FIntFRMMinGrassDistance"":""0"",""FFlagDebugGraphicsPreferVulkan"":""False"",""FFlagDebugGraphicsPreferOpenGL"":""False"",""FIntGrassMovementReducedMotionFactor"":""0""}";

            Clipboard.SetText(json);
            LoadStatusText.Text = "✅ Copied! Paste into FastFlag Editor.";
            LoadStatusText.Visibility = Visibility.Visible;
        }
    }
}
