namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Dynamic;
    using global::Singular.Core.Enum;

    /// <summary>
    /// Handles the initialize behavior for Sivir
    /// </summary>
    public partial class Sivir
    {
        /// <summary>
        /// Creates the initialize behavior for Sivir.
        /// </summary>
        /// <returns>The composite for Sivir initialize behavior.</returns>
        [Behavior(BehaviorType.Initialize, Champion.Sivir)]
        public static Composite CreateSivirInitialize()
        {
            return new SequentialSelector(
                new Decorator(
                    ctx => true,
                    new Action(a => RunStatus.Success)));
        }
    }
}
