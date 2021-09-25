using Game.Model.Resources;

namespace Game.Model.Buildings.Settings
{
    public static class StartingResources
    {
        public static Stone Stone = new Stone(100);
        public static Lumber Lumber = new Lumber(100);
        public static Food Food = new Food(100);
        public static bool Apply = true;
    }
}