using System.Threading.Tasks;
using Game.Model;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Buildings.Settlement;
using Game.Model.Factories;
using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;
using Moq;
using Shouldly;
using Xunit;

namespace Game.ApplicationService.Tests
{
    public class EngineTests
    {
        public EngineTests()
        {
            _buildingFactoryService = new Mock<IBuildingFactoryService>();
            _workerFactoryService = new Mock<IWorkerFactoryService>();
            _toolFactoryService = new Mock<IToolFactoryService>();
            _engine = new Engine(_buildingFactoryService.Object, _workerFactoryService.Object);
            StartingResources.Lumber = new Lumber(200);
            StartingResources.Stone = new Stone(200);
            _settlement = new Settlement(2, 2, 2, _toolFactoryService.Object, _workerFactoryService.Object,
                _buildingFactoryService.Object);
        }

        private readonly Engine _engine;
        private readonly Settlement _settlement;
        private readonly Mock<IBuildingFactoryService> _buildingFactoryService;
        private readonly Mock<IWorkerFactoryService> _workerFactoryService;
        private readonly Mock<IToolFactoryService> _toolFactoryService;

        [Fact]
        public async Task StartResourceProduction_NoTimeLimit_WillProduceResourcesForAllBuildingsUntilEmpty()
        {
            ExecutionTimes.ResourceProductionTime = 0;

            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            _settlement.CopperMines.Add(copperMine);
            _settlement.Farms.Add(farm);
            _settlement.Lumberyards.Add(lumberyard);
            _settlement.Quarries.Add(quarry);

            _engine.StartResourceProduction(_settlement);
            await Task.Delay(50);
            _engine.StopResourceProduction();

            copperMine.ResourcesGathered.ShouldBe(10);
            farm.ResourcesGathered.ShouldBe(10);
            lumberyard.ResourcesGathered.ShouldBe(10);
            quarry.ResourcesGathered.ShouldBe(10);
        }

        [Fact]
        public async Task StartResourceProduction_TimeForFiveRepetitions_WillProduceFiveResourcesForAllBuildings()
        {
            StartingResources.Lumber = new Lumber(200);
            StartingResources.Stone = new Stone(200);
            ExecutionTimes.ResourceProductionTime = 50;

            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            _settlement.CopperMines.Add(copperMine);
            _settlement.Farms.Add(farm);
            _settlement.Lumberyards.Add(lumberyard);
            _settlement.Quarries.Add(quarry);

            _engine.StartResourceProduction(_settlement);
            await Task.Delay(350);
            _engine.StopResourceProduction();

            copperMine.ResourcesGathered.ShouldBe(5);
            farm.ResourcesGathered.ShouldBe(5);
            lumberyard.ResourcesGathered.ShouldBe(5);
            quarry.ResourcesGathered.ShouldBe(5);
        }
    }
}