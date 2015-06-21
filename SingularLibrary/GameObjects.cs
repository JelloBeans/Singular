namespace SingularLibrary
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// A class of game objects
    /// </summary>
    public static class GameObjects
    {
        /// <summary>
        /// The minions
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> Minions = new HashSet<Obj_AI_Minion>();

        /// <summary>
        /// The game objects
        /// </summary>
        private static readonly HashSet<GameObject> GameObjectSet = new HashSet<GameObject>(); 

        /// <summary>
        /// The initialized status flag
        /// </summary>
        private static bool initialized;

        /// <summary>
        /// Initializes static members of the <see cref="GameObjects"/> class. 
        /// </summary>
        static GameObjects()
        {
            if (initialized)
            {
                return;
            }

            Initialize();
        }

        /// <summary>
        /// Gets the enemy minions.
        /// </summary>
        /// <value>
        /// The enemy minions.
        /// </value>
        public static IEnumerable<Obj_AI_Minion> EnemyMinions
        {
            get
            {
                return Minions.Where(m => m.IsEnemy);
            }
        }

        /// <summary>
        /// Gets the ally minions.
        /// </summary>
        /// <value>
        /// The ally minions.
        /// </value>
        public static IEnumerable<Obj_AI_Minion> AllyMinions
        {
            get
            {
                return Minions.Where(m => m.IsAlly);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private static void Initialize()
        {
            initialized = true;
            CustomEvents.Game.OnGameLoad += GameObjects_Game_OnGameLoad;
        }

        /// <summary>
        /// Handles the on game load event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void GameObjects_Game_OnGameLoad(System.EventArgs args)
        {
            GameObjectSet.UnionWith(ObjectManager.Get<GameObject>());

            Minions.UnionWith(ObjectManager.Get<Obj_AI_Minion>().Where(m => m.Team != GameObjectTeam.Neutral && !m.Name.Contains("ward") && !m.Name.Contains("trinket")));

            GameObject.OnCreate += GameObjects_GameObject_OnCreate;
            GameObject.OnDelete += GameObjects_GameObject_OnDelete;
            Game.OnUpdate += GameObjects_Game_OnUpdate;
        }

        /// <summary>
        /// Handles the game update event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void GameObjects_Game_OnUpdate(System.EventArgs args)
        {
            foreach (var minion in Minions.Where(m => !m.IsValid).ToList())
            {
                Minions.Remove(minion);
            }
        }

        /// <summary>
        /// Handles the on create game object event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void GameObjects_GameObject_OnCreate(GameObject sender, System.EventArgs args)
        {
            GameObjectSet.Add(sender);

            var minion = sender as Obj_AI_Minion;
            if (minion != null && minion.Team != GameObjectTeam.Neutral && !minion.Name.Contains("ward") && !minion.Name.Contains("trinket"))
            {
                Minions.Add(minion);
            }
        }

        /// <summary>
        /// Handles the on delete game object event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void GameObjects_GameObject_OnDelete(GameObject sender, System.EventArgs args)
        {
            GameObjectSet.Remove(sender);

            var minion = sender as Obj_AI_Minion;
            if (minion != null && minion.Team != GameObjectTeam.Neutral && !minion.Name.Contains("ward") && !minion.Name.Contains("trinket"))
            {
                Minions.Remove(minion);
            }
        }
    }
}
