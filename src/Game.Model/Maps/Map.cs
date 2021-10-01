using System;

namespace Game.Model.Maps
{
    public class Map
    {
        private static readonly object Lock = new object();
        private readonly Urn[,] _map;
        private readonly int _mapSize;

        public Map(int size)
        {
            _mapSize = size;
            _map = new Urn[_mapSize, _mapSize];
        }

        public Coordinates GetPosition(Urn id)
        {
            lock (Lock)
            {
                for (var x = 0; x < _mapSize; x++)
                for (var y = 0; y < _mapSize; y++)
                {
                    var mapPoint = _map[x, y];
                    if (mapPoint != null && Equals(mapPoint, id)) return new Coordinates(x, y);
                }

                return null;
            }
        }

        public bool IsLocationAvailable(Coordinates coordinates)
        {
            lock (Lock)
            {
                return _map[coordinates.X, coordinates.Y] == null;
            }
        }

        public bool SetLocation(Coordinates coordinates, Identifiable objectOnMap)
        {
            if (!IsLocationAvailable(coordinates)) return false;
            lock (Lock)
            {
                _map[coordinates.X, coordinates.Y] = objectOnMap.Id;
                return true;
            }
        }

        public bool ReplaceLocation(Coordinates coordinates, Identifiable objectOnMap)
        {
            lock (Lock)
            {
                _map[coordinates.X, coordinates.Y] = objectOnMap.Id;
                return true;
            }
        }

        public bool RemoveLocation(Coordinates coordinates)
        {
            if (IsLocationAvailable(coordinates)) return false;
            lock (Lock)
            {
                _map[coordinates.X, coordinates.Y] = null;
                return true;
            }
        }

        public double GetDistance(Coordinates start, Coordinates destination)
        {
            var dx = Math.Abs(start.X - destination.X);
            var dy = Math.Abs(start.Y - destination.Y);

            var min = Math.Min(dx, dy);
            var max = Math.Max(dx, dy);

            var diagonalSteps = min;
            var straightSteps = max - min;

            return Math.Sqrt(2) * diagonalSteps + straightSteps;
        }
    }
}