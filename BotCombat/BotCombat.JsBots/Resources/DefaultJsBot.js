function iBot() {
    this.distributePower = function (power, game, result) {
        result.Ranged = power / 3;
        result.Strength = (power - result.Ranged) / 2;
        result.Stamina = power - result.Strength - result.Ranged;
    };

    this.chooseAction = function (game, result) {
        result.BotAction = Math.floor(Math.random() * 9) + 1;
    };
}