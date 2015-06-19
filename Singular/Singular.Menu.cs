namespace Singular
{
    using LeagueSharp.Common;

    /// <summary>
    /// Handles the menu functionality
    /// </summary>
    public partial class Singular
    {
        /// <summary>
        /// The main menu
        /// </summary>
        private static Menu mainMenu;

        /// <summary>
        /// Creates the menu.
        /// </summary>
        protected static void CreateMenu()
        {
            mainMenu = new Menu("Singular", "singular", true);
        }
    }
}
