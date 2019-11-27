using System.Collections.Generic;
using BotCombat.BotWorld;
using BotCombat.BuiltInBots;
using BotCombat.Core;
using BotCombat.JsBots;
using NUnit.Framework;

namespace BotCombat.Test
{
    public class GameManagerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Play()
        {
            var mapSettings = new MapSettings(1, 3, 3, 10, 1, 1, 1,
                int.MaxValue, 5, 1,
                new List<Wall> { new Wall(1, 1, 1) },
                new List<Bonus> { new Bonus(1, 0, 0, 1) },
                new List<Trap> { new Trap(1, 2, 2, 1) }, 
                new List<Coordinates>());

            var gameManager = new GameManager(mapSettings, new List<IBot> { new RandomBot(1), new JsBot(2) });
            var game = gameManager.Play();

            Assert.Pass();
        }
    }
}