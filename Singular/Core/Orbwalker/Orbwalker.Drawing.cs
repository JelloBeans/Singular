namespace Singular.Core.Orbwalker
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using SingularLibrary;

    using Color = System.Drawing.Color;

    /// <summary>
    /// Handles the drawing for the orbwalker
    /// </summary>
    public partial class Orbwalker
    {
        /// <summary>
        /// Handles the drawing event
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void Orbwalker_Drawing_OnDraw(System.EventArgs args)
        {
            var minions = GameObjects.EnemyMinions.ToList();
            //foreach (var minion in minions.Where(m => m.IsHPBarRendered && m.IsValidTarget()))
            //{
            //    var attackDamage = Me.GetAutoAttackDamage(minion, true);

            //    var healthBar = minion.HPBarPosition;
            //    var maxHealth = minion.MaxHealth;
            //    var currentHealth = minion.Health;

            //    if (currentHealth <= attackDamage)
            //    {
            //        continue;
            //    }

            //    var barWidth = healthBar.Length();

            //    var split = (float)(barWidth / (maxHealth / attackDamage));
            //    var offset = barWidth - split;
            //    while (offset > 0)
            //    {
            //        var startVector = new Vector2(healthBar.X + 45 + offset, healthBar.Y + 18);
            //        var endVector = new Vector2(healthBar.X + 45 + offset, healthBar.Y + 23);
            //        Drawing.DrawLine(startVector, endVector, 1f, Color.White);
            //        offset -= split;
            //    }
            //}
        }
    }
}
