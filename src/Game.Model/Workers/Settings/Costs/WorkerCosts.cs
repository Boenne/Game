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
        }

        public sealed class Lumberjack
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };
        }

        public sealed class Miner
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };
        }

        public sealed class Blacksmith
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Food), 20}
            };
        }
    }
}