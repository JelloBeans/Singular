namespace Singular.Core.Composite
{
    using System;

    using LeagueSharp;

    /// <summary>
    /// Handles the execution of the tree composites
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Executor"/> class.
        /// </summary>
        public Executor()
        {
            Game.OnUpdate += this.Game_OnUpdate;
        }

        /// <summary>
        /// The on update event of <see cref="Game"/>.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Game_OnUpdate(EventArgs args)
        {
        }
    }
}
