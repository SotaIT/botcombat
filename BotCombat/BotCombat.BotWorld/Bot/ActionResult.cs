namespace BotCombat.BotWorld
{
    public class ActionResult
    {
        public BotAction BotAction { get; set; } = BotAction.Stop;

        public void Stop()
        {
            BotAction = BotAction.Stop;
        }

        public void Shoot()
        {
            BotAction = BotAction.Shoot;
        }

        public void MoveUp()
        {
            BotAction = BotAction.MoveUp;
        }

        public void MoveRight()
        {
            BotAction = BotAction.MoveRight;
        }

        public void MoveDown()
        {
            BotAction = BotAction.MoveDown;
        }

        public void MoveLeft()
        {
            BotAction = BotAction.MoveLeft;
        }
    }
}