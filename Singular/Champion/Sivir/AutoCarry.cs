namespace Singular.Champion.Sivir
{
    using global::Singular.Core.Composite;
    using global::Singular.Core.Dynamic;
    using global::Singular.Core.Enum;

    /// <summary>
    /// Handles the auto carry behavior for Sivir
    /// </summary>
    public partial class Sivir
    {
        /// <summary>
        /// Creates the auto carry behavior for Sivir 
        /// </summary>
        /// <returns>The composite for Sivir auto carry behavior.</returns>
        [Behavior(BehaviorType.AutoCarry, Champion.Sivir)]
        public static Composite CreateSivirAutoCarry()
        {
            return new SequentialSelector(
                new Decorator(
                    ctx => true,
                    new Action(a => RunStatus.Success)));
        }
    }
}
