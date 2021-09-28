using System;

namespace Game.Model.Items.Tools
{
    public abstract class Tool : Item
    {
        protected Tool(string name, int modifier, int level) : base(level)
        {
            Name = name;
            Modifier = modifier;
        }

        public string Name { get; }
        public int Modifier { get; }

        public Tool Copy()
        {
            var instance = Activator.CreateInstance(GetType(), Name, Modifier, Level);
            return (Tool) instance;
        }
    }
}