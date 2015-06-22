namespace Singular.Core.Composite
{
    /// <summary>
    /// A predicate for determining if a decorator can execute
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>
    /// True or false if the decorator can execute
    /// </returns>
    public delegate bool DecoratorDelegate(Singular ctx);

    /// <summary>
    /// A wrapper to determine if a composite can execute
    /// </summary>
    public class Decorator : Composite
    {
        /// <summary>
        /// Gets or sets the decorator delegate.
        /// </summary>
        /// <value>
        /// The decorator delegate.
        /// </value>
        private DecoratorDelegate DecoratorDelegate { get; set; }

        /// <summary>
        /// Gets or sets the composite.
        /// </summary>
        /// <value>
        /// The composite.
        /// </value>
        private Composite Composite { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decorator" /> class.
        /// </summary>
        /// <param name="decoratorDelegate">The decorator delegate.</param>
        /// <param name="composite">The composite.</param>
        public Decorator(DecoratorDelegate decoratorDelegate, Composite composite)
        {
            this.DecoratorDelegate = decoratorDelegate;
            this.Composite = composite;
        }

        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>
        /// The RunStatus.
        /// </returns>
        public override RunStatus Execute(Singular ctx)
        {
            return this.DecoratorDelegate(ctx) ? this.Composite.Tick(ctx) : RunStatus.Failure;
        }

        /// <summary>
        /// Clears the last status so the composite can run again.
        /// </summary>
        public override void Cleanup()
        {
            this.Composite.Cleanup();
            base.Cleanup();
        }
    }
}
