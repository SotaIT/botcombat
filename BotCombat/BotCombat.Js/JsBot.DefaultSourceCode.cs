namespace BotCombat.Js
{
    public partial class JsBot
    {
        public const string DefaultSourceCode =
@"
function iBot() {
    this.distributePower = function (power, game, result) {
        result.Strength = power / 2;
        result.Stamina = power - result.Strength;
    };

    this.chooseDirection = function (game, result) {
        result.Direction = Math.floor(Math.random() * 5);
    };
}
";
    }
}