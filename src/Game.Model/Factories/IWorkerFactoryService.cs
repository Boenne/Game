using Game.Model.Workers.ResourceConsuming;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Factories
{
    public interface IWorkerFactoryService
    {
        Farmer CreateFarmer(int level);
        Miner CreateMiner(int level);
        Lumberjack CreateLumberjack(int level);
        Blacksmith CreateBlacksmith(int level);
    }
}