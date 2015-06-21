namespace Singular.Champion
{
    using global::Singular.Core.Composite;

    /// <summary>
    /// The base class for any champion
    /// </summary>
    public abstract class ChampionComposite
    {
        /// <summary>
        /// Gets the composite.
        /// </summary>
        /// <value>
        /// The composite.
        /// </value>
        public abstract Composite Composite { get; }
    }
}
