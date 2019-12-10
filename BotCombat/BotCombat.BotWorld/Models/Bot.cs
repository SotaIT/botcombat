namespace BotCombat.BotWorld
{
    /// <summary>
    /// A bot is moving on the map, trying to get bonuses, escape traps and kill other bots
    /// </summary>
    public class Bot : MapObject, IDamager, IDirection
    {
        public Bot(int id, int x, int y, int health, int damage, string error, int direction, bool isDamaged, bool isStunned) : base(id, x, y)
        {
            Health = health;
            Damage = damage;
            Error = error;
            Direction = direction;
            IsDamaged = isDamaged;
            IsStunned = isStunned;
        }

        /// <summary>
        /// The health of the bot
        /// </summary>
        public int Health { get; }

        /// <summary>
        /// The amount of damage the bot does
        /// </summary>
        public int Damage { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Bot direction
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// Is damaged during current step
        /// </summary>
        public bool IsDamaged { get; }

        /// <summary>
        /// Is stunned during current step
        /// </summary>
        public bool IsStunned { get; }
    }
}