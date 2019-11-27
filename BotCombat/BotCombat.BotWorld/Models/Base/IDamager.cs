namespace BotCombat.BotWorld
{
    public interface IDamager : IMapObject
    {
        int Damage { get; }
    }
}