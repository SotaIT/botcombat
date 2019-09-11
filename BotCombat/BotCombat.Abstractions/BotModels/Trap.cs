namespace BotCombat.Abstractions.BotModels
{
    public class Trap : MapObject
    {
        public int Damage { get; }

        public Trap(int x, int y, int damage) : base(x,y)
        {
            Damage = damage;
        }
      
    }
}