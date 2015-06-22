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
            Obj_AI_Base.OnProcessSpellCast += Executor_Obj_AI_Base_OnProcessSpellCast;
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

        /// <summary>
        /// Handles the auto attack (process spell) event of <see cref="Obj_AI_Base"/>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Executor_Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var abilityName = args.SData.Name;
            if (OrbwalkerHelpers.IsAutoAttack(abilityName))
            {
                var composite = Singular.GetComposite(BehaviorType.AfterAttack);
                if (composite != null)
                {
                    composite.Tick(Singular);
                }
            }
        }
    }
}
