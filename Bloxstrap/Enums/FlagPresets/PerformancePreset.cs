namespace Bloxstrap.Enums.FlagPresets
{
    /// <summary>
    /// One-click performance profiles that coordinate many FastFlags at once.
    /// Each preset applies a curated batch of rendering, geometry, and engine
    /// flags tuned for a specific hardware target / use case.
    /// </summary>
    public enum PerformancePreset
    {
        /// <summary>Leave all flags untouched.</summary>
        Default,

        /// <summary>Lowest possible quality. For toasters and integrated GPUs.</summary>
        Potato,

        /// <summary>Max FPS on low-end / laptop GPUs. Drops eye-candy hard.</summary>
        Performance,

        /// <summary>Good visuals, still smooth. Recommended for most PCs.</summary>
        Balanced,

        /// <summary>High fidelity for mid-to-high GPUs.</summary>
        Quality,

        /// <summary>Everything maxed. Needs a strong GPU.</summary>
        Ultra
    }
}
