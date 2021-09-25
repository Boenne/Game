using Game.Model.Workers;

namespace Game.Model.Buildings.ResourceConsuming
{
    public abstract class ResourceConsumingBuilding<T> : Building<T> where T : Worker
    {
    }
}