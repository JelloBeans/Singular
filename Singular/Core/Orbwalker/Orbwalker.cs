namespace Singular.Core.Orbwalker
{
    using System.Collections.Generic;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// Handles the orb walking functionality
    /// </summary>
    public partial class Orbwalker
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
        /// Gets or sets the last auto attack time.
        /// </summary>
        /// <value>
        /// The last auto attack time.
        /// </value>
        public float LastAutoAttack { get; set; }

        /// <summary>
        /// Gets or sets the last auto attack move time.
        /// </summary>
        /// <value>
        /// The last auto attack move time.
        /// </value>
        public float LastAutoAttackMove { get; set; }

        /// <summary>
        /// Gets or sets the last movement time.
        /// </summary>
        /// <value>
        /// The last movement time.
        /// </value>
        public float LastMovement { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Orbwalker"/> class.
        /// </summary>
        public Orbwalker()
        {
            this.hotkeysMenu = Singular.GetHotkeysMenu();
            this.MissileMarkers = new List<MissileMarker>();
            DisableLeagueSharpOrbwalker();
        }

        /// <summary>
        /// Disables the league sharp orbwalker.
        /// </summary>
        private static void DisableLeagueSharpOrbwalker()
        {
            Orbwalking.Attack = false;
            Orbwalking.Move = false;
        }
    }
}
