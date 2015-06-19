namespace Singular.Core.Composite
{
    /// <summary>
    /// The status of a composite routine.
    /// </summary>
    public enum RunStatus
    {
        /// <summary>
        /// The failure status
        /// </summary>
        Failure,

        /// <summary>
        /// The running status
        /// </summary>
        Running,

        /// <summary>
        /// The success status
        /// </summary>
        Success,

        /// <summary>
        /// The cancelled status
        /// </summary>
        Cancelled
    }
}
