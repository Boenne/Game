﻿using System.Threading.Tasks;
using Game.Model;
using Game.Model.Buildings.Settlement;
using Game.Model.Factories;

namespace Game.Application
{
    public class Engine
    {
        private readonly IBuildingFactoryService _buildingFactoryService;
        private readonly IWorkerFactoryService _workerFactoryService;
        private bool _produceResources = true;

        public Engine(IBuildingFactoryService buildingFactoryService, IWorkerFactoryService workerFactoryService)
        {
            _buildingFactoryService = buildingFactoryService;
            _workerFactoryService = workerFactoryService;
        }

        public void StartResourceProduction(Settlement settlement)
        {
            Task.Run(async () =>
            {
                while (_produceResources)
                {
                    await Task.Delay(ExecutionTimes.ResourceProductionTime);
                    if (!_produceResources) break;
                    settlement.ProduceResources();
                }
            });
        }

        public void StopResourceProduction()
        {
            _produceResources = false;
        }
    }
}