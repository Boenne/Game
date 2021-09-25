using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.ResourceProducing
{
    public class Quarry : ResourceProducingBuilding<Miner, Stone>
    {
        public Quarry(int level, int availableResources) : base(level, availableResources)
        {
        }
    }
}