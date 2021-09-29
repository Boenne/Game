using Game.Model.Buildings.ResourceProducing;

namespace Game.Model.Factories
{
    public interface IBuildingFactoryService
    {
        CopperMine CreateCopperMine(int level, int availableResources);
        Lumberyard CreateLumberyard(int level, int availableResources);
        Farm CreateFarm(int level, int availableResources);
        Quarry CreateQuarry(int level, int availableResources);
    }
}