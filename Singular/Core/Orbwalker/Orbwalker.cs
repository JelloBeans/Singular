namespace Singular.Core.Orbwalker
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using SingularLibrary;

    using Color = System.Drawing.Color;

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
        /// Gets the active mode.
        /// </summary>
        /// <value>
        /// The active mode.
        /// </value>
        public OrbwalkerMode ActiveMode
        {
            get
            {
                if (this.hotkeysMenu.Item("AutoCarry").GetValue<KeyBind>().Active)
                {
                    return OrbwalkerMode.AutoCarry;
                }

                if (this.hotkeysMenu.Item("Mixed").GetValue<KeyBind>().Active)
                {
                    return OrbwalkerMode.Mixed;
                }

                if (this.hotkeysMenu.Item("LaneClear").GetValue<KeyBind>().Active)
                {
                    return OrbwalkerMode.LaneClear;
                }

                return this.hotkeysMenu.Item("LastHit").GetValue<KeyBind>().Active ? OrbwalkerMode.LastHit : OrbwalkerMode.None;
            }
        }

        /// <summary>
        /// The hotkeys menu
        /// </summary>
        private Menu hotkeysMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="Orbwalker"/> class.
        /// </summary>
        public Orbwalker()
        {
            this.MissileMarkers = new List<MissileMarker>();
            DisableLeagueSharpOrbwalker();
            this.hotkeysMenu = Singular.GetHotkeysMenu();
        }

        /// <summary>
        /// Disables the league sharp orbwalker.
        /// </summary>
        private static void DisableLeagueSharpOrbwalker()
        {
            Orbwalking.Attack = false;
            Orbwalking.Move = false;
        }

        /// <summary>
        /// Attaches the orb walker events.
        /// </summary>
        public void AttachEvents()
        {
            GameObject.OnCreate += this.Orbwalker_GameObject_OnCreate;
            GameObject.OnDelete += this.Orbwalker_GameObject_OnDelete;
            Game.OnUpdate += this.Orbwalker_Game_OnUpdate;
            Drawing.OnDraw += Orbwalker_Drawing_OnDraw;
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
        private static void Orbwalker_Drawing_OnDraw(System.EventArgs args)
        {
            var minions = GameObjects.EnemyMinions.ToList();
            foreach (var minion in minions.Where(m => m.IsHPBarRendered && m.IsValidTarget()))
            {
                var attackDamage = Me.GetAutoAttackDamage(minion, true);

                var healthBar = minion.HPBarPosition;
                var maxHealth = minion.MaxHealth;
                var currentHealth = minion.Health;

                if (currentHealth <= attackDamage)
                {
                    continue;
                }

                var barWidth = healthBar.Length();

                var split = (float)(barWidth / (maxHealth / attackDamage));
                var offset = barWidth - split;
                while (offset > 0)
                {
                    var startVector = new Vector2(healthBar.X + 45 + offset, healthBar.Y + 18);
                    var endVector = new Vector2(healthBar.X + 45 + offset, healthBar.Y + 23);
                    Drawing.DrawLine(startVector, endVector, 1f, Color.White);
                    offset -= split;
                }
            }

            // foreach (var sourceMarkers in this.MissileMarkers.GroupBy(g => g.Target.NetworkId))
            // {
            //    var targetId = sourceMarkers.Key;
            //    foreach (var marker in sourceMarkers)
            //    {
            //        var minion = minions.FirstOrDefault(m => m.NetworkId == targetId);
            //        if (minion == null || !minion.IsHPBarRendered || !minion.IsValidTarget())
            //        {
            //            continue;
            //        }
            //        var attackDamage = Me.GetAutoAttackDamage(minion, true);
            //        var healthBar = minion.HPBarPosition;
            //        var minionMaxHealth = minion.MaxHealth;
            //    }
            // }
        }
    }
}
