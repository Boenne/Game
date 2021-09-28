using System.Collections.Generic;
using Game.Model.Resources;

namespace Game.Model.Items.Settings.Costs
{
    public static class ToolCosts
    {
        public sealed class Hammer
        {
            public static List<Resource> Level1 = new List<Resource> {new Lumber(10), new Stone(10)};
            public static List<Resource> Level2 = new List<Resource> {new Copper(50), new Lumber(30)};

            public static List<Resource> GetCosts(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default: return Level1;
                }
            }
        }
        public sealed class Pickaxe
        {
            public static List<Resource> Level1 = new List<Resource> {new Lumber(10), new Stone(10)};
            public static List<Resource> Level2 = new List<Resource> {new Copper(50), new Lumber(30)};

            public static List<Resource> GetCosts(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default: return Level1;
                }
            }
        }

        public sealed class Hatchet
        {
            public static List<Resource> Level1 = new List<Resource> {new Lumber(10), new Stone(10)};
            public static List<Resource> Level2 = new List<Resource> {new Copper(50), new Lumber(30)};

            public static List<Resource> GetCosts(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default: return Level1;
                }
            }
        }

        public sealed class Rake
        {
            public static List<Resource> Level1 = new List<Resource> {new Lumber(20)};
            public static List<Resource> Level2 = new List<Resource> {new Copper(50), new Lumber(30)};

            public static List<Resource> GetCosts(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default: return Level1;
                }
            }
        }
    }
}