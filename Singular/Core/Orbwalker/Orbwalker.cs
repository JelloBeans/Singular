namespace Singular.Core.Orbwalker
{
    using System.Collections.Generic;

    using LeagueSharp;

    /// <summary>
    /// Handles the orb walking functionality
    /// </summary>
    public class Orbwalker
    {

        /// <summary>
        /// Gets the local player.
        /// </summary>
        /// <value>
        /// The local player.
        /// </value>
        private static Obj_AI_Hero Me
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        /// <summary>
        /// Gets or sets the missile markers.
        /// </summary>
        /// <value>
        /// The missile markers.
        /// </value>
        public List<MissileMarker> MissileMarkers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Orbwalker"/> class.
        /// </summary>
        public Orbwalker()
        {
            this.MissileMarkers = new List<MissileMarker>();
            DisableLeagueSharpOrbwalker();
        }

        /// <summary>
        /// Disables the league sharp orbwalker.
        /// </summary>
        private static void DisableLeagueSharpOrbwalker()
        {
            LeagueSharp.Common.Orbwalking.Attack = false;
            LeagueSharp.Common.Orbwalking.Move = false;
        }

        /// <summary>
        /// Attaches the orb walker events.
        /// </summary>
        public void AttachEvents()
        {
            GameObject.OnCreate += this.Orbwalker_GameObject_OnCreate;
            GameObject.OnDelete += this.Orbwalker_GameObject_OnDelete;
            Game.OnUpdate += this.Orbwalker_Game_OnUpdate;
            Drawing.OnDraw += this.Orbwalker_Drawing_OnDraw;
        }

        /// <summary>
        /// Handles the game object created event.
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
        private void Orbwalker_GameObject_OnDelete(GameObject sender, System.EventArgs args)
        {
        }

        /// <summary>
        /// Handles the game update event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Game_OnUpdate(System.EventArgs args)
        {
            var gameTime = Game.Time;
            this.MissileMarkers.RemoveAll(m => m.CollisionTime >= gameTime);
        }

        /// <summary>
        /// Handles the drawing event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Orbwalker_Drawing_OnDraw(System.EventArgs args)
        {
            //Drawing.DrawCircle(Me.Position, Me.GetRealAutoAttackRange(), Color.Blue);
        }
    }
}
