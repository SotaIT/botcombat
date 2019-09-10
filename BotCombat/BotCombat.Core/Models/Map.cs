using System.Collections.Generic;

namespace BotCombat.Core.Models
{
    public class Map
    {
        public Map(int id,
            int width,
            int height,
            int scale,
            int initialPower,
            int strengthWeight,
            int staminaWeight,
            List<Wall> walls,
            List<Bonus> bonuses)
        {
            Id = id;
            Width = width;
            Height = height;
            Scale = scale;
            InitialPower = initialPower;
            StrengthWeight = strengthWeight;
            StaminaWeight = staminaWeight;
            Walls = walls;
            Bonuses = bonuses;
        }

        public int Id { get; }

        public int Width { get; }

        public int Height { get; }

        public int Scale { get; }

        public int InitialPower { get; }

        public int StrengthWeight { get; }

        public int StaminaWeight { get; }

        public List<Wall> Walls { get; }

        public List<Bonus> Bonuses { get; }
    }
}