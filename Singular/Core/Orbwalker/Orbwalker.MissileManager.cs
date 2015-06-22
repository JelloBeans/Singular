namespace Singular.Core.Orbwalker
{
    using System.Linq;

    using LeagueSharp;

    /// <summary>
    /// Handles the missiles sent by minions
    /// </summary>
    public partial class Orbwalker
    {
        /// <summary>
        /// Handles the game object created event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_GameObject_OnCreate(GameObject sender, System.EventArgs args)
        {
            if (!sender.IsValid)
            {
                return;
            }

            var missile = sender as MissileClient;
            if (missile != null)
            {
                this.Orbwalker_MissileClient_OnCreate(missile);
            }
        }

        /// <summary>
        /// Handles the missile client from game object created event.
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
        /// Handles the game object deleted event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_GameObject_OnDelete(GameObject sender, System.EventArgs args)
        {
        }

        /// <summary>
        /// Handles the game update event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Missile_Game_OnUpdate(System.EventArgs args)
        {
            var gameTime = Game.Time;
            this.MissileMarkers.RemoveAll(m => m.CollisionTime >= gameTime);
        }

    }
}
