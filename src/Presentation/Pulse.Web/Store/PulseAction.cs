namespace Pulse.Web.Store
{
    /// <summary>
    /// Represents an application event
    /// </summary>    
    public class PulseAction
    {
        /// <summary>
        /// Gets or sets the action type
        /// </summary>
        public ApplicationActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the payload
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// Gets or sets the correlation guid
        /// </summary>
        public Guid CorrelationGuid
        {
            get
            {
                return Guid.CreateVersion7();
            }
        }

        /// <summary>
        /// Gets or sets the timestamp
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }

    /// <summary>
    /// Represents the application action type
    /// </summary>
    public enum ApplicationActionType
    {
        /// <summary>
        /// Represents an action to execute a command
        /// </summary>
        Command,

        /// <summary>
        /// Represents the result of a command
        /// </summary>
        CommandEvent
    }
}
