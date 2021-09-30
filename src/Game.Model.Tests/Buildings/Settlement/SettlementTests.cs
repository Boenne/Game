using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Factories;
using Game.Model.Items.Tools;
using Game.Model.Maps;
using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;
using Moq;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings.Settlement
{
    public class SettlementTests
    {
        public SettlementTests()
        {
            _buildingFactoryService = new Mock<IBuildingFactoryService>();
            _workerFactoryService = new Mock<IWorkerFactoryService>();
            _toolFactoryService = new Mock<IToolFactoryService>();
        }

        private readonly Mock<IBuildingFactoryService> _buildingFactoryService;
        private readonly Mock<IWorkerFactoryService> _workerFactoryService;
        private readonly Mock<IToolFactoryService> _toolFactoryService;

        private Model.Buildings.Settlement.Settlement GetSettlement(Map map = null)
        {
            return new Model.Buildings.Settlement.Settlement(1, 2, 2, _toolFactoryService.Object,
                _workerFactoryService.Object, _buildingFactoryService.Object, map ?? new Map(0));
        }

        [Fact]
        public async Task BuildCopperMine_CanAfford_WillAdd()
        {
            var copperMine = new CopperMine(1, 0);
            _buildingFactoryService.Setup(x => x.CreateCopperMine(1, 10)).Returns(copperMine);
            StartingResources.Resources = new ResourceList(new Lumber(100), new Stone(100));
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Copper), 10);
            var coordinates = new Coordinates(1, 1);
            map.SetLocation(coordinates, resourceSite);

            await settlement.BuildCopperMine(1, resourceSite);

            settlement.CopperMines.First().ShouldBe(copperMine);
            map.GetPosition(copperMine.Id).ShouldBe(coordinates);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1, 10), Times.Once);
        }

        [Fact]
        public async Task BuildCopperMine_NoLumber_WillNotAdd()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Copper), 10);
            map.SetLocation(new Coordinates(1, 1), resourceSite);

            await settlement.BuildCopperMine(1, resourceSite);

            settlement.CopperMines.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateCopperMine(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task BuildFarm_CanAfford_WillAdd()
        {
            var farm = new Farm(1, 0);
            _buildingFactoryService.Setup(x => x.CreateFarm(1, 10)).Returns(farm);
            StartingResources.Resources = new ResourceList(new Lumber(100), new Stone(100));
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Food), 10);
            var coordinates = new Coordinates(1, 1);
            map.SetLocation(coordinates, resourceSite);

            await settlement.BuildFarm(1, resourceSite);

            settlement.Farms.First().ShouldBe(farm);
            map.GetPosition(farm.Id).ShouldBe(coordinates);
            _buildingFactoryService.Verify(x => x.CreateFarm(1, 10), Times.Once);
        }

        [Fact]
        public async Task BuildFarm_NoLumber_WillNotAdd()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Food), 10);
            map.SetLocation(new Coordinates(1, 1), resourceSite);

            await settlement.BuildFarm(1, resourceSite);

            settlement.Farms.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateFarm(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task BuildLumberyard_CanAfford_WillAdd()
        {
            var lumberyard = new Lumberyard(1, 0);
            _buildingFactoryService.Setup(x => x.CreateLumberyard(1, 10)).Returns(lumberyard);
            StartingResources.Resources = new ResourceList(new Lumber(100), new Stone(100));
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Lumber), 10);
            var coordinates = new Coordinates(1, 1);
            map.SetLocation(coordinates, resourceSite);

            await settlement.BuildLumberyard(1, resourceSite);

            settlement.Lumberyards.First().ShouldBe(lumberyard);
            map.GetPosition(lumberyard.Id).ShouldBe(coordinates);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1, 10), Times.Once);
        }

        [Fact]
        public async Task BuildLumberyard_NoStone_WillNotAdd()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Lumber), 10);
            map.SetLocation(new Coordinates(1, 1), resourceSite);

            await settlement.BuildLumberyard(1, resourceSite);

            settlement.Lumberyards.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateLumberyard(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task BuildQuarry_CanAfford_WillAdd()
        {
            var quarry = new Quarry(1, 0);
            _buildingFactoryService.Setup(x => x.CreateQuarry(1, 10)).Returns(quarry);
            StartingResources.Resources = new ResourceList(new Lumber(100), new Stone(100));
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Stone), 10);
            var coordinates = new Coordinates(1, 1);
            map.SetLocation(coordinates, resourceSite);

            await settlement.BuildQuarry(1, resourceSite);

            settlement.Quarries.First().ShouldBe(quarry);
            map.GetPosition(quarry.Id).ShouldBe(coordinates);
            _buildingFactoryService.Verify(x => x.CreateQuarry(1, 10), Times.Once);
        }

        [Fact]
        public async Task BuildQuarry_NoLumber_WillNotAdd()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var map = new Map(2);
            var settlement = GetSettlement(map);
            var resourceSite = new ResourceSite(typeof(Stone), 10);
            map.SetLocation(new Coordinates(1, 1), resourceSite);

            await settlement.BuildQuarry(1, resourceSite);

            settlement.Quarries.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateQuarry(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void EquipWorkerWithTool_CorrectId_EquipsTool()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var pickaxe = new Pickaxe("", 0, 1);
            settlement.Forge.Tools.Add(pickaxe);

            settlement.EquipWorkerTool(pickaxe.Id, miner);

            miner.Tool.ShouldBe(pickaxe);
        }

        [Fact]
        public void EquipWorkerWithTool_IncorrectId_DoesNotEquipTool()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var pickaxe = new Pickaxe("", 0, 1);
            settlement.Forge.Tools.Add(pickaxe);

            settlement.EquipWorkerTool(Guid.Empty, miner);

            miner.Tool.ShouldBeNull();
        }

        [Fact]
        public void EquipWorkerWithTool_NoToolsInForge_DoesNotEquipTool()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var pickaxe = new Pickaxe("", 0, 1);

            settlement.EquipWorkerTool(pickaxe.Id, miner);

            miner.Tool.ShouldBeNull();
        }

        [Fact]
        public async Task MoveMinersToKeep_InvalidWorkerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            quarry.AddWorker(miner);

            await settlement.MoveWorkersToKeep(quarry, Guid.Empty);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            quarry.NumberOfWorkers().ShouldBe(1);
        }

        [Fact]
        public async Task MoveWorkersToBuilding_InvalidWorkerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            settlement.Keep.AddWorker(miner);

            await settlement.MoveWorkersToBuilding(quarry, Guid.Empty);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            quarry.NumberOfWorkers().ShouldBe(0);
        }

        [Fact]
        public async Task MoveWorkersToBuilding_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            settlement.Keep.AddWorker(miner);
            settlement.Keep.AddWorker(miner2);

            await settlement.MoveWorkersToBuilding(quarry, miner.Id, miner2.Id);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            quarry.NumberOfWorkers().ShouldBe(2);
        }

        [Fact]
        public async Task MoveWorkersToKeep_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            quarry.AddWorker(miner);
            quarry.AddWorker(miner2);

            await settlement.MoveWorkersToKeep(quarry, miner.Id, miner2.Id);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(2);
            quarry.NumberOfWorkers().ShouldBe(0);
        }

        [Fact]
        public async Task SendCarriersToBuilding_InvalidCarrierIds_DoesNothing()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var settlement = GetSettlement();
            var copperMine = new CopperMine(1, 10);
            copperMine.AddWorker(new Miner(0, 10));
            settlement.CopperMines.Add(copperMine);
            copperMine.Produce();

            await settlement.SendCarriersToBuilding(copperMine, Guid.Empty);

            copperMine.ResourcesGathered.ShouldBe(10);
            settlement.Storage.Resources[typeof(Copper)].ShouldBe(0);
            settlement.Storage.Carriers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task SendCarriersToBuilding_TwoCarriersCanCarryTwoResourcesEach_WillSendAndRetrieveResources()
        {
            var settlement = GetSettlement();
            var copperMine = new CopperMine(1, 10);
            copperMine.AddWorker(new Miner(0, 10));
            settlement.CopperMines.Add(copperMine);
            copperMine.Produce();

            await settlement.SendCarriersToBuilding(copperMine,
                settlement.Storage.Carriers.Select(x => x.Id).ToArray());

            copperMine.ResourcesGathered.ShouldBe(6);
            settlement.Storage.Resources[typeof(Copper)].ShouldBe(4);
            settlement.Storage.Carriers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_AndCanAddWorker_AddsFarmer()
        {
            var farmer = new Farmer(1, 0);
            _workerFactoryService.Setup(x => x.CreateFarmer(1)).Returns(farmer);
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.First().ShouldBe(farmer);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Once);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddFarmer()
        {
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Miner(1, 0));

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateFarmer(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task TrainFarmer_CannotAfford_AndCanAddWorker_DoesNotAddFarmer()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateFarmer(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_AndCanAddWorker_AddsLumberjack()
        {
            var lumberjack = new Lumberjack(1, 0);
            _workerFactoryService.Setup(x => x.CreateLumberjack(1)).Returns(lumberjack);
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.First().ShouldBe(lumberjack);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Once);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddLumberjack()
        {
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Miner(1, 0));

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateLumberjack(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CannotAfford_AndCanAddWorker_DoesNotAddLumberjack()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateLumberjack(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task TrainMiner_CanAfford_AndCanAddWorker_AddsMiner()
        {
            var miner = new Miner(1, 0);
            _workerFactoryService.Setup(x => x.CreateMiner(1)).Returns(miner);
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.First().ShouldBe(miner);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Once);
        }

        [Fact]
        public async Task TrainMiner_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddMiner()
        {
            StartingResources.Resources = new ResourceList(new Food(100));
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Farmer(1, 0));

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateMiner(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task TrainMiner_CannotAfford_AndCanAddWorker_DoesNotAddMiner()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var settlement = GetSettlement();

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateMiner(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void UnequipWorkerTool_HasNoTool_DoesNothing()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);

            settlement.UnequipWorkerTool(miner);

            settlement.Forge.Tools.ShouldBeEmpty();
            miner.HasTool().ShouldBeFalse();
        }

        [Fact]
        public void UnequipWorkerTool_HasTool_UnequipsCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var pickaxe = new Pickaxe("", 0, 1);
            miner.SetTool(pickaxe);

            settlement.UnequipWorkerTool(miner);

            settlement.Forge.Tools.First().ShouldBe(pickaxe);
            miner.HasTool().ShouldBeFalse();
        }
    }
}