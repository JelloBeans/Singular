namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Composite.Helpers;

    /// <summary>
    /// Handles the auto carry behavior for Sivir
    /// </summary>
    public partial class Sivir
    {
        /// <summary>
        /// Creates the auto carry behavior for Sivir 
        /// </summary>
        /// <returns>The composite for Sivir auto carry behavior.</returns>
        public static Composite CreateSivirAutoCarry()
        {
            return new SequentialSelector(new Decorator(ctx => true, new ActionAlwaysSuccess()));
        }
    }
}
