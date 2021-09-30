using Game.Model.Resources;

namespace Game.Model.Workers.Settings.Costs
{
    public static class WorkerCosts
    {
        public sealed class Farmer
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Lumberjack
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Miner
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };

            public static ResourceList GetCosts(int level)
            {
                switch (level)
                {
                    default: return Level1;
                }
            }
        }

        public sealed class Blacksmith
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
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