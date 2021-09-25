using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.ResourceProducing
{
    public class CopperMine : ResourceProducingBuilding<Miner, Copper>
    {
        public CopperMine(int level, int availableResources) : base(level, availableResources)
        {
        }
    }
}