namespace Singular.Core.Orbwalker
{
    using System.Linq;

    using global::Singular.Core.Enum;

    using LeagueSharp;

    /// <summary>
    /// Extensions for the orb walker class.
    /// </summary>
    public static class OrbwalkerHelpers
    {
        /// <summary>
        /// Determines whether [is automatic attack] [the specified ability].
        /// </summary>
        /// <param name="ability">The ability.</param>
        /// <returns>True if is auto attack else false.</returns>
        public static bool IsAutoAttack(string ability)
        {
            var lowerAbility = ability.ToLower();

            NoAttackAbility noAttackAbility;
            if (System.Enum.TryParse(lowerAbility, true, out noAttackAbility))
            {
                return false;
            }
            
            if (lowerAbility.Contains("attack"))
            {
                return true;
            }

            AttackAbility attackAbility;
            return System.Enum.TryParse(lowerAbility, true, out attackAbility);
        }

        /// <summary>
        /// Gets the projectile speed.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The projectile speed of the source.</returns>
        public static float GetProjectileSpeed(this Obj_AI_Base source)
        {
            var hero = source as Obj_AI_Hero;
            if (hero != null)
            {
                return hero.IsMeele || hero.ChampionName == "Azir" ? float.MaxValue : hero.BasicAttack.MissileSpeed;
            }

            return source.IsMeele ? float.MaxValue : source.BasicAttack.MissileSpeed;
        }

        /// <summary>
        /// Time until the source dies.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="orbwalker">The orb walker.</param>
        /// <returns>The time until the source dies.</returns>
        public static int TimeTillDeath(this Obj_AI_Base source, Orbwalker orbwalker)
        {
            var health = (double)source.Health;
            foreach (var marker in orbwalker.MissileMarkers.Where(m => m.Target.NetworkId == source.NetworkId).OrderBy(m => m.CollisionTime))
            {
                // Subtract the damage from the source health.
                health -= marker.Damage;

                // If unit will die return the collision time minus the current game time
                if (health <= 0)
                {
                    return (int)(marker.CollisionTime - Game.Time);
                }
            }

            return int.MaxValue;
        }

        /// <summary>
        /// Gets the missile count.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="orbwalker">The orb walker.</param>
        /// <returns>
        /// Get the missile count for a source.
        /// </returns>
        public static int GetMissileCount(this Obj_AI_Base source, Orbwalker orbwalker)
        {
            return orbwalker.MissileMarkers.Count(m => m.Target.NetworkId == source.NetworkId);
        }

        /// <summary>
        /// Gets the missile damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="orbwalker">The orb walker.</param>
        /// <returns>
        /// Gets the missile damage for a source.
        /// </returns>
        public static double GetMissileDamage(this Obj_AI_Base source, Orbwalker orbwalker)
        {
            return orbwalker.MissileMarkers.Where(m => m.Target.NetworkId == source.NetworkId).Sum(m => m.Damage);
        }
    }
}
