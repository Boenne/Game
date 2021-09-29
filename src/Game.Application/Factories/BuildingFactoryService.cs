using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Specifications;
using Game.Model.Factories;

namespace Game.ApplicationService.Factories
{
    public class BuildingFactoryService : IBuildingFactoryService
    {
        public CopperMine CreateCopperMine(int level)
        {
            var specifications = GetSpecifications(level);
            return new CopperMine(level, specifications.AvailableResources);
        }

        public Lumberyard CreateLumberyard(int level)
        {
            var specifications = GetSpecifications(level);
            return new Lumberyard(level, specifications.AvailableResources);
        }

        public Farm CreateFarm(int level)
        {
            var specifications = GetSpecifications(level);
            return new Farm(level, specifications.AvailableResources);
        }

        public Quarry CreateQuarry(int level)
        {
            var specifications = GetSpecifications(level);
            return new Quarry(level, specifications.AvailableResources);
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