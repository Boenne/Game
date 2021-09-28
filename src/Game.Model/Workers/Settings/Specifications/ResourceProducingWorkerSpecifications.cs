namespace Game.Model.Workers.Settings.Specifications
{
    public static class ResourceProducingWorkerSpecifications
    {
        public static ResourceProducingWorkerSpecification Level1 = new ResourceProducingWorkerSpecification(2);
    }

    public class ResourceProducingWorkerSpecification
    {
        public ResourceProducingWorkerSpecification(int outputPerWorker)
        {
            OutputPerWorker = outputPerWorker;
        }

        public int OutputPerWorker { get; }
    }
}