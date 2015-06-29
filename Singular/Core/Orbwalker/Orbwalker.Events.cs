namespace Singular.Core.Orbwalker
{
    using System;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// Handles the events for the orbwalker
    /// </summary>
    public partial class Orbwalker
    {
        /// <summary>
        /// The handler for the after auto attack event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        public delegate void OnAfterAutoAttackHandler(AttackableUnit sender, AttackableUnit target);

        /// <summary>
        /// The handler for the before auto attack event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        public delegate void OnBeforeAutoAttackHandler(AttackableUnit sender, AttackableUnit target);

        /// <summary>
        /// The after auto attack event.
        /// </summary>
        public static event OnAfterAutoAttackHandler OnAfterAutoAttack;

        /// <summary>
        /// The before auto attack event.
        /// </summary>
        public static event OnBeforeAutoAttackHandler OnBeforeAutoAttack;

        /// <summary>
        /// Attaches the orb walker events.
        /// </summary>
        public void AttachEvents()
        {
            GameObject.OnCreate += this.Orbwalker_GameObject_OnCreate;
            GameObject.OnDelete += this.Orbwalker_Missile_GameObject_OnDelete;
            Game.OnUpdate += this.Orbwalker_Game_OnUpdate;
            Drawing.OnDraw += Orbwalker_Drawing_OnDraw;
            
            Obj_AI_Base.OnProcessSpellCast += this.Orbwalker_Obj_AI_Base_OnProcessSpellCast;
            Spellbook.OnStopCast += this.Orbwalker_Spellbook_OnStopCast;
        }

        /// <summary>
        /// Fires the after auto attack event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        private static void AfterAutoAttack(AttackableUnit sender, AttackableUnit target)
        {
            if (OnAfterAutoAttack != null)
            {
                OnAfterAutoAttack(sender, target);
            }
        }

        /// <summary>
        /// Fires the before auto attack event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        private static void BeforeAutoAttack(AttackableUnit sender, AttackableUnit target)
        {
            if (OnBeforeAutoAttack != null)
            {
                OnBeforeAutoAttack(sender, target);
            }
        }

        /// <summary>
        /// The on update event of a game.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Game_OnUpdate(EventArgs args)
        {
            this.Orbwalker_Missile_Game_OnUpdate();

            var activeMode = this.ActiveMode;
            if (activeMode == OrbwalkerMode.None)
            {
                return;
            }

            var target = this.GetTarget(activeMode);
            if (target != null)
            {
                this.Orbwalk(target, Game.CursorPos);
            }
        }

        /// <summary>
        /// The create game object event of <see cref="GameObject"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid)
            {
                return;
            }

            var missile = sender as MissileClient;
            if (missile == null)
            {
                return;
            }

            this.Orbwalker_MissileClient_OnCreate(missile);

            if (!missile.SpellCaster.IsMe)
            {
                return;
            }

            var target = missile.Target as Obj_AI_Hero;

            var abilityName = missile.SData.Name;
            if (!OrbwalkerHelpers.IsAutoAttack(abilityName) || target == null)
            {
                return;
            }

            this.MissileLaunched = true;

            AfterAutoAttack(missile.SpellCaster, target);
        }

        /// <summary>
        /// The auto attack (process spell) event of <see cref="Obj_AI_Base"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var turret = sender as Obj_AI_Turret;
            if (turret != null)
            {
                this.Orbwalker_Missile_Obj_AI_Turret_OnProcessSpellCast(turret, args);
                return;
            }

            if (!sender.IsMe)
            {
                return;
            }

            var abilityName = args.SData.Name;
            if (!OrbwalkerHelpers.IsAutoAttack(abilityName))
            {
                return;
            }

            this.LastAutoAttack = OrbwalkerHelpers.GetGameTickCount();

            if (!sender.IsMeele)
            {
                return;
            }

            var attackCastDelay = sender.AttackCastDelay;
            var actionDelay = (attackCastDelay * 1000);
            Utility.DelayAction.Add((int)actionDelay, () => AfterAutoAttack(sender, (AttackableUnit)args.Target));
        }

        /// <summary>
        /// The cancel auto attack (stop cast) event of <see cref="Spellbook"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellbookStopCastEventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Spellbook_OnStopCast(Spellbook sender, SpellbookStopCastEventArgs args)
        {
            if (!sender.Owner.IsMe || !args.DestroyMissile)
            {
                return;
            }

            this.LastAutoAttack = 0;
        }
    }
}
