namespace Game.Model.Workers
{
    public abstract class Worker
    {
        public int FoodCost { get; }

        protected Worker(int foodCost)
        {
            FoodCost = foodCost;
        }
    }
}