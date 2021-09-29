using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Specifications;
using Game.Model.Factories;

namespace Game.Application.Factories
{
    public class BuildingFactoryService : IBuildingFactoryService
    {
        public CopperMine CreateCopperMine(int level, int availableResources)
        {
            return new CopperMine(level, availableResources);
        }

        public Lumberyard CreateLumberyard(int level, int availableResources)
        {
            return new Lumberyard(level, availableResources);
        }

        public Farm CreateFarm(int level, int availableResources)
        {
            return new Farm(level, availableResources);
        }

        public Quarry CreateQuarry(int level, int availableResources)
        {
            return new Quarry(level, availableResources);
        }

        private static ResourceProducingBuildingSpecification GetSpecifications(int level)
        {
            switch (level)
            {
                default:
                    return ResourceProducingBuildingSpecifications.Level1;
            }
        }
    }
}