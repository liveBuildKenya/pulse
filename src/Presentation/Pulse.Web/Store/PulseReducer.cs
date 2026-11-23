using Fluxor;

namespace Pulse.Web.Store
{
    /// <summary>
    /// Represents a state reducer
    /// </summary>
    public static class PulseReducer
    {
        [ReducerMethod]
        public static PulseWebState ReducePulseAction(PulseWebState pulseWebState, PulseAction action)
        {
            return pulseWebState with
            {
                Data = new Dictionary<string, object>(pulseWebState.Data)
                {
                    [action.Name] = action.Payload
                }
            };
        }
    }
}
