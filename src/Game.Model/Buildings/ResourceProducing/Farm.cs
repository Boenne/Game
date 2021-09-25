using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.ResourceProducing
{
    public class Farm : ResourceProducingBuilding<Farmer, Food>
    {
        public Farm(int level, int availableResources) : base(level, availableResources)
        {
        }
    }
}