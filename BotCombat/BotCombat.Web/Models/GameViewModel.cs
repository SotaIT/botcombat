using System.Collections.Generic;
using BotCombat.BotWorld;

namespace BotCombat.Web.Models
{
    public class GameViewModel
    {

        public GameViewModel(Game game,
            int scale,
            int backgroundImageId,
            int bulletSpeed,
            Dictionary<int, int> wallImages,
            Dictionary<int, int> bonusImages,
            Dictionary<int, int> trapImages,
            Dictionary<int, int> botImages,
            Dictionary<int, int> bulletImages,
            Dictionary<int, int> shotImages,
            Dictionary<int, int> explosionImages,
            Dictionary<int, string> images, 
            Dictionary<int, string> botNames)
        {
            Game = game;
            Scale = scale;
            BackgroundImageId = backgroundImageId;
            BulletSpeed = bulletSpeed;
            WallImages = wallImages;
            BonusImages = bonusImages;
            TrapImages = trapImages;
            BotImages = botImages;
            BulletImages = bulletImages;
            ShotImages = shotImages;
            ExplosionImages = explosionImages;
            Images = images;
            BotNames = botNames;
        }

        public Game Game { get; }
        public int Scale { get; }
        public int BackgroundImageId { get; }
        public int BulletSpeed { get; }
        public Dictionary<int, int> WallImages { get; }
        public Dictionary<int, int> BonusImages { get; }
        public Dictionary<int, int> TrapImages { get; }
        public Dictionary<int, int> BotImages { get; }
        public Dictionary<int, int> BulletImages { get; }
        public Dictionary<int, int> ShotImages { get; }
        public Dictionary<int, int> ExplosionImages { get; }
        public Dictionary<int, string> Images { get; }
        public Dictionary<int, string> BotNames { get; }
    }
}