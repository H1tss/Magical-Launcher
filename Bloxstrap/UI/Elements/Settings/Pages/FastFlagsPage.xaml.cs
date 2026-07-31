using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bloxstrap.UI.ViewModels;
using Wpf.Ui.Controls;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    public partial class FastFlagsPage : UiPage
    {
        public FastFlagsPage()
        {
            InitializeComponent();
            DataContext = new FastFlagsViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void LoadAllowlistedButton_Click(object sender, RoutedEventArgs e)
        {
            var allowlistedFlags = new Dictionary<string, object>
            {
                ["DFIntCSGLevelOfDetailSwitchingDistance"] = 0,
                ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = 0,
                ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = 0,
                ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = 0,
                ["FFlagHandleAltEnterFullscreenManually"] = true,
                ["DFFlagTextureQualityOverrideEnabled"] = false,
                ["DFIntTextureQualityOverride"] = 3,
                ["FIntDebugForceMSAASamples"] = 4,
                ["DFFlagDisableDPIScale"] = false,
                ["FFlagDebugGraphicsPreferD3D11"] = false,
                ["FFlagDebugSkyGray"] = false,
                ["DFFlagDebugPauseVoxelizer"] = false,
                ["DFIntDebugFRMQualityLevelOverride"] = 10,
                ["FIntFRMMaxGrassDistance"] = 1000,
                ["FIntFRMMinGrassDistance"] = 0,
                ["FFlagDebugGraphicsPreferVulkan"] = false,
                ["FFlagDebugGraphicsPreferOpenGL"] = false,
                ["FIntGrassMovementReducedMotionFactor"] = 0
            };

            foreach (var flag in allowlistedFlags)
            {
                App.FastFlags.SetValue(flag.Key, flag.Value.ToString());
            }
            
            App.FastFlags.Save();
            
            LoadStatusText.Text = "✅ All 18 allowlisted flags loaded!";
            LoadStatusText.Visibility = Visibility.Visible;
            LoadStatusText.Foreground = new SolidColorBrush(Colors.LimeGreen);
        }
    }
}
