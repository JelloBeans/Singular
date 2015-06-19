namespace Singular.Core.Orbwalker
{
    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// Represents a missile client for use in the orb walker.
    /// </summary>
    public class MissileMarker
    {
        /// <summary>
        /// Gets or sets the missile client.
        /// </summary>
        /// <value>
        /// The missile client.
        /// </value>
        public MissileClient MissileClient { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public Obj_AI_Base Source { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public Obj_AI_Base Target { get; set; }

        /// <summary>
        /// Gets or sets the collision time.
        /// </summary>
        /// <value>
        /// The collision time.
        /// </value>
        public double CollisionTime { get; set; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        /// <value>
        /// The damage.
        /// </value>
        public double Damage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissileMarker"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public MissileMarker(MissileClient client)
        {
            this.MissileClient = client;

            this.Target = (Obj_AI_Base)client.Target;

            var startPosition = client.StartPosition;
            var targetPosition = this.Target.Position;
            var distance = startPosition.Distance(targetPosition);
            this.CollisionTime = Game.Time + ((distance / client.SData.MissileSpeed) * 1000);

            this.Damage = client.SpellCaster.GetAutoAttackDamage(this.Target);
        }
    }
}