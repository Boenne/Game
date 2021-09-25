using System.Threading.Tasks;
using Game.ApplicationService.Factories;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Buildings.Settlement;
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
            _engine = new Engine(_buildingFactoryService.Object);

            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
        }

        private readonly Mock<IBuildingFactoryService> _buildingFactoryService;
        private readonly Engine _engine;

        [Fact]
        public async Task AddCopperMine_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateCopperMine(1)).Returns(new CopperMine(1, 0));
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddCopperMine(settlement, 1);

            settlement.CopperMines.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Once);
        }

        [Fact]
        public async Task AddCopperMine_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddCopperMine(settlement, 1);

            settlement.CopperMines.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Never);
        }

        [Fact]
        public async Task AddFarm_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateFarm(1)).Returns(new Farm(1, 0));
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddFarm(settlement, 1);

            settlement.Farms.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Once);
        }

        [Fact]
        public async Task AddFarm_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddFarm(settlement, 1);

            settlement.Farms.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Never);
        }

        [Fact]
        public async Task AddLumberyard_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateLumberyard(1)).Returns(new Lumberyard(1, 0));
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddLumberyard(settlement, 1);

            settlement.Lumberyards.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Once);
        }

        [Fact]
        public async Task AddLumberyard_NoStone_WillNotAdd()
        {
            StartingResources.Stone = new Stone(0);
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddLumberyard(settlement, 1);

            settlement.Lumberyards.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Never);
        }

        [Fact]
        public async Task AddQuarry_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateQuarry(1)).Returns(new Quarry(1, 0));
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddQuarry(settlement, 1);

            settlement.Quarries.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateQuarry(1), Times.Once);
        }

        [Fact]
        public async Task AddQuarry_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = new Settlement(0, 0, 0, 0, 0);

            await _engine.AddQuarry(settlement, 1);

            settlement.Quarries.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateQuarry(1), Times.Never);
        }

        [Fact]
        public async Task StartResourceProduction_NoTimeLimit_WillProduceResourcesForAllBuildingsUntilEmpty()
        {
            StartingResources.Lumber = new Lumber(200);
            StartingResources.Stone = new Stone(200);
            ExecutionTimes.ResourceProductionTime = 0;

            var settlement = new Settlement(0, 0, 0, 0, 0);
            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            settlement.CopperMines.Add(copperMine);
            settlement.Farms.Add(farm);
            settlement.Lumberyards.Add(lumberyard);
            settlement.Quarries.Add(quarry);

            _engine.StartResourceProduction(settlement);
            await Task.Delay(100);
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

            var settlement = new Settlement(0, 0, 0, 0, 0);
            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            settlement.CopperMines.Add(copperMine);
            settlement.Farms.Add(farm);
            settlement.Lumberyards.Add(lumberyard);
            settlement.Quarries.Add(quarry);

            _engine.StartResourceProduction(settlement);
            await Task.Delay(350);
            _engine.StopResourceProduction();

            copperMine.ResourcesGathered.ShouldBe(5);
            farm.ResourcesGathered.ShouldBe(5);
            lumberyard.ResourcesGathered.ShouldBe(5);
            quarry.ResourcesGathered.ShouldBe(5);
        }
    }
}