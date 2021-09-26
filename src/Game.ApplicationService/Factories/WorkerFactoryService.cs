﻿using Game.Model.Workers.ResourceProducing;
using Game.Model.Workers.Settings.ResourceProducing;

namespace Game.ApplicationService.Factories
{
    public interface IWorkerFactoryService
    {
        Farmer CreateFarmer(int level);
        Miner CreateMiner(int level);
        Lumberjack CreateLumberjack(int level);
    }

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