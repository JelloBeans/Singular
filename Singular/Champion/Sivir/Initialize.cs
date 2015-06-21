namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Composite.Helpers;

    /// <summary>
    /// Handles the initialize behavior for Sivir
    /// </summary>
    public partial class Sivir
    {
        /// <summary>
        /// Creates the initialize behavior for Sivir.
        /// </summary>
        /// <returns>The composite for Sivir initialize behavior.</returns>
        public static Composite CreateSivirInitialize()
        {
            return new SequentialSelector(new Decorator(ctx => true, new ActionAlwaysSuccess()));
        }
    }
}
