namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Dynamic;
    using global::Singular.Core.Enum;

    /// <summary>
    /// Creates the Sivir champion composite
    /// </summary>
    [Behavior(BehaviorType.Initialize | BehaviorType.AutoCarry, Champion.Sivir)]
    public partial class Sivir : ChampionComposite
    {
        /// <summary>
        /// Gets the composite.
        /// </summary>
        /// <value>
        /// The composite.
        /// </value>
        public override Composite Composite
        {
            get
            {
                switch (CompositeBuilder.CurrentBehaviorType)
                {
                    case BehaviorType.Initialize:
                        return CreateSivirInitialize();
                    case BehaviorType.AutoCarry:
                        return CreateSivirAutoCarry();
                }

                return null;
            }
        }

    }
}
