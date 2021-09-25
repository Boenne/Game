using System.Collections.Generic;
using Game.Model.Resources;

namespace Game.Model.Buildings.Settings.Costs
{
    public static class BuildingCosts
    {
        public sealed class CopperMine
        {
            public static List<Resource> Level1 = new List<Resource>
            {
                new Stone {Quantity = 100},
                new Lumber {Quantity = 100}
            };
        }

        public sealed class Farm
        {
            public static List<Resource> Level1 = new List<Resource>
            {
                new Stone {Quantity = 50},
                new Lumber {Quantity = 50}
            };
        }

        public sealed class Lumberyard
        {
            public static List<Resource> Level1 = new List<Resource>
            {
                new Stone {Quantity = 50},
            };
        }

        public sealed class Quarry
        {
            public static List<Resource> Level1 = new List<Resource>
            {
                new Lumber {Quantity = 50},
            };
        }
    }
}