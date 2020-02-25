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
            int actionTimeout,
            int memoryLimit,
            IEnumerable<Wall> walls,
            IEnumerable<Bonus> bonuses,
            IEnumerable<Trap> traps,
            IEnumerable<StartPoint> startPoints)
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
            ActionTimeout = actionTimeout;
            MemoryLimit = memoryLimit;
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
        public int ActionTimeout { get; }
        public int MemoryLimit { get; }
        public IEnumerable<Wall> Walls { get; }
        public IEnumerable<Bonus> Bonuses { get; }
        public IEnumerable<Trap> Traps { get; }
        public IEnumerable<StartPoint> StartPoints { get; }
    }
}