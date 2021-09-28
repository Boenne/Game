using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Factories
{
    public interface IWorkerFactoryService
    {
        Farmer CreateFarmer(int level);
        Miner CreateMiner(int level);
        Lumberjack CreateLumberjack(int level);
    }
}