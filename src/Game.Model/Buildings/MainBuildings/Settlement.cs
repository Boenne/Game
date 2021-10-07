using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceConsuming;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings.Costs;
using Game.Model.Buildings.Settings.Specifications;
using Game.Model.Factories;
using Game.Model.Maps;
using Game.Model.Resources;
using Game.Model.Workers;
using Game.Model.Workers.Settings.Costs;
using Newtonsoft.Json;

namespace Game.Model.Buildings.MainBuildings
{
    public class Settlement : Identifiable
    {
        private readonly IBuildingFactoryService _buildingFactoryService;

        [JsonProperty] private readonly Map _map;

        private readonly IWorkerFactoryService _workerFactoryService;

        public Settlement(int maximumNumberOfWorkers, int numberOfCarriers, int carrierResourceLimit,
            IToolFactoryService toolFactoryService,
            IWorkerFactoryService workerFactoryService,
            IBuildingFactoryService buildingFactoryService,
            Map map)
        {
            _workerFactoryService = workerFactoryService;
            _buildingFactoryService = buildingFactoryService;
            _map = map;
            MaximumNumberOfWorkers = maximumNumberOfWorkers;
            Keep = new Keep();
            Storage = new Storage(numberOfCarriers, carrierResourceLimit);
            Forge = new Forge(Storage, toolFactoryService);
        }

        public Settlement()
        {
        }

        [JsonProperty] public int MaximumNumberOfWorkers { get; private set; }

        [JsonProperty] public int Level { get; private set; }

        [JsonProperty]
        public List<IResourceProducingBuilding> Buildings { get; private set; } =
            new List<IResourceProducingBuilding>();

        [JsonProperty] public Storage Storage { get; private set; }

        [JsonProperty] public Keep Keep { get; private set; }

        [JsonProperty] public Forge Forge { get; private set; }

        public void ProduceResources()
        {
            lock (Lock)
            {
                Buildings.ForEach(x => x.Produce());
            }
        }

        public bool CanAddWorker()
        {
            lock (Lock)
            {
                return MaximumNumberOfWorkers > Buildings.Sum(x => x.NumberOfWorkers()) +
                       Forge.NumberOfWorkers() +
                       Keep.NumberOfIdleWorkers();
            }
        }

        public async Task BuildCopperMine(int level, ResourceSite resourceSite)
        {
            if (resourceSite.ResourceType != typeof(Copper)) return;
            if (Storage.Consume(BuildingCosts.CopperMine.GetCosts(level)))
            {
                var copperMine = _buildingFactoryService.CreateCopperMine(level, resourceSite.AvailableResources);
                await AddBuilding(copperMine, resourceSite);
            }
        }

        public async Task BuildFarm(int level, ResourceSite resourceSite)
        {
            if (resourceSite.ResourceType != typeof(Food)) return;
            if (Storage.Consume(BuildingCosts.Farm.GetCosts(level)))
            {
                var farm = _buildingFactoryService.CreateFarm(level, resourceSite.AvailableResources);
                await AddBuilding(farm, resourceSite);
            }
        }

        public async Task BuildQuarry(int level, ResourceSite resourceSite)
        {
            if (resourceSite.ResourceType != typeof(Stone)) return;
            if (Storage.Consume(BuildingCosts.Quarry.GetCosts(level)))
            {
                var quarry = _buildingFactoryService.CreateQuarry(level, resourceSite.AvailableResources);
                await AddBuilding(quarry, resourceSite);
            }
        }

        public async Task BuildLumberyard(int level, ResourceSite resourceSite)
        {
            if (resourceSite.ResourceType != typeof(Lumber)) return;
            if (Storage.Consume(BuildingCosts.Lumberyard.GetCosts(level)))
            {
                var lumberyard = _buildingFactoryService.CreateLumberyard(level, resourceSite.AvailableResources);
                await AddBuilding(lumberyard, resourceSite);
            }
        }

        private async Task AddBuilding<T>(T building, ResourceSite resourceSite) where T : Identifiable
        {
            await Task.Delay(ExecutionTimes.BuildTime);
            lock (Lock)
            {
                Buildings.Add((IResourceProducingBuilding) building);
            }

            var coordinates = _map.GetPosition(resourceSite.Id);
            _map.ReplaceLocation(coordinates, building);
        }

        public async Task SendCarriersToBuilding(IResourceProducingBuilding building,
            params Urn[] carrierIds)
        {
            var carriers = Storage.GetCarriers(carrierIds);
            if (!carriers.Any()) return;
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
            if (CanAddWorker() && Storage.Consume(WorkerCosts.Miner.GetCosts(level)))
            {
                var miner = _workerFactoryService.CreateMiner(level);
                await Task.Delay(ExecutionTimes.TrainingTime);
                Keep.AddWorker(miner);
            }
        }

        public async Task TrainFarmer(int level)
        {
            if (CanAddWorker() && Storage.Consume(WorkerCosts.Farmer.GetCosts(level)))
            {
                var farmer = _workerFactoryService.CreateFarmer(level);
                await Task.Delay(ExecutionTimes.TrainingTime);
                Keep.AddWorker(farmer);
            }
        }

        public async Task TrainLumberjack(int level)
        {
            if (CanAddWorker() && Storage.Consume(WorkerCosts.Lumberjack.GetCosts(level)))
            {
                var lumberjack = _workerFactoryService.CreateLumberjack(level);
                await Task.Delay(ExecutionTimes.TrainingTime);
                Keep.AddWorker(lumberjack);
            }
        }

        public async Task TrainBlacksmith(int level)
        {
            if (CanAddWorker() && Storage.Consume(WorkerCosts.Blacksmith.GetCosts(level)))
            {
                var blacksmith = _workerFactoryService.CreateBlacksmith(level);
                await Task.Delay(ExecutionTimes.TrainingTime);
                Keep.AddWorker(blacksmith);
            }
        }

        public async Task MoveWorkersToBuilding<T>(Building<T> building, params Urn[] workerIds) where T : Worker
        {
            var workers = Keep.GetWorkers(workerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var worker in workers) building.AddWorker((T) worker);
        }

        public async Task MoveWorkersToKeep<T>(Building<T> building, params Urn[] workerIds) where T : Worker
        {
            var workers = building.RemoveWorker(workerIds);
            await Task.Delay(ExecutionTimes.WorkerTravelTime);
            foreach (var worker in workers) Keep.AddWorker(worker);
        }

        public async Task Upgrade()
        {
            if (!Storage.Consume(BuildingCosts.Settlement.GetCosts(Level + 1))) return;
            await Task.Delay(ExecutionTimes.SettlementUpgradeTime);
            Level++;
            var settlementSpecification = SettlementSpecifications.GetSpecifications(Level);
            MaximumNumberOfWorkers = settlementSpecification.MaximumNumberOfWorkers;
            Storage.Upgrade(settlementSpecification.NumberOfCarriers, settlementSpecification.MaximumNumberOfWorkers);
            Buildings.ForEach(x => x.UpgradeCarrier(settlementSpecification.CarrierResourceLimit));
        }

        public void EquipWorkerTool(Urn toolId, Worker worker)
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