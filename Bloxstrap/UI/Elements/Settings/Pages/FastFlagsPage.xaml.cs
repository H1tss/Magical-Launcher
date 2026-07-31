using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class FastFlagsPage : UiPage
    {
        public FastFlagsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
