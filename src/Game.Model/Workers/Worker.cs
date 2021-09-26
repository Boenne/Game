namespace Game.Model.Workers
{
    public abstract class Worker : Identifiable
    {
        protected Worker(int level)
        {
            Level = level;
        }

        public int Level { get; }
    }
}