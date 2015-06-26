namespace Singular.Core.Orbwalker
{
    using LeagueSharp;

    /// <summary>
    /// The missile manager for the orbwalker, tracks missiles sent by minions and allies
    /// </summary>
    public partial class Orbwalker
    {
        /// <summary>
        /// The missile client created event from <see cref="Orbwalker"/>.
        /// </summary>
        /// <param name="missile">The missile.</param>
        private void Orbwalker_MissileClient_OnCreate(MissileClient missile)
        {
            if (!missile.SpellCaster.IsAlly)
            {
                return;
            }

            if (!(missile.Target is Obj_AI_Base))
            {
                return;
            }

            var marker = new MissileMarker(missile);
            this.MissileMarkers.Add(marker);
        }

        /// <summary>
        /// The event when a <see cref="Obj_AI_Turret"/> attacks a target.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="arg">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_Obj_AI_Turret_OnProcessSpellCast(Obj_AI_Turret sender, GameObjectProcessSpellCastEventArgs arg)
        {
            
        }

        /// <summary>
        /// The game object deleted event of <see cref="GameObject"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_GameObject_OnDelete(GameObject sender, System.EventArgs args)
        {
            this.MissileMarkers.RemoveAll(m => m.MissileClient.NetworkId == sender.NetworkId);
        }

        /// <summary>
        /// The game update event of <see cref="GameObject"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_Game_OnUpdate(System.EventArgs args)
        {
            var gameTime = Game.Time;
            this.MissileMarkers.RemoveAll(m => m.CollisionTime >= gameTime);
        }

    }
}