namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Composite.Helpers;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// Handles the initialize behavior for Sivir
    /// </summary>
    public partial class Sivir
    {
        /// <summary>
        /// Creates the initialize behavior for Sivir.
        /// </summary>
        /// <returns>The composite for Sivir initialize behavior.</returns>
        public Composite CreateSivirInitialize()
        {
            return new SequentialSelector(
                new Action(a =>
                {
                    this.Q = new Spell(SpellSlot.Q);
                })
            );
        }
    }
}
