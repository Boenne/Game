namespace Game.Model.Maps
{
    public class Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            var coordinates = obj as Coordinates;
            if (coordinates == null) return false;
            return X == coordinates.X && Y == coordinates.Y;
        }
    }
}