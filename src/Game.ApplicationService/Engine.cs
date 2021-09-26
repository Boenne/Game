using System;
using System.Threading.Tasks;
using Game.ApplicationService.Factories;
using Game.Model.Buildings;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Costs;
using Game.Model.Buildings.Settlement;
using Game.Model.Workers.ResourceProducing;
using Game.Model.Workers.Settings.Costs;

namespace Game.ApplicationService
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

        public async Task AddCopperMine(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.Consume(BuildingCosts.CopperMine.Level1))
                {
                    var copperMine = _buildingFactoryService.CreateCopperMine(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddCopperMine(copperMine);
                }
        }

        public async Task AddFarm(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.Consume(BuildingCosts.Farm.Level1))
                {
                    var farm = _buildingFactoryService.CreateFarm(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddFarm(farm);
                }
        }

        public async Task AddQuarry(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.Consume(BuildingCosts.Quarry.Level1))
                {
                    var quarry = _buildingFactoryService.CreateQuarry(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddQuarry(quarry);
                }
        }

        public async Task AddLumberyard(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.Consume(BuildingCosts.Lumberyard.Level1))
                {
                    var lumberyard = _buildingFactoryService.CreateLumberyard(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddLumberyard(lumberyard);
                }
        }

        public async Task SendCarriersToBuilding(Settlement settlement, IResourceProducingBuilding building,
            params Guid[] carrierIds)
        {
            var carriers = settlement.Storage.GetCarriers(carrierIds);
            if (carriers.Count == 0) return;
            foreach (var carrier in carriers)
            {
                building.LoadCarrier(carrier);
                await Task.Delay(ExecutionTimes.CarrierTravelTime);
                building.CarrierArrivedAtStorage(carrier);
                settlement.Storage.UnloadCarrier(carrier);
            }
        }

        public async Task TrainMiner(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.CanAddWorker() && settlement.Storage.Consume(WorkerCosts.Miner.Level1))
                {
                    var miner = _workerFactoryService.CreateMiner(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    settlement.Keep.AddMiner(miner);
                }
        }

        public async Task TrainFarmer(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.CanAddWorker() && settlement.Storage.Consume(WorkerCosts.Farmer.Level1))
                {
                    var farmer = _workerFactoryService.CreateFarmer(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    settlement.Keep.AddFarmer(farmer);
                }
        }

        public async Task TrainLumberjack(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.CanAddWorker() && settlement.Storage.Consume(WorkerCosts.Lumberjack.Level1))
                {
                    var lumberjack = _workerFactoryService.CreateLumberjack(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    settlement.Keep.AddLumberjack(lumberjack);
                }
        }

        public async Task MoveMinersToBuilding(Building<Miner> building, Keep keep, params Guid[] minerIds)
        {
            var miners = keep.GetMiners(minerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var miner in miners) building.AddWorker(miner);
        }

        public async Task MoveFarmersToBuilding(Building<Farmer> building, Keep keep, params Guid[] farmerIds)
        {
            var farmers = keep.GetFarmers(farmerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var farmer in farmers) building.AddWorker(farmer);
        }

        public async Task MoveLumberjacksToBuilding(Building<Lumberjack> building, Keep keep,
            params Guid[] lumberjackIds)
        {
            var lumberjacks = keep.GetLumberjacks(lumberjackIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var lumberjack in lumberjacks) building.AddWorker(lumberjack);
        }

        public async Task MoveMinersToKeep(Building<Miner> building, Keep keep, params Guid[] minerIds)
        {
            var miners = building.RemoveWorker(minerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var miner in miners) keep.AddMiner(miner);
        }

        public async Task MoveFarmersToKeep(Building<Farmer> building, Keep keep, params Guid[] farmerIds)
        {
            var farmers = building.RemoveWorker(farmerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var farmer in farmers) keep.AddFarmer(farmer);
        }

        public async Task MoveLumberjacksToKeep(Building<Lumberjack> building, Keep keep, params Guid[] lumberjackIds)
        {
            var lumberjacks = building.RemoveWorker(lumberjackIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var lumberjack in lumberjacks) keep.AddLumberjack(lumberjack);
        }
    }

    public static class ExecutionTimes
    {
        public static int BuildTime { get; set; }
        public static int TrainingTime { get; set; }
        public static int ResourceProductionTime { get; set; }
        public static int CarrierTravelTime { get; set; }
        public static int WorkerTravelTime { get; set; }
    }
}