using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.ResourceProducing
{
    public class Lumberyard : ResourceProducingBuilding<Lumberjack, Lumber>
    {
        public Lumberyard(int level, int availableResources) : base(level, availableResources)
        {
        }
    }
}