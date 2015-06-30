namespace Singular.Champion
{
    using global::Singular.Core.Composite;

    using LeagueSharp.Common;

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

        /// <summary>
        /// Gets or sets the q ability.
        /// </summary>
        /// <value>
        /// The q ability.
        /// </value>
        internal Spell Q { get; set; }

        /// <summary>
        /// Gets or sets the w ability.
        /// </summary>
        /// <value>
        /// The w ability.
        /// </value>
        internal Spell W { get; set; }

        /// <summary>
        /// Gets or sets the e ability
        /// </summary>
        /// <value>
        /// The e ability.
        /// </value>
        internal Spell E { get; set; }

        /// <summary>
        /// Gets or sets the r ability.
        /// </summary>
        /// <value>
        /// The r ability.
        /// </value>
        internal Spell R { get; set; }
    }
}
