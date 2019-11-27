namespace BotCombat.BotWorld
{
    /// <summary>
    /// An object that can be captured by a bot to get/lose Power
    /// Will disapear after that
    /// </summary>
    public class Bonus : MapObject
    {
        /// <summary>
        /// The amount of power a bot will get (or lose of the value is less than zero)
        /// </summary>
        public int Power { get; }

        public Bonus(int id, int x, int y, int power) : base(id, x, y)
        {
            Power = power;
        }
    }
}