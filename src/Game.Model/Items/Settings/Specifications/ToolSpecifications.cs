namespace Game.Model.Items.Settings.Specifications
{
    public class ToolSpecifications
    {
        public sealed class Hammer
        {
            public static ToolSpecification Level1 = new ToolSpecification("Stone Hammer", 1);
            public static ToolSpecification Level2 = new ToolSpecification("Copper Hammer", 3);

            public static ToolSpecification GetSpecification(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default:
                        return Level1;
                }
            }
        }
        public sealed class Pickaxe
        {
            public static ToolSpecification Level1 = new ToolSpecification("Stone Pickaxe", 1);
            public static ToolSpecification Level2 = new ToolSpecification("Copper Pickaxe", 3);

            public static ToolSpecification GetSpecification(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default:
                        return Level1;
                }
            }
        }

        public sealed class Hatchet
        {
            public static ToolSpecification Level1 = new ToolSpecification("Stone Hatchet", 1);
            public static ToolSpecification Level2 = new ToolSpecification("Copper Hatchet", 3);

            public static ToolSpecification GetSpecification(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default:
                        return Level1;
                }
            }
        }

        public sealed class Rake
        {
            public static ToolSpecification Level1 = new ToolSpecification("Wooden Rake", 1);
            public static ToolSpecification Level2 = new ToolSpecification("Copper Rake", 3);

            public static ToolSpecification GetSpecification(int level)
            {
                switch (level)
                {
                    case 2:
                        return Level2;
                    default:
                        return Level1;
                }
            }
        }
    }

    public class ToolSpecification
    {
        public ToolSpecification(string name, int modifier)
        {
            Name = name;
            Modifier = modifier;
        }

        public int Modifier { get; }
        public string Name { get; }
    }
}