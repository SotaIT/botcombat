﻿using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.BotWorld;

namespace BotCombat.Core
{
    /// <summary>
    /// The game map manager
    /// </summary>
    internal class MapManager
    {
        private readonly MapSettings _settings;

        private readonly List<IMapObject>[,] _points;
        private readonly List<IMapObject> _objects = new List<IMapObject>();

        internal MapManager(MapSettings mapSettings)
        {
            _settings = mapSettings;

            _points = new List<IMapObject>[_settings.Width, _settings.Height];

            for (var x = 0; x < _settings.Width; x++)
                for (var y = 0; y < _settings.Height; y++)
                    _points[x, y] = new List<IMapObject>();

            // Add walls to the map
            AddRange(_settings.Walls);
            // Add traps to the map
            AddRange(_settings.Traps);
            // Add bonuses to the map
            AddRange(_settings.Bonuses);
        }

        /// <summary>
        /// Checks if there is no IMapObject at the point
        /// </summary>
        internal bool IsEmpty(int x, int y)
        {
            return _points[x, y].Count == 0;
        }

        /// <summary>
        /// Checks if there are more than 1 IMapObjects at the point
        /// </summary>
        internal bool IsCollision(int x, int y)
        {
            return _points[x, y].Count > 1;
        }

        /// <summary>
        /// Adds the IMapObject to the map
        /// </summary>
        internal void Add(IMapObject mapObject, bool checkIsEmpty = false)
        {
            if (checkIsEmpty && !IsEmpty(mapObject.X, mapObject.Y))
                throw new Exception("The point is not empty!");

            _points[mapObject.X, mapObject.Y].Add(mapObject);
            _objects.Add(mapObject);
        }

        /// <summary>
        /// Adds MapObjects to the map
        /// </summary>
        internal void AddRange<T>(IEnumerable<T> mapObjects, bool checkIsEmpty = true) where T : IMapObject
        {
            foreach (var mapObject in mapObjects)
                Add(mapObject, checkIsEmpty);
        }

        /// <summary>
        /// Removes the IMapObject from the map
        /// </summary>
        internal void Remove(IMapObject mapObject)
        {
            _points[mapObject.X, mapObject.Y].Remove(mapObject);
            _objects.Remove(mapObject);
        }

        /// <summary>
        /// Removes all objects of specified type
        /// </summary>
        internal void RemoveAll<T>() where T: IMapObject
        {
            var objects = GetObjects<T>().ToList();
            foreach (var o in objects)
                Remove(o);
        }

        internal void RemoveShotsAndExplosions()
        {
            RemoveAll<Explosion>();
            RemoveAll<Shot>();
        }

        /// <summary>
        /// Gets an IMapObject of the specified type from the point
        /// </summary>
        internal T GetObject<T>(int x, int y) where T : class
        {
            return _points[x, y].FirstOrDefault(i => i is T) as T;
        }

        /// <summary>
        /// Gets all IMapObjects of the specified type from the map
        /// </summary>
        internal IEnumerable<T> GetObjects<T>()
        {
            return _objects.OfType<T>();
        }

        internal IEnumerable<BotManager> Bots => GetObjects<BotManager>();
        internal IEnumerable<Bonus> Bonuses => GetObjects<Bonus>();
        internal IEnumerable<BulletManager> Bullets => GetObjects<BulletManager>();
        internal IEnumerable<Shot> Shots => GetObjects<Shot>();
        internal IEnumerable<Explosion> Explosions => GetObjects<Explosion>();

        /// <summary>
        /// Gets all IMapObjects of the specified type from the point
        /// </summary>
        internal List<T> GetObjects<T>(int x, int y)
        {
            return _points[x, y].OfType<T>().ToList();
        }

        /// <summary>
        /// Gets all empty points from the map
        /// </summary>
        internal List<StartPoint> GetEmptyPoints()
        {
            var emptyPoints = new List<StartPoint>();

            for (var x = 0; x < _settings.Width; x++)
                for (var y = 0; y < _settings.Height; y++)
                    if (IsEmpty(x, y))
                        emptyPoints.Add(new StartPoint(x, y));

            return emptyPoints;
        }
    }
}