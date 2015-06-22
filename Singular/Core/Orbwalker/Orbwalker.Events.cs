namespace Singular.Core.Orbwalker
{
    using LeagueSharp;

    /// <summary>
    /// Handles the events for the orbwalker
    /// </summary>
    public partial class Orbwalker
    {
        /// <summary>
        /// Attaches the orb walker events.
        /// </summary>
        public void AttachEvents()
        {
            GameObject.OnCreate += this.Orbwalker_Missile_GameObject_OnCreate;
            GameObject.OnDelete += this.Orbwalker_Missile_GameObject_OnDelete;
            Game.OnUpdate += this.Orbwalker_Missile_Game_OnUpdate;
            Drawing.OnDraw += Orbwalker_Drawing_OnDraw;

            // Auto attack events
            Obj_AI_Base.OnProcessSpellCast += this.Orbwalker_Obj_AI_Base_OnProcessSpellCast;
            Spellbook.OnStopCast += this.Orbwalker_Spellbook_OnStopCast;
        }

        /// <summary>
        /// Handles the cancel auto attack (stop cast) event of <see cref="Spellbook"/>
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

        /// <summary>
        /// Handles the auto attack (process spell) event of <see cref="Obj_AI_Base"/>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var abilityName = args.SData.Name;
            if (OrbwalkerHelpers.IsAutoAttack(abilityName))
            {
                this.LastAutoAttack = Game.Time;
                this.LastAutoAttackMove = Game.Time;
            }
        }
    }
}
