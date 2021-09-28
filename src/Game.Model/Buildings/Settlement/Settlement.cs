using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceConsuming;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Costs;
using Game.Model.Factories;
using Game.Model.Workers;
using Game.Model.Workers.ResourceProducing;
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
                return MaximumNumberOfWorkers > CopperMines.Sum(x => x.NumberOfWorkers) +
                       Forge.NumberOfWorkers +
                       Lumberyards.Sum(x => x.NumberOfWorkers) +
                       Farms.Sum(x => x.NumberOfWorkers) +
                       Keep.NumberOfIdleWorkers();
            }
        }

        public async Task AddCopperMine(int level)
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

        public async Task AddFarm(int level)
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

        public async Task AddQuarry(int level)
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

        public async Task AddLumberyard(int level)
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
                    Keep.AddMiner(miner);
                }
        }

        public async Task TrainFarmer(int level)
        {
            if (level == 1)
                if (CanAddWorker() && Storage.Consume(WorkerCosts.Farmer.Level1))
                {
                    var farmer = _workerFactoryService.CreateFarmer(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    Keep.AddFarmer(farmer);
                }
        }

        public async Task TrainLumberjack(int level)
        {
            if (level == 1)
                if (CanAddWorker() && Storage.Consume(WorkerCosts.Lumberjack.Level1))
                {
                    var lumberjack = _workerFactoryService.CreateLumberjack(level);
                    await Task.Delay(ExecutionTimes.TrainingTime);
                    Keep.AddLumberjack(lumberjack);
                }
        }

        public async Task MoveMinersToBuilding(Building<Miner> building, params Guid[] minerIds)
        {
            var miners = Keep.GetMiners(minerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var miner in miners) building.AddWorker(miner);
        }

        public async Task MoveFarmersToBuilding(Building<Farmer> building, params Guid[] farmerIds)
        {
            var farmers = Keep.GetFarmers(farmerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var farmer in farmers) building.AddWorker(farmer);
        }

        public async Task MoveLumberjacksToBuilding(Building<Lumberjack> building,
            params Guid[] lumberjackIds)
        {
            var lumberjacks = Keep.GetLumberjacks(lumberjackIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var lumberjack in lumberjacks) building.AddWorker(lumberjack);
        }

        public async Task MoveMinersToKeep(Building<Miner> building, params Guid[] minerIds)
        {
            var miners = building.RemoveWorker(minerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var miner in miners) Keep.AddMiner(miner);
        }

        public async Task MoveFarmersToKeep(Building<Farmer> building, params Guid[] farmerIds)
        {
            var farmers = building.RemoveWorker(farmerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var farmer in farmers) Keep.AddFarmer(farmer);
        }

        public async Task MoveLumberjacksToKeep(Building<Lumberjack> building, params Guid[] lumberjackIds)
        {
            var lumberjacks = building.RemoveWorker(lumberjackIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var lumberjack in lumberjacks) Keep.AddLumberjack(lumberjack);
        }

        public void EquipWorkerWithTool(Guid id, Worker worker)
        {
            var tool = Forge.GetTool(id);
            if (tool != null)
                worker.SetTool(tool);
        }
    }
}