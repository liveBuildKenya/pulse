using Pulse.Web.Store;

namespace Pulse.Web.Common
{
    /// <summary>
    /// Represents application helper and extension methods
    /// </summary>
    public static class Extensions
    {
        public static T Get<T>(this PulseWebState pulseWebState, string key)
        {
            return pulseWebState.Data.TryGetValue(key, out var value) &&
                value is T result ? result : default;
        }
    }
}
