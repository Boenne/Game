using Game.Model.Factories;
using Game.Model.Workers.ResourceProducing;
using Game.Model.Workers.Settings.Specifications;

namespace Game.Application.Factories
{
    public class WorkerFactoryService : IWorkerFactoryService
    {
        public Farmer CreateFarmer(int level)
        {
            var specifications = GetSpecifications(level);
            return new Farmer(level, specifications.OutputPerWorker);
        }

        public Miner CreateMiner(int level)
        {
            var specifications = GetSpecifications(level);
            return new Miner(level, specifications.OutputPerWorker);
        }

        public Lumberjack CreateLumberjack(int level)
        {
            var specifications = GetSpecifications(level);
            return new Lumberjack(level, specifications.OutputPerWorker);
        }

        private static ResourceProducingWorkerSpecification GetSpecifications(int level)
        {
            switch (level)
            {
                default:
                    return ResourceProducingWorkerSpecifications.Level1;
            }
        }
    }
}