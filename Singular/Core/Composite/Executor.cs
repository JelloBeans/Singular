namespace Singular.Core.Composite
{
    using System;

    using global::Singular.Core.Enum;

    using LeagueSharp;

    /// <summary>
    /// Handles the execution of the tree composites
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Executor"/> class.
        /// </summary>
        public Executor()
        {
            Game.OnUpdate += Executor_Game_OnUpdate;
        }

        /// <summary>
        /// The on update event of <see cref="Game"/>.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Executor_Game_OnUpdate(EventArgs args)
        {
            var singular = Singular.Instance;
            var orbwalker = singular.Orbwalker;

            Composite composite = null;

            switch (orbwalker.ActiveMode)
            {
                case OrbwalkerMode.AutoCarry:
                    composite = singular.GetComposite(BehaviorType.AutoCarry);
                    break;
                case OrbwalkerMode.Mixed:
                    composite = singular.GetComposite(BehaviorType.Mixed);
                    break;
                case OrbwalkerMode.LaneClear:
                    composite = singular.GetComposite(BehaviorType.LaneClear);
                    break;
                case OrbwalkerMode.LastHit:
                    composite = singular.GetComposite(BehaviorType.LastHit);
                    break;
            }

            if (composite != null)
            {
                composite.Tick(singular);
            }
        }
    }
}
