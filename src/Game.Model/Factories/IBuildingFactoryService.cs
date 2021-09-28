using Game.Model.Buildings.ResourceProducing;

namespace Game.Model.Factories
{
    public interface IBuildingFactoryService
    {
        CopperMine CreateCopperMine(int level);
        Lumberyard CreateLumberyard(int level);
        Farm CreateFarm(int level);
        Quarry CreateQuarry(int level);
    }
}