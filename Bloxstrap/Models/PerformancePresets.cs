using Bloxstrap.Enums.FlagPresets;

namespace Bloxstrap.Models
{
    /// <summary>
    /// Curated one-click performance profiles for Magical Launcher.
    ///
    /// Each preset maps a FastFlag key to its target value. A null value
    /// means "clear this flag (revert to Roblox default)". The <c>Default</c>
    /// preset clears every flag the other presets touch, so applying it is
    /// a clean reset of the performance profile (without touching unrelated
    /// user flags).
    /// </summary>
    public static class PerformancePresets
    {
        /// <summary>
        /// Every FastFlag key any preset may touch. Used by the
        /// <c>Default</c> preset to reset only what we own, leaving
        /// user-set flags in other categories (UI, debug, etc.) intact.
        /// </summary>
        public static readonly IReadOnlyList<string> ManagedKeys = new[]
        {
            // Rendering / graphics backend
            "FFlagDebugGraphicsPreferD3D11",
            "FFlagDebugGraphicsPreferVulkan",
            "FFlagDebugGraphicsPreferOpenGL",
            "DFFlagDisableDPIScale",
            "FIntDebugForceMSAASamples",
            "DFFlagTextureQualityOverrideEnabled",
            "DFIntTextureQualityOverride",

            // Geometry / LOD
            "DFIntCSGLevelOfDetailSwitchingDistance",
            "DFIntCSGLevelOfDetailSwitchingDistanceL12",
            "DFIntCSGLevelOfDetailSwitchingDistanceL23",
            "DFIntCSGLevelOfDetailSwitchingDistanceL34",

            // FRM / grass
            "DFIntDebugFRMQualityLevelOverride",
            "FIntFRMMaxGrassDistance",
            "FIntFRMMinGrassDistance",
            "FIntGrassMovementReducedMotionFactor",

            // Engine tuning
            "FFlagDebugSkyGray",
            "DFFlagDebugPauseVoxelizer"
        };

        public static IReadOnlyDictionary<PerformancePreset, IReadOnlyDictionary<string, string?>> Profiles =>
            new Dictionary<PerformancePreset, IReadOnlyDictionary<string, string?>>
            {
                [PerformancePreset.Default] = ResetProfile(),

                [PerformancePreset.Potato] = new Dictionary<string, string?>
                {
                    // Prefer D3D11 (most stable on weak GPUs)
                    ["FFlagDebugGraphicsPreferD3D11"] = "True",
                    ["FFlagDebugGraphicsPreferVulkan"] = null,
                    ["FFlagDebugGraphicsPreferOpenGL"] = null,
                    // Disable DPI scaling (cheap on integrated)
                    ["DFFlagDisableDPIScale"] = "False",
                    // No MSAA
                    ["FIntDebugForceMSAASamples"] = "1",
                    // Lowest texture quality
                    ["DFFlagTextureQualityOverrideEnabled"] = "True",
                    ["DFIntTextureQualityOverride"] = "0",
                    // Aggressive LOD (close geometry switches early)
                    ["DFIntCSGLevelOfDetailSwitchingDistance"] = "100",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "150",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "250",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "400",
                    // Lowest FRM quality, no grass
                    ["DFIntDebugFRMQualityLevelOverride"] = "1",
                    ["FIntFRMMaxGrassDistance"] = "0",
                    ["FIntFRMMinGrassDistance"] = "0",
                    ["FIntGrassMovementReducedMotionFactor"] = "0",
                    // Pause voxelizer to save CPU
                    ["DFFlagDebugPauseVoxelizer"] = "True",
                    // Flat gray sky (no skybox rendering cost)
                    ["FFlagDebugSkyGray"] = "True"
                },

                [PerformancePreset.Performance] = new Dictionary<string, string?>
                {
                    ["FFlagDebugGraphicsPreferD3D11"] = "True",
                    ["FFlagDebugGraphicsPreferVulkan"] = null,
                    ["FFlagDebugGraphicsPreferOpenGL"] = null,
                    ["DFFlagDisableDPIScale"] = "False",
                    ["FIntDebugForceMSAASamples"] = "2",
                    ["DFFlagTextureQualityOverrideEnabled"] = "True",
                    ["DFIntTextureQualityOverride"] = "1",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"] = "200",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "300",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "500",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "700",
                    ["DFIntDebugFRMQualityLevelOverride"] = "5",
                    ["FIntFRMMaxGrassDistance"] = "50",
                    ["FIntFRMMinGrassDistance"] = "0",
                    ["FIntGrassMovementReducedMotionFactor"] = "1",
                    ["DFFlagDebugPauseVoxelizer"] = null,
                    ["FFlagDebugSkyGray"] = null
                },

                [PerformancePreset.Balanced] = new Dictionary<string, string?>
                {
                    ["FFlagDebugGraphicsPreferD3D11"] = null, // let Roblox pick
                    ["FFlagDebugGraphicsPreferVulkan"] = null,
                    ["FFlagDebugGraphicsPreferOpenGL"] = null,
                    ["DFFlagDisableDPIScale"] = null,
                    ["FIntDebugForceMSAASamples"] = "4",
                    ["DFFlagTextureQualityOverrideEnabled"] = "True",
                    ["DFIntTextureQualityOverride"] = "2",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"] = "400",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "600",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "900",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "1200",
                    ["DFIntDebugFRMQualityLevelOverride"] = "10",
                    ["FIntFRMMaxGrassDistance"] = "500",
                    ["FIntFRMMinGrassDistance"] = "100",
                    ["FIntGrassMovementReducedMotionFactor"] = "-1",
                    ["DFFlagDebugPauseVoxelizer"] = null,
                    ["FFlagDebugSkyGray"] = null
                },

                [PerformancePreset.Quality] = new Dictionary<string, string?>
                {
                    ["FFlagDebugGraphicsPreferD3D11"] = null,
                    ["FFlagDebugGraphicsPreferVulkan"] = null,
                    ["FFlagDebugGraphicsPreferOpenGL"] = null,
                    ["DFFlagDisableDPIScale"] = "True", // preserve quality on HiDPI
                    ["FIntDebugForceMSAASamples"] = "8",
                    ["DFFlagTextureQualityOverrideEnabled"] = "True",
                    ["DFIntTextureQualityOverride"] = "3",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"] = "700",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "1000",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "1400",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "1800",
                    ["DFIntDebugFRMQualityLevelOverride"] = "15",
                    ["FIntFRMMaxGrassDistance"] = "1500",
                    ["FIntFRMMinGrassDistance"] = "300",
                    ["FIntGrassMovementReducedMotionFactor"] = "-1",
                    ["DFFlagDebugPauseVoxelizer"] = null,
                    ["FFlagDebugSkyGray"] = null
                },

                [PerformancePreset.Ultra] = new Dictionary<string, string?>
                {
                    ["FFlagDebugGraphicsPreferD3D11"] = null,
                    ["FFlagDebugGraphicsPreferVulkan"] = null,
                    ["FFlagDebugGraphicsPreferOpenGL"] = null,
                    ["DFFlagDisableDPIScale"] = "True",
                    ["FIntDebugForceMSAASamples"] = "16",
                    ["DFFlagTextureQualityOverrideEnabled"] = "True",
                    ["DFIntTextureQualityOverride"] = "3",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"] = "1200",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "1500",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "1800",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "2000",
                    ["DFIntDebugFRMQualityLevelOverride"] = "21",
                    ["FIntFRMMaxGrassDistance"] = "3000",
                    ["FIntFRMMinGrassDistance"] = "500",
                    ["FIntGrassMovementReducedMotionFactor"] = "-1",
                    ["DFFlagDebugPauseVoxelizer"] = null,
                    ["FFlagDebugSkyGray"] = null
                }
            };

        /// <summary>
        /// Build a reset profile that clears every managed key.
        /// </summary>
        private static IReadOnlyDictionary<string, string?> ResetProfile()
        {
            var dict = new Dictionary<string, string?>();
            foreach (var key in ManagedKeys)
                dict[key] = null;
            return dict;
        }

        /// <summary>
        /// Detect which preset (if any) the current FastFlags match.
        /// Returns <c>Default</c> when nothing matches or flags are mixed.
        /// </summary>
        public static PerformancePreset DetectCurrent(IDictionary<string, object> currentFlags)
        {
            foreach (var (preset, profile) in Profiles)
            {
                if (preset == PerformancePreset.Default)
                    continue;

                bool match = true;
                foreach (var (key, value) in profile)
                {
                    string? current = currentFlags.TryGetValue(key, out var obj) ? obj?.ToString() : null;
                    // Normalize: treat missing key as null
                    string? normalized = string.IsNullOrEmpty(current) ? null : current;
                    if (!string.Equals(normalized, value, StringComparison.Ordinal))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return preset;
            }

            return PerformancePreset.Default;
        }
    }
}
