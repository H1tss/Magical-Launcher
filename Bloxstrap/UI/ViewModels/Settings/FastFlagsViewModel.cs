using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Bloxstrap.Enums.FlagPresets;

namespace Bloxstrap.UI.ViewModels.Settings
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private Dictionary<string, object>? _preResetFlags;

        public event EventHandler? RequestPageReloadEvent;
        
        public event EventHandler? OpenFlagEditorEvent;

        private void OpenFastFlagEditor() => OpenFlagEditorEvent?.Invoke(this, EventArgs.Empty);

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public Visibility CanShowFastFlagEditor => App.IsStudioInstalled ? Visibility.Visible : Visibility.Collapsed;

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
        }

        public IReadOnlyDictionary<MSAAMode, string?> MSAALevels => FastFlagManager.MSAAModes;

        public MSAAMode SelectedMSAALevel
        {
            get => MSAALevels.FirstOrDefault(x => x.Value == App.FastFlags.GetPreset("Rendering.MSAA")).Key;
            set => App.FastFlags.SetPreset("Rendering.MSAA", MSAALevels[value]);
        }

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null);
        }

        public IReadOnlyDictionary<TextureQuality, string?> TextureQualities => FastFlagManager.TextureQualityLevels;

        public TextureQuality SelectedTextureQuality
        {
            get => TextureQualities.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.TextureQuality.Level")).FirstOrDefault().Key;
            set
            {
                if (value == TextureQuality.Default)
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality", null);
                }
                else
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", "True");
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", TextureQualities[value]);
                }
            }
        }

        public bool ResetConfiguration
        {
            get => _preResetFlags is not null;

            set
            {
                if (value)
                {
                    _preResetFlags = new(App.FastFlags.Prop);
                    App.FastFlags.Prop.Clear();
                }
                else
                {
                    App.FastFlags.Prop = _preResetFlags!;
                    _preResetFlags = null;
                }

                RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        // ----------------------------------------------------------------
        // Helpers for the 13 directly-bound allowlisted flags below.
        // -1 is used as the "not set / use Roblox default" sentinel for
        // every slider-backed int flag, since 0 is a meaningful value for
        // several of them (e.g. grass distance).
        // ----------------------------------------------------------------

        private static int GetIntFlag(string key)
        {
            var raw = App.FastFlags.GetValue(key);
            return int.TryParse(raw, out int value) ? value : -1;
        }

        private static void SetIntFlag(string key, int value)
        {
            App.FastFlags.SetValue(key, value <= -1 ? null : value.ToString());
        }

        private static bool GetBoolFlag(string key) => App.FastFlags.GetValue(key) == "True";

        private static void SetBoolFlag(string key, bool value) => App.FastFlags.SetValue(key, value ? "True" : null);

        // --- Geometry ---

        public int CSGLodSwitchingDistance
        {
            get => GetIntFlag("DFIntCSGLevelOfDetailSwitchingDistance");
            set => SetIntFlag("DFIntCSGLevelOfDetailSwitchingDistance", value);
        }

        public int CSGLodSwitchingDistanceL12
        {
            get => GetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL12");
            set => SetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL12", value);
        }

        public int CSGLodSwitchingDistanceL23
        {
            get => GetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL23");
            set => SetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL23", value);
        }

        public int CSGLodSwitchingDistanceL34
        {
            get => GetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL34");
            set => SetIntFlag("DFIntCSGLevelOfDetailSwitchingDistanceL34", value);
        }

        // --- Rendering (advanced / debug) ---

        public bool PreferD3D11
        {
            get => GetBoolFlag("FFlagDebugGraphicsPreferD3D11");
            set => SetBoolFlag("FFlagDebugGraphicsPreferD3D11", value);
        }

        public bool PreferVulkan
        {
            get => GetBoolFlag("FFlagDebugGraphicsPreferVulkan");
            set => SetBoolFlag("FFlagDebugGraphicsPreferVulkan", value);
        }

        public bool PreferOpenGL
        {
            get => GetBoolFlag("FFlagDebugGraphicsPreferOpenGL");
            set => SetBoolFlag("FFlagDebugGraphicsPreferOpenGL", value);
        }

        public bool DebugSkyGray
        {
            get => GetBoolFlag("FFlagDebugSkyGray");
            set => SetBoolFlag("FFlagDebugSkyGray", value);
        }

        public bool DebugPauseVoxelizer
        {
            get => GetBoolFlag("DFFlagDebugPauseVoxelizer");
            set => SetBoolFlag("DFFlagDebugPauseVoxelizer", value);
        }

        public int DebugFRMQualityLevelOverride
        {
            get => GetIntFlag("DFIntDebugFRMQualityLevelOverride");
            set => SetIntFlag("DFIntDebugFRMQualityLevelOverride", value);
        }

        public int FRMMaxGrassDistance
        {
            get => GetIntFlag("FIntFRMMaxGrassDistance");
            set => SetIntFlag("FIntFRMMaxGrassDistance", value);
        }

        public int FRMMinGrassDistance
        {
            get => GetIntFlag("FIntFRMMinGrassDistance");
            set => SetIntFlag("FIntFRMMinGrassDistance", value);
        }

        // --- UI ---

        public int GrassMovementReducedMotionFactor
        {
            get => GetIntFlag("FIntGrassMovementReducedMotionFactor");
            set => SetIntFlag("FIntGrassMovementReducedMotionFactor", value);
        }
    }
}
