namespace BotCombat.BotWorld
{
    /// <summary>
    /// Base model for all objects on the Map
    /// </summary>
    public abstract class MapObject: IMapObject
    {
        protected MapObject(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// The Id of the object
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The X coordinate of the object
        /// </summary>
        public int X { get; }

        /// <summary>
        /// The Y coordinate of the object
        /// </summary>
        public int Y { get; }
    }
}