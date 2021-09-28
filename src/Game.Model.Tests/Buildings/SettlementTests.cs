using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Buildings.Settlement;
using Game.Model.Factories;
using Game.Model.Resources;
using Game.Model.Workers.ResourceProducing;
using Moq;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings
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
        public async Task AddCopperMine_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateCopperMine(1)).Returns(new CopperMine(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.AddCopperMine(1);

            settlement.CopperMines.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Once);
        }

        [Fact]
        public async Task AddCopperMine_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.AddCopperMine(1);

            settlement.CopperMines.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateCopperMine(1), Times.Never);
        }

        [Fact]
        public async Task AddFarm_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateFarm(1)).Returns(new Farm(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.AddFarm(1);

            settlement.Farms.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Once);
        }

        [Fact]
        public async Task AddFarm_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.AddFarm(1);

            settlement.Farms.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateFarm(1), Times.Never);
        }

        [Fact]
        public async Task AddLumberyard_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateLumberyard(1)).Returns(new Lumberyard(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.AddLumberyard(1);

            settlement.Lumberyards.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Once);
        }

        [Fact]
        public async Task AddLumberyard_NoStone_WillNotAdd()
        {
            StartingResources.Stone = new Stone(0);
            var settlement = GetSettlement();

            await settlement.AddLumberyard(1);

            settlement.Lumberyards.Count.ShouldBe(0);
            _buildingFactoryService.Verify(x => x.CreateLumberyard(1), Times.Never);
        }

        [Fact]
        public async Task AddQuarry_CanAfford_WillAdd()
        {
            _buildingFactoryService.Setup(x => x.CreateQuarry(1)).Returns(new Quarry(1, 0));
            StartingResources.Lumber = new Lumber(100);
            StartingResources.Stone = new Stone(100);
            var settlement = GetSettlement();

            await settlement.AddQuarry(1);

            settlement.Quarries.Count.ShouldBe(1);
            _buildingFactoryService.Verify(x => x.CreateQuarry(1), Times.Once);
        }

        [Fact]
        public async Task AddQuarry_NoLumber_WillNotAdd()
        {
            StartingResources.Lumber = new Lumber(0);
            var settlement = GetSettlement();

            await settlement.AddQuarry(1);

            settlement.Quarries.Count.ShouldBe(0);
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

            settlement.Keep.AvailableMiners.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Once);
        }

        [Fact]
        public async Task TrainMiner_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddMiner()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddFarmer(new Farmer(1, 0));

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableMiners.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Never);
        }

        [Fact]
        public async Task TrainMiner_CannotAfford_AndCanAddWorker_DoesNotAddMiner()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainMiner(1);

            settlement.Keep.AvailableMiners.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateMiner(1), Times.Never);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_AndCanAddWorker_AddsFarmer()
        {
            _workerFactoryService.Setup(x => x.CreateFarmer(1)).Returns(new Farmer(1, 0));
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableFarmers.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Once);
        }

        [Fact]
        public async Task TrainFarmer_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddFarmer()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddMiner(new Miner(1, 0));

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableFarmers.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Never);
        }

        [Fact]
        public async Task TrainFarmer_CannotAfford_AndCanAddWorker_DoesNotAddFarmer()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainFarmer(1);

            settlement.Keep.AvailableFarmers.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateFarmer(1), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_AndCanAddWorker_AddsLumberjack()
        {
            _workerFactoryService.Setup(x => x.CreateLumberjack(1)).Returns(new Lumberjack(1, 0));
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(1);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Once);
        }

        [Fact]
        public async Task TrainLumberjack_CanAfford_MaximumNumberOfWorkersReached_DoesNotAddLumberjack()
        {
            StartingResources.Food = new Food(100);
            var settlement = GetSettlement();
            settlement.Keep.AddMiner(new Miner(1, 0));

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Never);
        }

        [Fact]
        public async Task TrainLumberjack_CannotAfford_AndCanAddWorker_DoesNotAddLumberjack()
        {
            StartingResources.Food = new Food(0);
            var settlement = GetSettlement();

            await settlement.TrainLumberjack(1);

            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(0);
            _workerFactoryService.Verify(x => x.CreateLumberjack(1), Times.Never);
        }

        [Fact]
        public async Task MoveMinersToBuilding_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            settlement.Keep.AddMiner(miner);
            settlement.Keep.AddMiner(miner2);

            await settlement.MoveMinersToBuilding(quarry, miner.Id, miner2.Id);

            settlement.Keep.AvailableMiners.Count.ShouldBe(0);
            quarry.NumberOfWorkers.ShouldBe(2);
        }

        [Fact]
        public async Task MoveMinersToBuilding_InvalidMinerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            settlement.Keep.AddMiner(miner);

            await settlement.MoveMinersToBuilding(quarry, Guid.Empty);

            settlement.Keep.AvailableMiners.Count.ShouldBe(1);
            quarry.NumberOfWorkers.ShouldBe(0);
        }

        [Fact]
        public async Task MoveFarmersToBuilding_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var farmer = new Farmer(1, 0);
            var farmer2 = new Farmer(1, 0);
            var farm = new Farm(1, 0);
            settlement.Keep.AddFarmer(farmer);
            settlement.Keep.AddFarmer(farmer2);

            await settlement.MoveFarmersToBuilding(farm, farmer.Id, farmer2.Id);

            settlement.Keep.AvailableFarmers.Count.ShouldBe(0);
            farm.NumberOfWorkers.ShouldBe(2);
        }

        [Fact]
        public async Task MoveFarmersToBuilding_InvalidFarmerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var farmer = new Farmer(1, 0);
            var farm = new Farm(1, 0);
            settlement.Keep.AddFarmer(farmer);

            await settlement.MoveFarmersToBuilding(farm, Guid.Empty);

            settlement.Keep.AvailableFarmers.Count.ShouldBe(1);
            farm.NumberOfWorkers.ShouldBe(0);
        }

        [Fact]
        public async Task MoveLumberjacksToBuilding_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var lumberjack = new Lumberjack(1, 0);
            var lumberjack2 = new Lumberjack(1, 0);
            var lumberyard = new Lumberyard(1, 0);
            settlement.Keep.AddLumberjack(lumberjack);
            settlement.Keep.AddLumberjack(lumberjack2);

            await settlement.MoveLumberjacksToBuilding(lumberyard, lumberjack.Id, lumberjack2.Id);

            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(0);
            lumberyard.NumberOfWorkers.ShouldBe(2);
        }

        [Fact]
        public async Task MoveLumberjacksToBuilding_InvalidLumberjackIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var lumberjack = new Lumberjack(1, 0);
            var lumberyard = new Lumberyard(1, 0);
            settlement.Keep.AddLumberjack(lumberjack);

            await settlement.MoveLumberjacksToBuilding(lumberyard, Guid.Empty);

            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(1);
            lumberyard.NumberOfWorkers.ShouldBe(0);
        }

        [Fact]
        public async Task MoveMinersToKeep_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            quarry.AddWorker(miner);
            quarry.AddWorker(miner2);

            await settlement.MoveMinersToKeep(quarry, miner.Id, miner2.Id);

            quarry.NumberOfWorkers.ShouldBe(0);
            settlement.Keep.AvailableMiners.Count.ShouldBe(2);
        }

        [Fact]
        public async Task MoveMinersToKeep_InvalidMinerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var miner = new Miner(1, 0);
            var quarry = new Quarry(1, 0);
            quarry.AddWorker(miner);

            await settlement.MoveMinersToKeep(quarry, Guid.Empty);

            quarry.NumberOfWorkers.ShouldBe(1);
            settlement.Keep.AvailableMiners.Count.ShouldBe(0);
        }

        [Fact]
        public async Task MoveFarmersToKeep_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var farmer = new Farmer(1, 0);
            var farmer2 = new Farmer(1, 0);
            var farm = new Farm(1, 0);
            farm.AddWorker(farmer);
            farm.AddWorker(farmer2);

            await settlement.MoveFarmersToKeep(farm, farmer.Id, farmer2.Id);

            farm.NumberOfWorkers.ShouldBe(0);
            settlement.Keep.AvailableFarmers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task MoveFarmersToKeep_InvalidFarmerIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var farmer = new Farmer(1, 0);
            var farm = new Farm(1, 0);
            farm.AddWorker(farmer);

            await settlement.MoveFarmersToKeep(farm, Guid.Empty);

            farm.NumberOfWorkers.ShouldBe(1);
            settlement.Keep.AvailableFarmers.Count.ShouldBe(0);
        }

        [Fact]
        public async Task MoveLumberjacksToKeep_MovesCorrectly()
        {
            var settlement = GetSettlement();
            var lumberjack = new Lumberjack(1, 0);
            var lumberjack2 = new Lumberjack(1, 0);
            var lumberyard = new Lumberyard(1, 0);
            lumberyard.AddWorker(lumberjack);
            lumberyard.AddWorker(lumberjack2);

            await settlement.MoveLumberjacksToKeep(lumberyard, lumberjack.Id, lumberjack2.Id);

            lumberyard.NumberOfWorkers.ShouldBe(0);
            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(2);
        }

        [Fact]
        public async Task MoveLumberjacksToKeep_InvalidLumberjackIds_DoesNothing()
        {
            var settlement = GetSettlement();
            var lumberjack = new Lumberjack(1, 0);
            var lumberyard = new Lumberyard(1, 0);
            lumberyard.AddWorker(lumberjack);

            await settlement.MoveLumberjacksToKeep(lumberyard, Guid.Empty);

            lumberyard.NumberOfWorkers.ShouldBe(1);
            settlement.Keep.AvailableLumberjacks.Count.ShouldBe(0);
        }

        private Settlement GetSettlement()
        {
            return new Settlement(1, 2, 2, _toolFactoryService.Object, _workerFactoryService.Object, _buildingFactoryService.Object);
        }
    }
}