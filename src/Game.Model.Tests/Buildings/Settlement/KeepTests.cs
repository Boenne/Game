using System;
using System.Linq;
using Game.Model.Buildings.Settlement;
using Game.Model.Workers.ResourceProducing;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings.Settlement
{
    public class KeepTests
    {
        [Fact]
        public void AddWorker_AddsWorker()
        {
            var keep = new Keep();
            var miner = new Miner(1, 0);

            keep.AddWorker(miner);

            keep.AvailableWorkers.First().ShouldBe(miner);
        }

        [Fact]
        public void GetWorkers_IncorrectIds_DoesNothing()
        {
            var keep = new Keep();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            keep.AddWorker(miner);
            keep.AddWorker(miner2);

            var workers = keep.GetWorkers(Guid.Empty, Guid.Empty);

            workers.ShouldBeEmpty();
            keep.AvailableWorkers.Count.ShouldBe(2);
        }

        [Fact]
        public void GetWorkers_RemovesWorkersFromList()
        {
            var keep = new Keep();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            keep.AddWorker(miner);
            keep.AddWorker(miner2);

            var workers = keep.GetWorkers(miner.Id, miner2.Id);

            workers.Count.ShouldBe(2);
            keep.AvailableWorkers.ShouldBeEmpty();
        }

        [Fact]
        public void NumberOfIdleWorkers_ReturnsCorrectCount()
        {
            var keep = new Keep();
            var miner = new Miner(1, 0);
            var miner2 = new Miner(1, 0);
            keep.AddWorker(miner);
            keep.AddWorker(miner2);

            var numberOfIdleWorkers = keep.NumberOfIdleWorkers();

            numberOfIdleWorkers.ShouldBe(2);
        }
    }
}