namespace Game.Model.Workers.ResourceProducing
{
    public class ResourceProducingWorker : Worker
    {
        public int Output { get; }

        public ResourceProducingWorker(int level, int output) : base(level)
        {
            Output = output;
        }
    }
}