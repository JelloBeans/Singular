namespace Singular.Core.Orbwalker
{
    using System.Linq;

    using global::Singular.Core.Enum;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    /// Extensions for the orb walker class.
    /// </summary>
    public static class OrbwalkerHelpers
    {
        /// <summary>
        /// Gets the game ping.
        /// </summary>
        /// <returns></returns>
        public static int GetGamePing()
        {
            return Game.Ping / 2;
        }

        /// <summary>
        /// Gets the game tick count.
        /// </summary>
        /// <returns></returns>
        public static int GetGameTickCount()
        {
            return Utils.GameTimeTickCount + GetGamePing();
        }

        /// <summary>
        /// Gets the hero auto attack delay.
        /// </summary>
        /// <param name="hero">The hero.</param>
        /// <returns>
        /// The player attack delay
        /// </returns>
        public static int GetHeroAutoAttackDelay(this Obj_AI_Hero hero)
        {
            return (int)(hero.AttackDelay * 1000);
        }

        /// <summary>
        /// Gets the hero attack cast delay.
        /// </summary>
        /// <param name="hero">The hero.</param>
        /// <returns></returns>
        public static int GetHeroAttackCastDelay(this Obj_AI_Hero hero)
        {
            return (int)(hero.AttackCastDelay * 1000);
        }

        /// <summary>
        /// Determines whether [is auto attack ready] for [the specified hero].
        /// </summary>
        /// <param name="lastAttack">The last attack.</param>
        /// <param name="hero">The hero.</param>
        /// <returns>True if auto attack is ready.</returns>
        public static bool IsAutoAttackReady(this Obj_AI_Hero hero, int lastAttack)
        {
            return GetGameTickCount() >= lastAttack + GetHeroAutoAttackDelay(hero);
        }

        /// <summary>
        /// Gets the time until auto attack ready.
        /// </summary>
        /// <param name="hero">The hero.</param>
        /// <param name="lastAttack">The last attack.</param>
        /// <returns>The time until auto attack is ready.</returns>
        public static int TimeUntilAutoAttackReady(this Obj_AI_Hero hero, int lastAttack)
        {
            if (lastAttack == 0)
            {
                return GetHeroAutoAttackDelay(hero);
            }

            return lastAttack + GetHeroAutoAttackDelay(hero) - GetGameTickCount();
        }

        /// <summary>
        /// Determines whether [is move ready] for [the specified hero].
        /// </summary>
        /// <param name="lastAttack">The last attack.</param>
        /// <param name="hero">The hero.</param>
        /// <returns>True if move is ready.</returns>
        public static bool IsMoveReady(this Obj_AI_Hero hero, int lastAttack)
        {
            return GetGameTickCount() >= lastAttack + GetHeroAttackCastDelay(hero);
        }

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
        /// Gets the real auto attack range.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static float GetRealAutoAttackRange(this Obj_AI_Hero source, AttackableUnit target)
        {
            var result = source.AttackRange + source.BoundingRadius;
            if (target.IsValidTarget())
            {
                return result + target.BoundingRadius;
            }
            return result;
        }

        /// <summary>
        /// Checks if target is in auto attack range
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="unit">The target unit.</param>
        /// <returns></returns>
        public static bool IsInAutoAttackRange(this Obj_AI_Hero source, AttackableUnit unit)
        {
            if (!unit.IsValidTarget())
            {
                return false;
            }

            var range = source.GetRealAutoAttackRange(unit);
            var rangeSqr = range * range;

            var target = unit as Obj_AI_Base;
            if (target != null)
            {
                return Vector2.DistanceSquared(target.ServerPosition.To2D(), source.ServerPosition.To2D()) <= rangeSqr;
            }

            return Vector2.DistanceSquared(unit.Position.To2D(), source.ServerPosition.To2D()) <= rangeSqr;
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
        /// Gets the predicted health using missile markers and our projectile delay.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="orbwalker">The orbwalker.</param>
        /// <param name="delay">The projectile delay.</param>
        /// <returns></returns>
        public static double GetPredictedHealth(this Obj_AI_Base source, Orbwalker orbwalker, int delay)
        {
            var gameTick = GetGameTickCount();
            var health = (double)source.Health;
            foreach (var marker in orbwalker.MissileMarkers.Where(m => m.Target.NetworkId == source.NetworkId).OrderBy(m => m.CollisionTime))
            {
                var collisionTime = marker.CollisionTime;
                if (gameTick >= collisionTime - delay || collisionTime >= gameTick + delay)
                {
                    break;
                }

                health -= marker.Damage;

                if (health <= 0)
                {
                    return health;
                }
            }

            return health;
        }
    }
}
