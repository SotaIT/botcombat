function Bot() {
    this.initPower = function (power, game, result) {
        distributePower(power, game, result);
    };

    this.distributePower = function (power, game, result) {
        result.Strength = power / 2;
        result.Stamina = power - result.Strength;
    };

    this.chooseDirection = function (game, result) {
        result.Direction = Math.floor(Math.random() * 5);
    };
}