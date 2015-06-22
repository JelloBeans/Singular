namespace Singular
{
    using System;

    using global::Singular.Core.Composite;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Orbwalker = global::Singular.Core.Orbwalker.Orbwalker;

    /// <summary>
    /// The main singular class.
    /// </summary>
    public partial class Singular
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Singular Instance { get; private set; }

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
        /// The current champion
        /// </summary>
        private Core.Enum.Champion champion = Core.Enum.Champion.None;

        /// <summary>
        /// Gets the champion type from the champion name.
        /// </summary>
        /// <value>
        /// The current champion.
        /// </value>
        private Core.Enum.Champion Champion
        {
            get
            {
                if (this.champion == Core.Enum.Champion.None)
                {
                    Enum.TryParse(Me.ChampionName, out this.champion);
                }

                return this.champion;
            }
        }

        /// <summary>
        /// Gets or sets the orbwalker.
        /// </summary>
        /// <value>
        /// The orbwalker.
        /// </value>
        public Orbwalker Orbwalker { get; set; }

        /// <summary>
        /// Gets or sets the executor.
        /// </summary>
        /// <value>
        /// The executor.
        /// </value>
        private Executor Executor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Singular"/> class.
        /// </summary>
        public Singular()
        {
            Instance = this;
            CustomEvents.Game.OnGameLoad += this.Initialize;
        }

        /// <summary>
        /// Creates the champion behaviors and attaches any required events.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Initialize(EventArgs e)
        {
            if (!this.CreateBehaviors())
            {
                // If we have no auto carry behavior we cannot proceed and must stop immediately.
                Notifications.AddNotification(new Notification(string.Format("No AutoCarry composite could be found for champion: {0}", this.Champion), 3000));
                return;
            }

            CreateMenu();

            this.Orbwalker = new Orbwalker();
            this.Executor = new Executor();
            
            this.AttachEvents();

            Notifications.AddNotification(new Notification(string.Format("Loaded Singular for {0}", this.Champion), 3000));
        }

    }
}
