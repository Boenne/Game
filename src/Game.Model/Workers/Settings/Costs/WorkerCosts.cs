using System.Collections.Generic;
using Game.Model.Resources;

namespace Game.Model.Workers.Settings.Costs
{
    public static class WorkerCosts
    {
        public sealed class Farmer
        {
            public static List<Resource> Level1 = new List<Resource> {new Food(20)};
        }

        public sealed class Lumberjack
        {
            public static List<Resource> Level1 = new List<Resource> {new Food(20)};
        }

        public sealed class Miner
        {
            public static List<Resource> Level1 = new List<Resource> {new Food(20)};
        }
    }
}