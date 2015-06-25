namespace Singular.Core.Composite
{
    using System;

    using global::Singular.Core.Enum;
    using global::Singular.Core.Orbwalker;

    using LeagueSharp;

    /// <summary>
    /// Handles the execution of the tree composites
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Gets the singular instance.
        /// </summary>
        /// <value>
        /// The singular instance.
        /// </value>
        private static Singular Singular
        {
            get
            {
                return Singular.Instance;
            }
        }

        /// <summary>
        /// Attaches the events.
        /// </summary>
        public void AttachEvents()
        {
            Game.OnUpdate += Executor_Game_OnUpdate;
            Orbwalker.OnAfterAutoAttack += Executor_Orbwalker_OnAfterAutoAttack;
        }

        /// <summary>
        /// The after auto attack event of <see cref="Orbwalker"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        private static void Executor_Orbwalker_OnAfterAutoAttack(AttackableUnit sender, AttackableUnit target)
        {
            var composite = Singular.GetComposite(BehaviorType.AfterAttack);
            if (composite != null)
            {
                composite.Tick(Singular);
            }
        }

        /// <summary>
        /// The on update event of <see cref="Game"/>.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Executor_Game_OnUpdate(EventArgs args)
        {
            var orbwalker = Singular.Orbwalker;

            Composite composite = null;

            switch (orbwalker.ActiveMode)
            {
                case OrbwalkerMode.AutoCarry:
                    composite = Singular.GetComposite(BehaviorType.AutoCarry);
                    break;
                case OrbwalkerMode.Mixed:
                    composite = Singular.GetComposite(BehaviorType.Mixed);
                    break;
                case OrbwalkerMode.LaneClear:
                    composite = Singular.GetComposite(BehaviorType.LaneClear);
                    break;
                case OrbwalkerMode.LastHit:
                    composite = Singular.GetComposite(BehaviorType.LastHit);
                    break;
            }

            if (composite != null)
            {
                composite.Tick(Singular);
            }
        }
    }
}
