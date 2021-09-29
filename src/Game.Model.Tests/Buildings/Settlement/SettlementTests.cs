using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Factories;
using Game.Model.Items.Tools;
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

        [Fact]
        public async Task BuildCopperMine_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateCopperMine(1)).Returns(new CopperMine(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.BuildCopperMine(1);

            settlement.CopperMines.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Once);
        }

        [Fact]
        public async Task BuildCopperMine_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.BuildCopperMine(1);

            settlement.CopperMines.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Never);
        }

        [Fact]
        public async Task BuildFarm_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateFarm(1)).Returns(new Farm(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.BuildFarm(1);

            settlement.Farms.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Once);
        }

        [Fact]
        public async Task BuildFarm_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.BuildFarm(1);

            settlement.Farms.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Never);
        }

        [Fact]
        public async Task BuildLumberyard_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateLumberyard(1)).Returns(new Lumberyard(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.BuildLumberyard(1);

            settlement.Lumberyards.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Once);
        }

        [Fact]
        public async Task BuildLumberyard_NoStone_WillNotAdd()
        {
            StartingResources.Stone = new Stone(0);
            var settlement = GetSettlement();

            await settlement.BuildLumberyard(1);

            settlement.Lumberyards.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Never);
        }

        [Fact]
        public async Task BuildQuarry_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateQuarry(1)).Returns(new Quarry(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.BuildQuarry(1);

            settlement.Quarries.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateQuarry(1), Times.Once);
        }

        [Fact]
        public async Task BuildQuarry_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.BuildQuarry(1);

            settlement.Quarries.ShouldBeEmpty();
            _buildingFactoryService.Verify(x => x.CreateQuarry(1), Times.Never);
        }

        [Fact]
        public async Task SendCarriersToBuilding_InvalidCarrierIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var copperMine = new CopperMine(1, 10);
            copperMine.AddWorker(new Miner(0, 10));
            settlement.CopperMines.Add(copperMine);
            copperMine.Produce();

            await settlement.SendCarriersToBuilding(copperMine, Guid.Empty);

            copperMine.ResourcesGathered.ShouldBe(10);
            settlement.Storage.Copper.Quantity.ShouldBe(0);
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

            await settlement.SendCarriersToBuilding(copperMine, settlement.Storage.Carriers.Select(x => x.Id).ToArray());

            copperMine.ResourcesGathered.ShouldBe(6);
            settlement.Storage.Copper.Quantity.ShouldBe(4);
            settlement.Storage.Carriers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task TrainMiner_CanAfford_AndCanAddWorker_AddsMiner()
        {
            _workerFactoryService.Setup(x => x.CreateMiner(1)).Returns(new Miner(1, 0));
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Once);
        }

        [Fact]
        public async Task TrainMiner_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddMiner()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Farmer(1, 0));

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Never);
        }

        [Fact]
        public async Task TrainMiner_CannotAfford_AndCanAddWorker_DoesNotAddMiner()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Never);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_AndCanAddWorker_AddsFarmer()
        {
            _workerFactoryService.Setup(x => x.CreateFarmer(1)).Returns(new Farmer(1, 0));
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Once);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddFarmer()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Miner(1, 0));

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Never);
        }

        [Fact]
        public async Task TrainFarmer_CannotAfford_AndCanAddWorker_DoesNotAddFarmer()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_AndCanAddWorker_AddsLumberjack()
        {
            _workerFactoryService.Setup(x => x.CreateLumberjack(1)).Returns(new Lumberjack(1, 0));
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Once);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddLumberjack()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddWorker(new Miner(1, 0));

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CannotAfford_AndCanAddWorker_DoesNotAddLumberjack()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableWorkers.ShouldBeEmpty();
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Never);
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
        public void UnequipWorkerTool_HasTool_UnequipsCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var pickaxe = new Pickaxe("", 0, 1);
            miner.SetTool(pickaxe);

            settlement.UnequipWorkerTool(miner);

            settlement.Forge.Tools.Count.ShouldBe(1);
            miner.HasTool().ShouldBeFalse();
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

        private Model.Buildings.Settlement.Settlement GetSettlement()
        {
            return new Model.Buildings.Settlement.Settlement(1, 2, 2, _toolFactoryService.Object, _workerFactoryService.Object, _buildingFactoryService.Object);
        }
    }
}