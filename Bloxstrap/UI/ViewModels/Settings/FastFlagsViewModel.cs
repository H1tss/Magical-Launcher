using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Bloxstrap.Enums.FlagPresets;

namespace Bloxstrap.UI.ViewModels.Settings
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private Dictionary<string, object>? _preResetFlags;

        // ================================================================
        // PERFORMANCE PRESETS (one-click profiles)
        // ================================================================

        public List<string> PerformancePresetLabels { get; } = new()
        {
            "Default (no override)",
            "Potato (lowest quality)",
            "Performance (max FPS)",
            "Balanced (recommended)",
            "Quality (high fidelity)",
            "Ultra (max everything)"
        };

        private static readonly PerformancePreset[] _presetOrder = {
            PerformancePreset.Default,
            PerformancePreset.Potato,
            PerformancePreset.Performance,
            PerformancePreset.Balanced,
            PerformancePreset.Quality,
            PerformancePreset.Ultra
        };

        public string SelectedPerformancePresetLabel
        {
            get
            {
                var detected = Models.PerformancePresets.DetectCurrent(App.FastFlags.Prop);
                int idx = Array.IndexOf(_presetOrder, detected);
                return idx >= 0 ? PerformancePresetLabels[idx] : PerformancePresetLabels[0];
            }
            set
            {
                int idx = PerformancePresetLabels.IndexOf(value);
                if (idx < 0) return;
                ApplyPerformancePreset(_presetOrder[idx]);
                RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(nameof(SelectedPerformancePresetLabel));
            }
        }

        private void ApplyPerformancePreset(PerformancePreset preset)
        {
            if (!Models.PerformancePresets.Profiles.TryGetValue(preset, out var profile))
                return;

            foreach (var (key, value) in profile)
                App.FastFlags.SetValue(key, value);
        }

        public event EventHandler? RequestPageReloadEvent;
        public event EventHandler? OpenFlagEditorEvent;

        private void OpenFastFlagEditor() => OpenFlagEditorEvent?.Invoke(this, EventArgs.Empty);

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public Visibility CanShowFastFlagEditor => App.IsStudioInstalled ? Visibility.Visible : Visibility.Collapsed;

        // ================================================================
        // FAST FLAG MANAGER TOGGLE
        // ================================================================

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
        }

        // ================================================================
        // MSAA — string-based binding (no enum, no converter)
        // ================================================================

        public List<string> MSAALevelLabels { get; } = new()
        {
            "Automatic",
            "1x",
            "2x",
            "4x",
            "8x",
            "16x"
        };

        private static readonly Dictionary<string, string?> _msaaMap = new()
        {
            { "Automatic", null },
            { "1x", "1" },
            { "2x", "2" },
            { "4x", "4" },
            { "8x", "8" },
            { "16x", "16" }
        };

        public string SelectedMSAALevelLabel
        {
            get
            {
                string? current = App.FastFlags.GetPreset("Rendering.MSAA");
                foreach (var (label, val) in _msaaMap)
                    if (val == current)
                        return label;
                return "Automatic";
            }
            set
            {
                if (_msaaMap.TryGetValue(value, out var flagVal))
                    App.FastFlags.SetPreset("Rendering.MSAA", flagVal);
                OnPropertyChanged(nameof(SelectedMSAALevelLabel));
            }
        }

        // ================================================================
        // FIX DISPLAY SCALING (DPI)
        // ================================================================

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null);
        }

        // ================================================================
        // TEXTURE QUALITY — string-based binding
        // ================================================================

        public List<string> TextureQualityLabels { get; } = new()
        {
            "Automatic",
            "Level 0 (lowest)",
            "Level 1",
            "Level 2",
            "Level 3 (highest)"
        };

        private static readonly Dictionary<string, string?> _textureMap = new()
        {
            { "Automatic", null },
            { "Level 0 (lowest)", "0" },
            { "Level 1", "1" },
            { "Level 2", "2" },
            { "Level 3 (highest)", "3" }
        };

        public string SelectedTextureQualityLabel
        {
            get
            {
                string? current = App.FastFlags.GetPreset("Rendering.TextureQuality.Level");
                foreach (var (label, val) in _textureMap)
                    if (val == current)
                        return label;
                return "Automatic";
            }
            set
            {
                if (!_textureMap.TryGetValue(value, out var levelVal))
                    return;

                if (levelVal is null)
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", null);
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", null);
                }
                else
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", "True");
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", levelVal);
                }
                OnPropertyChanged(nameof(SelectedTextureQualityLabel));
            }
        }

        // ================================================================
        // RESET CONFIGURATION
        // ================================================================

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

        // ================================================================
        // INDIVIDUAL FLAG BINDINGS (sliders + toggles)
        // ================================================================

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

        // --- Geometry (CSG LOD) ---

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

        // --- Rendering backend (toggles) ---

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

        // --- FRM / grass (sliders) ---

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

        public int GrassMovementReducedMotionFactor
        {
            get => GetIntFlag("FIntGrassMovementReducedMotionFactor");
            set => SetIntFlag("FIntGrassMovementReducedMotionFactor", value);
        }
    }
}
