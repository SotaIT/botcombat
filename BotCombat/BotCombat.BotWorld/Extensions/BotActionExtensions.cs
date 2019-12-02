namespace BotCombat.BotWorld
{
    public static class BotActionExtensions
    {
        public static Direction ToDirection(this BotAction botAction)
        {
            switch (botAction)
            {
                case BotAction.MoveUp:
                case BotAction.TurnUp:
                    return Direction.Up;

                case BotAction.MoveRight:
                case BotAction.TurnRight:
                    return Direction.Right;

                case BotAction.MoveDown:
                case BotAction.TurnDown:
                    return Direction.Down;

                case BotAction.MoveLeft:
                case BotAction.TurnLeft:
                    return Direction.Left;
            }
            return Direction.None;
        }
    }
}