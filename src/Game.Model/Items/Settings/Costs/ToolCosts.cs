using Game.Model.Resources;

namespace Game.Model.Items.Settings.Costs
{
    public static class ToolCosts
    {
        public sealed class Hammer
        {
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Lumber), 10},
                {typeof(Stone), 10}
            };

            public static ResourceList Level2 = new ResourceList
            {
                {typeof(Lumber), 30},
                {typeof(Copper), 50}
            };

            public static ResourceList GetCosts(int level)
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
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Lumber), 10},
                {typeof(Stone), 10}
            };

            public static ResourceList Level2 = new ResourceList
            {
                {typeof(Lumber), 30},
                {typeof(Copper), 50}
            };

            public static ResourceList GetCosts(int level)
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
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Lumber), 10},
                {typeof(Stone), 10}
            };

            public static ResourceList Level2 = new ResourceList
            {
                {typeof(Lumber), 30},
                {typeof(Copper), 50}
            };

            public static ResourceList GetCosts(int level)
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
            public static ResourceList Level1 = new ResourceList
            {
                {typeof(Lumber), 20},
            };

            public static ResourceList Level2 = new ResourceList
            {
                {typeof(Lumber), 30},
                {typeof(Copper), 50}
            };

            public static ResourceList GetCosts(int level)
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