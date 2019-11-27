using System.Collections.Generic;
using BotCombat.BotWorld;

namespace BotCombat.Core
{
    public class MapSettings
    {
        public MapSettings(int id,
            int width,
            int height,
            int initialPower,
            int strengthWeight,
            int staminaWeight,
            int rangedWeight,
            int maxStepCount,
            int bonusSpawnInterval,
            int bulletSpeed,
            List<Wall> walls,
            List<Bonus> bonuses,
            List<Trap> traps,
            List<Coordinates> startPoints)
        {
            Id = id;
            Width = width;
            Height = height;
            InitialPower = initialPower;
            StrengthWeight = strengthWeight;
            StaminaWeight = staminaWeight;
            RangedWeight = rangedWeight;
            MaxStepCount = maxStepCount;
            BonusSpawnInterval = bonusSpawnInterval;
            BulletSpeed = bulletSpeed;
            Walls = walls;
            Bonuses = bonuses;
            Traps = traps;
            StartPoints = startPoints;
        }

        public int Id { get; }
        public int Width { get; }
        public int Height { get; }
        public int InitialPower { get; }
        public int StrengthWeight { get; }
        public int StaminaWeight { get; }
        public int RangedWeight { get; }
        public int MaxStepCount { get; }
        public int BonusSpawnInterval { get; }
        public int BulletSpeed { get; }
        public List<Wall> Walls { get; }
        public List<Bonus> Bonuses { get; }
        public List<Trap> Traps { get; }
        public List<Coordinates> StartPoints { get; }
    }
}