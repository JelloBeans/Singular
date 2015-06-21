namespace Singular.Core.Dynamic
{
    using System;

    using global::Singular.Core.Enum;

    /// <summary>
    /// The behavior attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class BehaviorAttribute : Attribute
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public BehaviorType Type { get; private set; }

        /// <summary>
        /// Gets the champion.
        /// </summary>
        /// <value>
        /// The champion.
        /// </value>
        public Champion Champion { get; private set; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="champion">The champion.</param>
        /// <param name="priority">The priority.</param>
        public BehaviorAttribute(BehaviorType type, Champion champion, int priority = 0)
        {
            this.Type = type;
            this.Champion = champion;
            this.Priority = priority;
        }
    }
}
