using System.Collections.Generic;
using System.Linq;
using Game.Model.Buildings.Settings;
using Game.Model.Resources;
using Game.Model.Workers;

namespace Game.Model.Buildings.MainBuildings
{
    public class Storage : Identifiable
    {
        public Storage(int numberOfCarriers, int carrierResourceLimit)
        {
            Carriers = Enumerable.Range(0, numberOfCarriers).Select(x => new Carrier(carrierResourceLimit)).ToList();
            _numberOfCarriers = numberOfCarriers;

            Resources = StartingResources.Resources.Copy();
        }

        public ResourceList Resources { get; private set; }
        public List<Carrier> Carriers { get; }
        private int _numberOfCarriers;

        public List<Carrier> GetCarriers(params Urn[] ids)
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

        public void Upgrade(int newNumberOfCarriers, int newCarrierResourceLimit)
        {
            lock (Lock)
            {
                Carriers.ForEach(x => x.Upgrade(newCarrierResourceLimit));
                Carriers.AddRange(Enumerable.Range(0, newNumberOfCarriers - _numberOfCarriers).Select(x => new Carrier(newCarrierResourceLimit)));
                _numberOfCarriers = newCarrierResourceLimit;
            }
        }
    }
}