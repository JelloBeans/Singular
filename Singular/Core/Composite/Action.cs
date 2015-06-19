namespace Singular.Core.Composite
{
    /// <summary>
    /// The action delegate.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>The run status of the action.</returns>
    public delegate RunStatus ActionDelegate(object ctx);

    /// <summary>
    /// The action success delegate.
    /// </summary>
    /// <param name="ctx">The context.</param>
    public delegate void ActionSuccessDelegate(object ctx);

    /// <summary>
    /// The base action class. Executes actions and returns their status.
    /// </summary>
    public class Action : Composite
    {
        /// <summary>
        /// Gets or sets the action runner.
        /// </summary>
        /// <value>
        /// The action runner.
        /// </value>
        public ActionDelegate ActionRunner { get; set; }

        /// <summary>
        /// Gets or sets the action runner.
        /// </summary>
        /// <value>
        /// The action runner.
        /// </value>
        public ActionSuccessDelegate ActionSuccessRunner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public Action(ActionDelegate action)
        {
            this.ActionRunner = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public Action(ActionSuccessDelegate action)
        {
            this.ActionSuccessRunner = action;
        }

        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The RunStatus.</returns>
        public override RunStatus Execute(object ctx)
        {
            if (this.ActionSuccessRunner != null)
            {
                this.ActionSuccessRunner(ctx);
                return RunStatus.Success;
            }

            return this.ActionRunner != null ? this.ActionRunner(ctx) : RunStatus.Failure;
        }
    }

}
