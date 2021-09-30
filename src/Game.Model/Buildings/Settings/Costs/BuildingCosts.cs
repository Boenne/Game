using Game.Model.Resources;

namespace Game.Model.Buildings.Settings.Costs
{
    public static class BuildingCosts
    {
        public sealed class CopperMine
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Stone), 100},
                {typeof(Lumber), 100}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Farm
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Stone), 50},
                {typeof(Lumber), 50}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Lumberyard
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Stone), 50}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Quarry
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Lumber), 50}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }
    }
}