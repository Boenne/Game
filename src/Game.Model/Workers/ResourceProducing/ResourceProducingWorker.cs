namespace Game.Model.Workers.ResourceProducing
{
    public class ResourceProducingWorker : Worker
    {
        public int Output { get; }

        public ResourceProducingWorker(int foodCost, int output) : base(foodCost)
        {
            Output = output;
        }
    }
}