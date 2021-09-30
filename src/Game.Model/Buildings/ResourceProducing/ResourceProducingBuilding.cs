﻿using System.Collections.Generic;
using System.Linq;
using Game.Model.Resources;
using Game.Model.Workers;
using Game.Model.Workers.ResourceProducing;

namespace Game.Model.Buildings.ResourceProducing
{
    public abstract class ResourceProducingBuilding<TWorker, TResource> : Building<TWorker>, IResourceProducingBuilding
        where TWorker : ResourceProducingWorker where TResource : Resource
    {
        protected ResourceProducingBuilding(int level, int availableResources)
        {
            AvailableResources = availableResources;
            Level = level;
        }

        public int AvailableResources { get; private set; }
        public int ResourcesGathered { get; private set; }

        public List<Carrier> CarriersGoingBackToStorage { get; } = new List<Carrier>();

        public void Produce()
        {
            lock (Lock)
            {
                var totalOutput = Workers.Sum(x => x.TotalOutput());
                if (AvailableResources >= totalOutput)
                {
                    AvailableResources -= totalOutput;
                    ResourcesGathered += totalOutput;
                }
                else
                {
                    ResourcesGathered += AvailableResources;
                    AvailableResources = 0;
                }
            }
        }

        public void LoadCarrier(Carrier carrier)
        {
            lock (Lock)
            {
                var resourceList = new ResourceList();
                if (ResourcesGathered >= carrier.MaxResourceLimit)
                {
                    ResourcesGathered -= carrier.MaxResourceLimit;
                    resourceList.Add(typeof(TResource), carrier.MaxResourceLimit);
                }
                else
                {
                    resourceList.Add(typeof(TResource), ResourcesGathered);
                    ResourcesGathered = 0;
                }
                CarriersGoingBackToStorage.Add(carrier);
                carrier.Load(resourceList);
            }
        }

        public void CarrierArrivedAtStorage(Carrier carrier)
        {
            lock (Lock)
            {
                CarriersGoingBackToStorage.Remove(carrier);
            }
        }

        public void UpgradeCarrier(int newCarrierResourceLimit)
        {
            lock (Lock)
            {
                CarriersGoingBackToStorage.ForEach(x => x.Upgrade(newCarrierResourceLimit));
            }
        }
    }
}