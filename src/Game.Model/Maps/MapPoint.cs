namespace Game.Model.Maps
{
    public class MapPoint
    {
        public MapPoint(Identifiable objectOnMap)
        {
            Object = objectOnMap;
        }

        public Identifiable Object { get; }
    }
}