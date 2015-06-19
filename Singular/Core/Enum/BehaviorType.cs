namespace Singular.Core.Enum
{
    using System;

    /// <summary>
    /// The behavior type flags
    /// </summary>
    [Flags]
    public enum BehaviorType
    {
        /// <summary>
        /// The idle type
        /// </summary>
        Idle      = 1 << 0,

        /// <summary>
        /// The last hit type
        /// </summary>
        LastHit   = 1 << 1,

        /// <summary>
        /// The lane clear type
        /// </summary>
        LaneClear = 1 << 2,

        /// <summary>
        /// The jungle clear type
        /// </summary>
        JungleClear = 1 << 3,

        /// <summary>
        /// The mixed type
        /// </summary>
        Mixed = 1 << 4,

        /// <summary>
        /// The auto carry type
        /// </summary>
        AutoCarry = 1 << 5,

        /// <summary>
        /// The dead type
        /// </summary>
        Dead     = 1 << 6,

        /// <summary>
        /// The initialize type
        /// </summary>
        Initialize = 1 << 7,

        /// <summary>
        /// The before attack type
        /// </summary>
        BeforeAttack = 1 << 8,

        /// <summary>
        /// The after attack type
        /// </summary>
        AfterAttack = 1 << 9
    }
}
