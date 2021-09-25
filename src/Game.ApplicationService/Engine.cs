using System.Threading.Tasks;
using Game.ApplicationService.Factories;
using Game.Model.Buildings.Settings.Costs;
using Game.Model.Buildings.Settlement;

namespace Game.ApplicationService
{
    public class Engine
    {
        private readonly IBuildingFactoryService _buildingFactoryService;
        private bool _produceResources = true;

        public Engine(IBuildingFactoryService buildingFactoryService)
        {
            _buildingFactoryService = buildingFactoryService;
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
                if (settlement.Storage.CanAfford(BuildingCosts.CopperMine.Level1))
                {
                    settlement.Storage.Consume(BuildingCosts.CopperMine.Level1);
                    var copperMine = _buildingFactoryService.CreateCopperMine(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddCopperMine(copperMine);
                }
        }

        public async Task AddFarm(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.CanAfford(BuildingCosts.Farm.Level1))
                {
                    settlement.Storage.Consume(BuildingCosts.Farm.Level1);
                    var farm = _buildingFactoryService.CreateFarm(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddFarm(farm);
                }
        }

        public async Task AddQuarry(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.CanAfford(BuildingCosts.Quarry.Level1))
                {
                    settlement.Storage.Consume(BuildingCosts.Quarry.Level1);
                    var quarry = _buildingFactoryService.CreateQuarry(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddQuarry(quarry);
                }
        }

        public async Task AddLumberyard(Settlement settlement, int level)
        {
            if (level == 1)
                if (settlement.Storage.CanAfford(BuildingCosts.Lumberyard.Level1))
                {
                    settlement.Storage.Consume(BuildingCosts.Lumberyard.Level1);
                    var lumberyard = _buildingFactoryService.CreateLumberyard(level);
                    await Task.Delay(ExecutionTimes.BuildTime);
                    settlement.AddLumberyard(lumberyard);
                }
        }
    }

    public static class ExecutionTimes
    {
        public static int BuildTime { get; set; }
        public static int ResourceProductionTime { get; set; }
    }
}