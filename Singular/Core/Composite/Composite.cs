namespace Singular.Core.Composite
{
    using System;

    /// <summary>
    /// The base class of the tree system.
    /// </summary>
    public abstract class Composite : IEquatable<Composite>
    {
        /// <summary>
        /// The unique identifier
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        private readonly Guid guid = Guid.NewGuid();

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid
        {
            get
            {
                return this.guid;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return this.LastStatus.HasValue && this.LastStatus == RunStatus.Running;
            }
        }

        /// <summary>
        /// Gets or sets the last status.
        /// </summary>
        /// <value>
        /// The last status.
        /// </value>
        public RunStatus? LastStatus { get; set; }

        /// <summary>
        /// Executes the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The RunStatus.</returns>
        public abstract RunStatus Execute(Singular ctx);
        
        /// <summary>
        /// Executes the user code of the composite.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <returns>The run status.</returns>
        public virtual RunStatus Tick(Singular ctx)
        {
            if (!this.LastStatus.HasValue || this.LastStatus == RunStatus.Running)
            {
                this.LastStatus = this.Execute(ctx);
            }

            return this.LastStatus.Value;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Composite other)
        {
            return other.Guid.Equals(this.Guid);
        }

        /// <summary>
        /// Clears the last status so the composite can run again.
        /// </summary>
        public virtual void Cleanup()
        {
            this.LastStatus = null;
        }
    }
}
