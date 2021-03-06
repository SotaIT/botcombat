﻿using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.Engine.Contracts;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;
using GameStates = BotCombat.Web.Data.Domain.GameStates;

namespace BotCombat.Web.Services
{
    public class GameDataService : BaseDataService<Game>
    {
        public GameDataService(ApplicationDbContext db) : base(db)
        {
        }

        public Game AddGame(int mapId, List<int> botIds)
        {
            var game = new Game
            {
                MapId = mapId,
                Created = DateTime.UtcNow,
                State = (int)GameStates.Created
            };

            Db.Games.Add(game);
            Db.SaveChanges();

            foreach (var botId in botIds)
            {
                Db.GameBots.Add(new GameBot
                {
                    BotId = botId,
                    GameId = game.Id
                });
            }
            Db.SaveChanges();

            return game;
        }

        public GameBot AddBot(int gameId, int botId)
        {
            var gameBot = new GameBot
            {
                BotId = botId,
                GameId = gameId
            };
            Db.GameBots.Add(gameBot);
            Db.SaveChanges();
            return gameBot;
        }

        public void UpdateGame(Game game, List<GameBot> gameBots = null)
        {
            Db.Games.Update(game);
            if (gameBots != null)
                Db.GameBots.UpdateRange(gameBots);
            Db.SaveChanges();
        }

        public List<GameBot> GetBots(int gameId)
        {
            return Db.GameBots.Where(b => b.GameId == gameId).ToList();
        }

        public List<Game> GetGames(GameStates state)
        {
            return GetQuery().Where(g => g.State == (int) state).ToList();
        }
    }
}