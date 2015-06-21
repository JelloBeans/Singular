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
        /// The hotkeys menu
        /// </summary>
        private static Menu hotkeysMenu;

        /// <summary>
        /// Creates the menu.
        /// </summary>
        protected static void CreateMenu()
        {
            mainMenu = new Menu("Singular", "singular", true);
            mainMenu.AddItem(new MenuItem("Streaming", "Streaming Mode").SetValue(false));
            mainMenu.AddToMainMenu();

            hotkeysMenu = new Menu("Hotkeys", "hotkeys");
            hotkeysMenu.AddItem(new MenuItem("AutoCarry", "Auto Carry").SetShared().SetValue(new KeyBind(' ', KeyBindType.Press)));
            hotkeysMenu.AddItem(new MenuItem("Mixed", "Mixed").SetShared().SetValue(new KeyBind('C', KeyBindType.Press)));
            hotkeysMenu.AddItem(new MenuItem("LaneClear", "Lane Clear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));
            hotkeysMenu.AddItem(new MenuItem("LastHit", "Last Hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));

            mainMenu.AddSubMenu(hotkeysMenu);
        }

        /// <summary>
        /// Gets the hotkeys menu.
        /// </summary>
        /// <returns>The hotkeys menu</returns>
        public static Menu GetHotkeysMenu()
        {
            return hotkeysMenu;
        }
    }
}
