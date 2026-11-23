using Fluxor;

namespace Pulse.Web.Store
{
    /// <summary>
    /// Represents an application state
    /// </summary>
    [FeatureState]
    public record PulseWebState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the application state is updating or loading data.
        /// </summary>
        public bool IsLoading { get; init; }

        /// <summary>
        /// Gets or sets the data based on a specified key 
        /// </summary>
        public Dictionary<string, object> Data { get; init; } = new();
    }
}
