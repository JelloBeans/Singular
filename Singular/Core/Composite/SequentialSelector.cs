namespace Singular.Core.Composite
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The sequential selector executes composites in a sequential order, when a status is successful it stops executing.
    /// </summary>
    public class SequentialSelector : Composite
    {
        /// <summary>
        /// Gets or sets the composites.
        /// </summary>
        /// <value>
        /// The composites.
        /// </value>
        private List<Composite> Composites { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialSelector"/> class.
        /// </summary>
        public SequentialSelector()
        {
            this.Composites = new List<Composite>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialSelector"/> class.
        /// </summary>
        /// <param name="composites">The composites.</param>
        public SequentialSelector(params Composite[] composites)
        {
            this.Composites = new List<Composite>();
            this.Composites.AddRange(composites);
        }

        /// <summary>
        /// Adds the child composite.
        /// </summary>
        /// <param name="composite">The composite.</param>
        public void AddChild(Composite composite)
        {
            this.Composites.Add(composite);
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
            var hasSuccess = false;
            foreach (var status in this.Composites.Select(composite => composite.Tick(ctx)))
            {
                if (status == RunStatus.Running)
                {
                    return status;
                }

                hasSuccess = status == RunStatus.Success || hasSuccess;
            }

            return hasSuccess ? RunStatus.Success : RunStatus.Failure;
        }

        /// <summary>
        /// Clears the last status so the composite can run again.
        /// </summary>
        public override void Cleanup()
        {
            this.Composites.ForEach(c => c.Cleanup());
            base.Cleanup();
        }
    }
}
