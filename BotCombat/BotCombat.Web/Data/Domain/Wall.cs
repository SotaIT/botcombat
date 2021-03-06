﻿namespace BotCombat.Web.Data.Domain
{
    public class Wall: IEntity, IMapObject, IHasImage, ICoordinates
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int ImageId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
