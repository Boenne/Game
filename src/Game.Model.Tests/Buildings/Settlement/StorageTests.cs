using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model.Buildings.Settings;
using Game.Model.Buildings.Settlement;
using Game.Model.Resources;
using Game.Model.Workers;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings.Settlement
{
    public class StorageTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Constructor_CreatesTheCorrectAmountOfCarriers(int numberOfCarriers)
        {
            var storage = new Storage(numberOfCarriers, 0);

            storage.Carriers.Count.ShouldBe(numberOfCarriers);
        }

        [Fact]
        public void CanAfford_HasEnoughFood_NotEnoughLumber_ReturnsFalse()
        {
            StartingResources.Food = new Food(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(2, 10);

            var canAfford = storage.CanAfford(new List<Resource> {new Food(10), new Lumber(15)});

            canAfford.ShouldBeFalse();
        }

        [Fact]
        public void CanAfford_HasEnoughResources_ReturnsTrue()
        {
            StartingResources.Food = new Food(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(2, 10);

            var canAfford = storage.CanAfford(new List<Resource> {new Food(5), new Lumber(10)});

            canAfford.ShouldBeTrue();
        }

        [Fact]
        public void Consume_CanAfford_ConsumesResources()
        {
            StartingResources.Food = new Food(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(2, 10);

            var wasConsumed = storage.Consume(new List<Resource> {new Food(10), new Lumber(10)});

            wasConsumed.ShouldBeTrue();
            storage.Food.Quantity.ShouldBe(0);
            storage.Lumber.Quantity.ShouldBe(0);
        }

        [Fact]
        public void Consume_CannotAfford_ReturnsFalse()
        {
            StartingResources.Food = new Food(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(2, 10);

            var wasConsumed = storage.Consume(new List<Resource> {new Food(10), new Lumber(15)});

            wasConsumed.ShouldBeFalse();
        }

        [Fact]
        public void GetCarriers_IncorrectIds_DoesNothing()
        {
            var storage = new Storage(2, 0);

            var carriers = storage.GetCarriers(Guid.Empty, Guid.Empty);

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
            StartingResources.Food = new Food(0);
            var storage = new Storage(2, 10);
            var carrier = new Carrier(10);
            carrier.Load(new Food(10));

            storage.UnloadCarrier(carrier);

            storage.Carriers.Count.ShouldBe(3);
            storage.Food.Quantity.ShouldBe(10);
        }
    }
}