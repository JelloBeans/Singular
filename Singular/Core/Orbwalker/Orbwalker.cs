namespace Singular.Core.Orbwalker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

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

        #region Menu Variables

        // TODO: Convert these variables to menu

        /// <summary>
        /// The movemeny delay
        /// </summary>
        private const int MovemenyDelay = 100;

        /// <summary>
        /// The hold area radius
        /// </summary>
        private const int HoldAreaRadius = 100 * 100;

        /// <summary>
        /// The minimum movement distance
        /// </summary>
        private const float MinimumMovementDistance = 400;

        #endregion

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
        public int LastAutoAttack { get; set; }

        /// <summary>
        /// Gets or sets the last movement time.
        /// </summary>
        /// <value>
        /// The last movement time.
        /// </value>
        public int LastMovement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [missile launched].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [missile launched]; otherwise, <c>false</c>.
        /// </value>
        public bool MissileLaunched { get; set; }

        /// <summary>
        /// The random instance.
        /// </summary>
        private readonly Random random = new Random();
        
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

        /// <summary>
        /// Orbwalks the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="destination">The destination.</param>
        private void Orbwalk(AttackableUnit target, Vector3 destination)
        {
            if (!target.IsValidTarget() || !Me.IsAutoAttackReady(this.LastAutoAttack))
            {
                this.Move(destination);
                return;
            }

            BeforeAutoAttack(Me, target);
            this.LastAutoAttack = OrbwalkerHelpers.GetGameTickCount() + Me.GetHeroAttackCastDelay();
            this.MissileLaunched = false;

            Me.IssueOrder(GameObjectOrder.AttackUnit, target);
        }

        /// <summary>
        /// Moves to the specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        private void Move(Vector3 destination)
        {
            if (!this.MissileLaunched && !Me.IsMoveReady(this.LastAutoAttack))
            {
                return;
            }

            var tickCount = OrbwalkerHelpers.GetGameTickCount();
            if (tickCount - this.LastMovement < MovemenyDelay)
            {
                return;
            }

            if (Me.ServerPosition.Distance(destination, true) < HoldAreaRadius)
            {
                if (Me.Path.Count() <= 1)
                {
                    return;
                }

                Me.IssueOrder(GameObjectOrder.Stop, Me.ServerPosition);
                Me.IssueOrder(GameObjectOrder.HoldPosition, Me.ServerPosition);
                return;
            }

            var minimumDistance = (this.random.NextFloat(0.6f, 1) + 0.2f) * MinimumMovementDistance;
            var vector3D = (destination.To2D() - Me.ServerPosition.To2D()).Normalized().To3D();
            var position = Me.ServerPosition + minimumDistance * vector3D;

            Me.IssueOrder(GameObjectOrder.MoveTo, position);

            this.LastMovement = OrbwalkerHelpers.GetGameTickCount();
        }
    }
}
