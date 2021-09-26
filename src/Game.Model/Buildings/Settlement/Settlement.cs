using System.Collections.Generic;
using System.Linq;
using Game.Model.Buildings.ResourceConsuming;
using Game.Model.Buildings.ResourceProducing;

namespace Game.Model.Buildings.Settlement
{
    public class Settlement
    {
        public Settlement(int maximumNumberOfWorkers, int numberOfCarriers, int carrierResourceLimit)
        {
            MaximumNumberOfWorkers = maximumNumberOfWorkers;
            Keep = new Keep();
            Storage = new Storage(numberOfCarriers, carrierResourceLimit);
        }

        public int MaximumNumberOfWorkers { get; }
        public object Lock { get; } = new object();
        public List<CopperMine> CopperMines { get; set; } = new List<CopperMine>();
        public List<Quarry> Quarries { get; set; } = new List<Quarry>();
        public List<Forge> Forges { get; set; } = new List<Forge>();
        public List<Lumberyard> Lumberyards { get; set; } = new List<Lumberyard>();
        public List<Farm> Farms { get; set; } = new List<Farm>();
        public List<Barrack> Barracks { get; set; } = new List<Barrack>();

        public Storage Storage { get; set; }
        public Keep Keep { get; set; }

        public void AddCopperMine(CopperMine copperMine)
        {
            lock (Lock)
            {
                CopperMines.Add(copperMine);
            }
        }

        public void AddFarm(Farm farm)
        {
            lock (Lock)
            {
                Farms.Add(farm);
            }
        }

        public void AddLumberyard(Lumberyard lumberyard)
        {
            lock (Lock)
            {
                Lumberyards.Add(lumberyard);
            }
        }

        public void AddQuarry(Quarry quarry)
        {
            lock (Lock)
            {
                Quarries.Add(quarry);
            }
        }

        public void AddBarrack(Barrack barrack)
        {
            Barracks.Add(barrack);
        }

        public void ProduceResources()
        {
            lock (Lock)
            {
                foreach (var copperMine in CopperMines) copperMine.Produce();
                foreach (var quarry in Quarries) quarry.Produce();
                foreach (var farm in Farms) farm.Produce();
                foreach (var lumberyard in Lumberyards) lumberyard.Produce();
            }
        }

        public bool CanAddWorker()
        {
            lock (Lock)
            {
                return MaximumNumberOfWorkers > CopperMines.Sum(x => x.NumberOfWorkers) +
                       Forges.Sum(x => x.NumberOfWorkers) +
                       Lumberyards.Sum(x => x.NumberOfWorkers) +
                       Farms.Sum(x => x.NumberOfWorkers) +
                       Barracks.Sum(x => x.NumberOfWorkers) +
                       Keep.NumberOfIdleWorkers();
            }
        }
    }
}