namespace Singular.Core.Composite.Helpers
{
    /// <summary>
    /// An action that will always return a RunStatus of Failure
    /// </summary>
    public class ActionAlwaysFail : Action
    {
        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The RunStatus.</returns>
        public override RunStatus Execute(Singular ctx)
        {
            return RunStatus.Failure;
        }
    }
}
