using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.Settlement
{
    public class Keep : Identifiable
    {
        public List<Miner> AvailableMiners { get; } = new List<Miner>();
        public List<Farmer> AvailableFarmers { get; } = new List<Farmer>();
        public List<Lumberjack> AvailableLumberjacks { get; } = new List<Lumberjack>();

        public void AddMiner(Miner miner)
        {
            lock (Lock)
            {
                AvailableMiners.Add(miner);
            }
        }

        public void AddFarmer(Farmer farmer)
        {
            lock (Lock)
            {
                AvailableFarmers.Add(farmer);
            }
        }

        public void AddLumberjack(Lumberjack lumberjack)
        {
            lock (Lock)
            {
                AvailableLumberjacks.Add(lumberjack);
            }
        }

        public List<Miner> GetMiners(params Guid[] ids)
        {
            lock (Lock)
            {
                var miners = AvailableMiners.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var miner in miners)
                {
                    AvailableMiners.Remove(miner);
                }
                return miners;
            }
        }

        public List<Farmer> GetFarmers(params Guid[] ids)
        {
            lock (Lock)
            {
                var farmers = AvailableFarmers.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var farmer in farmers)
                {
                    AvailableFarmers.Remove(farmer);
                }
                return farmers;
            }
        }

        public List<Lumberjack> GetLumberjacks(params Guid[] ids)
        {
            lock (Lock)
            {
                var lumberjacks = AvailableLumberjacks.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var lumberjack in lumberjacks)
                {
                    AvailableLumberjacks.Remove(lumberjack);
                }
                return lumberjacks;
            }
        }

        public int NumberOfIdleWorkers()
        {
            lock (Lock)
            {
                return AvailableMiners.Count + AvailableFarmers.Count + AvailableLumberjacks.Count;
            }
        }
    }
}