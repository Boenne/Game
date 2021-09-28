namespace Game.Model.Items
{
    public abstract class Item : Identifiable
    {
        protected Item(int level)
        {
            Level = level;
        }

        public int Level { get; }
    }
}