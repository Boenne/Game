using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceConsuming;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Costs;
using Game.Model.Factories;
using Game.Model.Workers;
using Game.Model.Workers.Settings.Costs;

namespace Game.Model.Buildings.Settlement
{
    public class Settlement : Identifiable
    {
        private readonly IWorkerFactoryService _workerFactoryService;
        private readonly IBuildingFactoryService _buildingFactoryService;

        public Settlement(int maximumNumberOfWorkers, int numberOfCarriers, int carrierResourceLimit,
            IToolFactoryService toolFactoryService, 
            IWorkerFactoryService workerFactoryService,
            IBuildingFactoryService buildingFactoryService)
        {
            _workerFactoryService = workerFactoryService;
            _buildingFactoryService = buildingFactoryService;
            MaximumNumberOfWorkers = maximumNumberOfWorkers;
            Keep = new Keep();
            Storage = new Storage(numberOfCarriers, carrierResourceLimit);
            Forge = new Forge(Storage, toolFactoryService);
        }

        public int MaximumNumberOfWorkers { get; }
        public List<CopperMine> CopperMines { get; set; } = new List<CopperMine>();
        public List<Quarry> Quarries { get; set; } = new List<Quarry>();
        public List<Lumberyard> Lumberyards { get; set; } = new List<Lumberyard>();
        public List<Farm> Farms { get; set; } = new List<Farm>();

        public Storage Storage { get; set; }
        public Keep Keep { get; set; }
        public Forge Forge { get; set; }

        public void ProduceResources()
        {
            lock (Lock)
            {
                foreach (var copperMine in CopperMines) copperMine.Produce();
                foreach (var quarry in Quarries) quarry.Produce();
                foreach (var farm in Farms) farm.Produce();
                foreach (var lumberyard in Lumberyards) lumberyard.Produce();
            }
        }

        public bool CanAddWorker()
        {
            lock (Lock)
            {
                return MaximumNumberOfWorkers > CopperMines.Sum(x => x.NumberOfWorkers()) +
                       Lumberyards.Sum(x => x.NumberOfWorkers()) +
                       Farms.Sum(x => x.NumberOfWorkers()) +
                       Forge.NumberOfWorkers() +
                       Keep.NumberOfIdleWorkers();
            }
        }

        public async Task BuildCopperMine(int level)
        {
            if (level == 1)
                if (Storage.Consume(BuildingCosts.CopperMine.Level1))
                {
                    var copperMine = _buildingFactoryService.CreateCopperMine(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    lock (Lock)
                    {
                        CopperMines.Add(copperMine);
                    }
                }
        }

        public async Task BuildFarm(int level)
        {
            if (level == 1)
                if (Storage.Consume(BuildingCosts.Farm.Level1))
                {
                    var farm = _buildingFactoryService.CreateFarm(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    lock (Lock)
                    {
                        Farms.Add(farm);
                    }
                }
        }

        public async Task BuildQuarry(int level)
        {
            if (level == 1)
                if (Storage.Consume(BuildingCosts.Quarry.Level1))
                {
                    var quarry = _buildingFactoryService.CreateQuarry(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    lock (Lock)
                    {
                        Quarries.Add(quarry);
                    }
                }
        }

        public async Task BuildLumberyard(int level)
        {
            if (level == 1)
                if (Storage.Consume(BuildingCosts.Lumberyard.Level1))
                {
                    var lumberyard = _buildingFactoryService.CreateLumberyard(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    lock (Lock)
                    {
                        Lumberyards.Add(lumberyard);
                    }
                }
        }

        public async Task SendCarriersToBuilding(IResourceProducingBuilding building,
            params Guid[] carrierIds)
        {
            var carriers = Storage.GetCarriers(carrierIds);
            if (carriers.Count == 0) return;
            foreach (var carrier in carriers)
            {
                building.LoadCarrier(carrier);
                await Task.Delay(ExecutionTimes.CarrierTravelTime);
                building.CarrierArrivedAtStorage(carrier);
                Storage.UnloadCarrier(carrier);
            }
        }

        public async Task TrainMiner(int level)
        {
            if (level == 1)
                if (CanAddWorker() && Storage.Consume(WorkerCosts.Miner.Level1))
                {
                    var miner = _workerFactoryService.CreateMiner(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    Keep.AddWorker(miner);
                }
        }

        public async Task TrainFarmer(int level)
        {
            if (level == 1)
                if (CanAddWorker() && Storage.Consume(WorkerCosts.Farmer.Level1))
                {
                    var farmer = _workerFactoryService.CreateFarmer(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    Keep.AddWorker(farmer);
                }
        }

        public async Task TrainLumberjack(int level)
        {
            if (level == 1)
                if (CanAddWorker() && Storage.Consume(WorkerCosts.Lumberjack.Level1))
                {
                    var lumberjack = _workerFactoryService.CreateLumberjack(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    Keep.AddWorker(lumberjack);
                }
        }

        public async Task MoveWorkersToBuilding<T>(Building<T> building, params Guid[] workerIds) where T : Worker
        {
            var workers = Keep.GetWorkers(workerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var worker in workers.Where(x => x.GetType() == typeof(T)))
            {
                building.AddWorker((T)worker);
            }
        }

        public async Task MoveWorkersToKeep<T>(Building<T> building, params Guid[] workerIds) where T : Worker
        {
            var workers = building.RemoveWorker(workerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var worker in workers.Where(x => x.GetType() == typeof(T)))
            {
                Keep.AddWorker(worker);
            }
        }

        public void EquipWorkerTool(Guid toolId, Worker worker)
        {
            var tool = Forge.GetTool(toolId);
            if (tool != null)
                worker.SetTool(tool);
        }

        public void UnequipWorkerTool(Worker worker)
        {
            var tool = worker.RemoveTool();
            if (tool != null)
                Forge.AddTool(tool);
        }
    }
}