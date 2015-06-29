namespace Singular.Core.Orbwalker
{
    using System.Linq;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SingularLibrary;

    /// <summary>
    /// The target selector for the orbwalker
    /// </summary>
    public partial class Orbwalker
    {

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <param name="mode">The active orbwalker mode.</param>
        /// <returns></returns>
        public AttackableUnit GetTarget(OrbwalkerMode mode)
        {
            AttackableUnit target = null;

            if (mode == OrbwalkerMode.LaneClear || mode == OrbwalkerMode.LastHit || mode == OrbwalkerMode.Mixed)
            {
                target = this.GetLastHitMinionTarget();
            }

            return target;
        }

        /// <summary>
        /// Gets the minion target.
        /// </summary>
        /// <returns></returns>
        private AttackableUnit GetLastHitMinionTarget()
        {
            var attackCastDelay = Me.GetHeroAttackCastDelay();
            var projectileSpeed = Me.GetProjectileSpeed();

            AttackableUnit target = null;

            // Prioritize Super > Cannon > Melee > Range
            // Although we prioritize we still want to fire non killable minion events so we cannot return the first killable minion.
            foreach (var minion in GameObjects.EnemyMinions.Where(m => m.IsValidTarget()).OrderByDescending(m => m.MaxHealth))
            {
                var current = minion;

                var damage = Me.GetAutoAttackDamage(current, true);

                var markers = this.MissileMarkers.Where(m => m.Target.NetworkId == current.NetworkId);
                if (!markers.Any())
                {
                    if (damage < current.Health && (target == null || target.MaxHealth < current.MaxHealth))
                    {
                        target = current;
                    }
                    continue;
                }

                var distance = Me.ServerPosition.Distance(minion.ServerPosition);
                var myProjectileTime = Me.TimeUntilAutoAttackReady(this.LastAutoAttack) + OrbwalkerHelpers.GetGameTickCount() + attackCastDelay;
                if (!Me.IsMeele)
                {
                    myProjectileTime += (int)(1000 * (distance / projectileSpeed));
                }

                var prediction = current.GetPredictedHealth(this, myProjectileTime);
                if (prediction <= 0)
                {
                    // Fire non killable minion event
                }

                if (prediction > 0 && prediction < damage && (target == null || target.MaxHealth < current.MaxHealth))
                {
                    target = current;
                }
            }

            return target;
        }

    }
}
