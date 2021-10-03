using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Game.Model;
using Game.Model.Buildings.MainBuildings;
using Game.Model.Buildings.ResourceProducing;
using Game.Model.Buildings.Settings;
using Game.Model.Factories;
using Game.Model.Items.Tools;
using Game.Model.Maps;
using Game.Model.Resources;
using Game.Model.Workers.ResourceConsuming;
using Game.Model.Workers.ResourceProducing;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Game.Application.Tests
{
    public class EngineTests
    {
        public EngineTests(Map map = null)
        {
            _engine = new Engine(new Mock<IBuildingFactoryService>().Object, new Mock<IWorkerFactoryService>().Object);
        }

        private readonly Engine _engine;

        [Fact]
        public async Task StartResourceProduction_NoTimeLimit_WillProduceResourcesForAllBuildingsUntilEmpty()
        {
            ExecutionTimes.ResourceProductionTime = 0;
            var settlement = CreateSettlement();
            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            settlement.Buildings.Add(copperMine);
            settlement.Buildings.Add(farm);
            settlement.Buildings.Add(lumberyard);
            settlement.Buildings.Add(quarry);

            _engine.StartResourceProduction(settlement);
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
            ExecutionTimes.ResourceProductionTime = 50;
            var settlement = CreateSettlement();
            var copperMine = new CopperMine(1, 10);
            var farm = new Farm(1, 10);
            var lumberyard = new Lumberyard(1, 10);
            var quarry = new Quarry(1, 10);
            copperMine.AddWorker(new Miner(0, 1));
            farm.AddWorker(new Farmer(0, 1));
            lumberyard.AddWorker(new Lumberjack(0, 1));
            quarry.AddWorker(new Miner(0, 1));

            settlement.Buildings.Add(copperMine);
            settlement.Buildings.Add(farm);
            settlement.Buildings.Add(lumberyard);
            settlement.Buildings.Add(quarry);

            _engine.StartResourceProduction(settlement);
            await Task.Delay(350);
            _engine.StopResourceProduction();

            copperMine.ResourcesGathered.ShouldBe(5);
            farm.ResourcesGathered.ShouldBe(5);
            lumberyard.ResourcesGathered.ShouldBe(5);
            quarry.ResourcesGathered.ShouldBe(5);
        }

        [Fact]
        public async Task SaveGame()
        {
            var map = new Map(10);
            var settlement = CreateSettlement(map);
            var copperMine = new CopperMine(1, 100);
            var farm = new Farm(1, 100);
            var lumberyard = new Lumberyard(1, 100);
            var quarry = new Quarry(1, 100);
            var miner = new Miner(1, 5);
            var farmer = new Farmer(1, 5);
            var blacksmith = new Blacksmith(3, 5);
            var pickaxe = new Pickaxe("Stone Pickaxe", 2, 1);
            var rake = new Rake("Wooden Rake", 3, 1);
            var hammer = new Hammer("Stone Hammer", 4, 2);

            farmer.SetTool(rake);
            miner.SetTool(pickaxe);
            copperMine.AddWorker(miner);
            farm.AddWorker(farmer);
            map.SetLocation(new Coordinates(0, 0), quarry);
            map.SetLocation(new Coordinates(1, 2), farm);
            map.SetLocation(new Coordinates(2, 2), new ResourceSite(typeof(Copper), 3));
            map.SetLocation(new Coordinates(3, 2), new ResourceSite(typeof(Lumber), 6));

            settlement.Forge.AddTool(hammer);
            settlement.Keep.AddWorker(blacksmith);
            settlement.Buildings.Add(copperMine);
            settlement.Buildings.Add(farm);
            settlement.Buildings.Add(lumberyard);
            settlement.Buildings.Add(quarry);
            settlement.ProduceResources();
            var storageCarrier = settlement.Storage.Carriers.First();
            var farmCarrier = settlement.Storage.Carriers.Last();

            var carriers = settlement.Storage.GetCarriers(farmCarrier.Id);
            farm.LoadCarrier(carriers.First());

            _engine.SaveGame(settlement);

            var newSettlement = JsonConvert.DeserializeObject<Settlement>(File.ReadAllText("savedgame2.json"),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                });

            newSettlement.BaseId.ShouldBe(settlement.BaseId);
            newSettlement.MaximumNumberOfWorkers.ShouldBe(settlement.MaximumNumberOfWorkers);
            newSettlement.Level.ShouldBe(settlement.Level);

            var newCarrier = newSettlement.Storage.Carriers.Single();
            newCarrier.BaseId.ShouldBe(storageCarrier.BaseId);
            newCarrier.MaxResourceLimit.ShouldBe(storageCarrier.MaxResourceLimit);


            newSettlement.Forge.BaseId.ShouldBe(settlement.Forge.BaseId);
            var newBlacksmith = newSettlement.Forge.Workers.Single();
            newBlacksmith.BaseId.ShouldBe(blacksmith.BaseId);
            newBlacksmith.CraftingSpeedReduction.ShouldBe(blacksmith.CraftingSpeedReduction);
            newBlacksmith.Level.ShouldBe(blacksmith.Level);


            var newHammer = newSettlement.Forge.Tools.Single();
            newHammer.BaseId.ShouldBe(hammer.BaseId);
            newHammer.Name.ShouldBe(hammer.Name);
            newHammer.Modifier.ShouldBe(hammer.Modifier);
            newHammer.Level.ShouldBe(hammer.Level);
        }

        [Fact]
        public void Test()
        {
            var settlement = JsonConvert.DeserializeObject<Settlement>(File.ReadAllText("savedgame2.json"),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                });
            
        }


        private Settlement CreateSettlement(Map map  = null)
        {
            StartingResources.Resources = new ResourceList
            {
                {typeof(Lumber), 200},
                {typeof(Stone), 200}
            };
            return new Settlement(2, 2, 2, new Mock<IToolFactoryService>().Object, new Mock<IWorkerFactoryService>().Object,
                new Mock<IBuildingFactoryService>().Object, map ?? new Map(1));
        }
    }
}