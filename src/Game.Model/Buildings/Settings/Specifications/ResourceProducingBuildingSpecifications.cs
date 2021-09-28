namespace Game.Model.Buildings.Settings.Specifications
{
    public static class ResourceProducingBuildingSpecifications
    {
        public static ResourceProducingBuildingSpecification Level1 =
            new ResourceProducingBuildingSpecification(1000);
    }

    public class ResourceProducingBuildingSpecification
    {
        public ResourceProducingBuildingSpecification(int availableResources)
        {
            AvailableResources = availableResources;
        }

        public int AvailableResources { get; }
    }
}