namespace Singular.Core.Orbwalker
{
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
        /// The after auto attack event.
        /// </summary>
        public static event OnAfterAutoAttackHandler OnAfterAutoAttack;

        /// <summary>
        /// Attaches the orb walker events.
        /// </summary>
        public void AttachEvents()
        {
            GameObject.OnCreate += this.Orbwalker_GameObject_OnCreate;
            GameObject.OnDelete += this.Orbwalker_Missile_GameObject_OnDelete;
            Game.OnUpdate += this.Orbwalker_Missile_Game_OnUpdate;
            Drawing.OnDraw += Orbwalker_Drawing_OnDraw;
            
            Obj_AI_Base.OnProcessSpellCast += this.Orbwalker_Obj_AI_Base_OnProcessSpellCast;
            Spellbook.OnStopCast += this.Orbwalker_Spellbook_OnStopCast;
        }

        /// <summary>
        /// Resets the last auto attack move timer and fires the after auto attack event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="target">The target.</param>
        private void AfterAutoAttack(AttackableUnit sender, AttackableUnit target)
        {
            this.LastAutoAttackMove = 0;
            if (OnAfterAutoAttack != null)
            {
                OnAfterAutoAttack(sender, target);
            }
        }

        /// <summary>
        /// The create game object event of <see cref="GameObject"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_GameObject_OnCreate(GameObject sender, System.EventArgs args)
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

            this.AfterAutoAttack(missile.SpellCaster, target);
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

            this.LastAutoAttack = Game.Time;
            this.LastAutoAttackMove = Game.Time;

            if (!sender.IsMeele)
            {
                return;
            }

            var attackCastDelay = sender.AttackCastDelay;
            var actionDelay = (attackCastDelay * 1000);
            Utility.DelayAction.Add((int)actionDelay, () => this.AfterAutoAttack(sender, (AttackableUnit)args.Target));
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
            this.LastAutoAttackMove = 0;
        }
    }
}
