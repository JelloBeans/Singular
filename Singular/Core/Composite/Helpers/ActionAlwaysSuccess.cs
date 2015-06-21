namespace Singular.Core.Composite.Helpers
{
    /// <summary>
    /// An action that will always return a RunStatus of Success
    /// </summary>
    public class ActionAlwaysSuccess : Action
    {
        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The RunStatus.</returns>
        public override RunStatus Execute(object ctx)
        {
            return RunStatus.Success;
        }
    }
}
