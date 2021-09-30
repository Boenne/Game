using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model.Buildings.Settings;
using Game.Model.Resources;
using Game.Model.Workers;

namespace Game.Model.Buildings.Settlement
{
    public class Storage : Identifiable
    {
        public Storage(int numberOfCarriers, int carrierResourceLimit)
        {
            Carriers = Enumerable.Range(0, numberOfCarriers).Select(x => new Carrier(carrierResourceLimit)).ToList();

            Resources = StartingResources.Resources.Copy();
        }

        public ResourceList Resources { get; private set; }
        public List<Carrier> Carriers { get; }

        public List<Carrier> GetCarriers(params Guid[] ids)
        {
            lock (Lock)
            {
                var carriers = Carriers.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var carrier in carriers) Carriers.Remove(carrier);

                return carriers;
            }
        }

        public void UnloadCarrier(Carrier carrier)
        {
            lock (Lock)
            {
                var resources = carrier.Unload();
                Resources += resources;
                Carriers.Add(carrier);
            }
        }

        public bool CanAfford(ResourceList resourcesToConsume)
        {
            lock (Lock)
            {
                return Resources > resourcesToConsume;
            }
        }

        public bool Consume(ResourceList resourcesToConsume)
        {
            if (!CanAfford(resourcesToConsume)) return false;
            lock (Lock)
            {
                Resources -= resourcesToConsume;
                return true;
            }
        }
    }
}