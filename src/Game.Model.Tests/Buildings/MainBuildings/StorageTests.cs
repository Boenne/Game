using System.Linq;
using Game.Model.Buildings.MainBuildings;
using Game.Model.Buildings.Settings;
using Game.Model.Resources;
using Game.Model.Workers;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings.MainBuildings
{
    public class StorageTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(21)]
        [InlineData(34)]
        [InlineData(47)]
        public void Constructor_CreatesTheCorrectAmountOfCarriers(int numberOfCarriers)
        {
            var storage = new Storage(numberOfCarriers, 0);

            storage.Carriers.Count.ShouldBe(numberOfCarriers);
        }

        [Fact]
        public void CanAfford_HasEnoughFood_NotEnoughLumber_ReturnsFalse()
        {
            StartingResources.Resources = new ResourceList(new Food(10), new Lumber(10));
            var storage = new Storage(2, 10);
            var resourcesToConsume = new ResourceList(new Food(10), new Lumber(15));

            var canAfford = storage.CanAfford(resourcesToConsume);

            canAfford.ShouldBeFalse();
        }

        [Fact]
        public void CanAfford_HasEnoughResources_ReturnsTrue()
        {
            StartingResources.Resources = new ResourceList(new Food(10), new Lumber(10));
            var storage = new Storage(2, 10);
            var resourcesToConsume = new ResourceList(new Food(5), new Lumber(10));

            var canAfford = storage.CanAfford(resourcesToConsume);

            canAfford.ShouldBeTrue();
        }

        [Fact]
        public void Consume_CanAfford_ConsumesResources()
        {
            StartingResources.Resources = new ResourceList(new Food(10), new Lumber(10));
            var storage = new Storage(2, 10);
            var resourcesToConsume = new ResourceList(new Food(10), new Lumber(10));

            var wasConsumed = storage.Consume(resourcesToConsume);

            wasConsumed.ShouldBeTrue();
            storage.Resources[typeof(Food)].ShouldBe(0);
            storage.Resources[typeof(Lumber)].ShouldBe(0);
        }

        [Fact]
        public void Consume_CannotAfford_ReturnsFalse()
        {
            StartingResources.Resources = new ResourceList(new Food(10), new Lumber(10));
            var storage = new Storage(2, 10);
            var resourcesToConsume = new ResourceList(new Food(10), new Lumber(15));

            var wasConsumed = storage.Consume(resourcesToConsume);

            wasConsumed.ShouldBeFalse();
        }

        [Fact]
        public void GetCarriers_IncorrectIds_DoesNothing()
        {
            var storage = new Storage(2, 0);

            var carriers = storage.GetCarriers(string.Empty, string.Empty);

            carriers.ShouldBeEmpty();
            storage.Carriers.Count.ShouldBe(2);
        }

        [Fact]
        public void GetCarriers_RemovesCarriersFromList()
        {
            var storage = new Storage(2, 0);
            var carrierId = storage.Carriers.Select(x => x.Id).First();
            var carrierId2 = storage.Carriers.Select(x => x.Id).Last();

            var carriers = storage.GetCarriers(carrierId, carrierId2);

            carriers.Count.ShouldBe(2);
            storage.Carriers.ShouldBeEmpty();
        }

        [Fact]
        public void UnloadCarrier_AddsQuantityToResource_AndAddsCarrierToList()
        {
            StartingResources.Resources = ResourceList.CreateDefault();
            var storage = new Storage(2, 10);
            var carrier = new Carrier(10);
            carrier.Load(new ResourceList(new Food(10)));

            storage.UnloadCarrier(carrier);

            storage.Carriers.Count.ShouldBe(3);
            storage.Resources[typeof(Food)].ShouldBe(10);
        }
    }
}