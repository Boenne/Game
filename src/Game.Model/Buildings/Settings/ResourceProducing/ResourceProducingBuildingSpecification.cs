namespace Game.Model.Buildings.Settings.ResourceProducing
{
    public class ResourceProducingBuildingSpecification
    {
        public ResourceProducingBuildingSpecification(int outputPerWorker, int availableResources)
        {
            OutputPerWorker = outputPerWorker;
            AvailableResources = availableResources;
        }

        public int OutputPerWorker { get; }
        public int AvailableResources { get; }
    }
}