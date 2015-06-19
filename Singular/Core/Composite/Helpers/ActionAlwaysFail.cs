namespace Singular.Core.Composite.Helpers
{
    /// <summary>
    /// An action that will always return a RunStatus of Failure
    /// </summary>
    public class ActionAlwaysFail : Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionAlwaysFail"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionAlwaysFail(ActionDelegate action)
            : base(action)
        {
        }

        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The RunStatus.</returns>
        public override RunStatus Execute(object ctx)
        {
            if (this.ActionRunner != null)
            {
                this.ActionRunner(ctx);
            }

            return RunStatus.Failure;
        }
    }
}
