namespace Singular
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Singular.Core.Composite;
    using global::Singular.Core.Dynamic;
    using global::Singular.Core.Enum;

    /// <summary>
    /// Creates and stores the tree behaviors
    /// </summary>
    public partial class Singular
    {

        /// <summary>
        /// A dictionary containing the composite for a specific behavior type.
        /// </summary>
        private readonly Dictionary<BehaviorType, Composite> behaviorComposites = new Dictionary<BehaviorType, Composite>();

        /// <summary>
        /// Creates the behaviors for the current champion
        /// </summary>
        /// <returns>True if successful else false.</returns>
        public bool CreateBehaviors()
        {
            CompositeBuilder.InvokeInitializers(this.Champion);

            foreach (var type in Enum.GetValues(typeof(BehaviorType)).Cast<BehaviorType>())
            {
                this.GetComposite(type);
            }

            return this.behaviorComposites.ContainsKey(BehaviorType.AutoCarry);
        }

        /// <summary>
        /// Determines whether the specified type has a composite.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The composite for the specified type.</returns>
        public Composite GetComposite(BehaviorType type)
        {
            if (this.behaviorComposites.ContainsKey(type))
            {
                return this.behaviorComposites[type];
            }

            int count;
            var composite = CompositeBuilder.GetComposite(this.Champion, type, out count);
            if (composite == null || count <= 0)
            {
                return null;
            }

            this.behaviorComposites.Add(type, composite);

            return composite;
        }
    }
}
