using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model.Buildings.Settings;
using Game.Model.Resources;
using Game.Model.Workers;

namespace Game.Model.Buildings.Settlement
{
    public class Storage
    {
        public Storage(int numberOfCarriers, int carrierResourceLimit)
        {
            Carriers = Enumerable.Range(0, numberOfCarriers).Select(x => new Carrier(carrierResourceLimit)).ToList();

            if (!StartingResources.Apply) return;
            Food.Quantity = StartingResources.Food.Quantity;
            Lumber.Quantity = StartingResources.Lumber.Quantity;
            Stone.Quantity = StartingResources.Stone.Quantity;
        }

        public Food Food { get; set; } = new Food();
        public Copper Copper { get; set; } = new Copper();
        public Stone Stone { get; set; } = new Stone();
        public Lumber Lumber { get; set; } = new Lumber();

        public object Lock { get; } = new object();

        public List<Carrier> Carriers { get; }

        public List<Carrier> GetCarriers(params Guid[] ids)
        {
            lock (Lock) {
                var carriers = Carriers.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var carrier in carriers)
                {
                    Carriers.Remove(carrier);
                }

                return carriers;
            }
        }

        public void UnloadCarrier(Carrier carrier)
        {
            lock (Lock)
            {
                var storageResource = GetResource(carrier.Resource);
                var resourceQuantity = carrier.Unload();
                storageResource.Quantity += resourceQuantity;
                Carriers.Add(carrier);
            }
        }

        public bool CanAfford(List<Resource> resources)
        {
            lock (Lock)
            {
                foreach (var resource in resources)
                {
                    var storageResource = GetResource(resource);
                    if (storageResource.Quantity < resource.Quantity) return false;
                }

                return true;
            }
        }

        public bool Consume(Resource resource)
        {
            lock (Lock)
            {
                var storageResource = GetResource(resource);
                if (storageResource.Quantity < resource.Quantity) return false;
                storageResource.Quantity -= resource.Quantity;
                return true;
            }
        }

        public bool Consume(List<Resource> resources)
        {
            if (!CanAfford(resources)) return false;
            lock (Lock)
            {
                foreach (var resource in resources)
                {
                    var storageResource = GetResource(resource);
                    if (storageResource.Quantity < resource.Quantity) return false;
                    storageResource.Quantity -= resource.Quantity;
                }
                return true;
            }
        }

        private Resource GetResource(Resource resource)
        {
            if (resource.GetType() == typeof(Stone)) return Stone;
            if (resource.GetType() == typeof(Copper)) return Copper;
            if (resource.GetType() == typeof(Lumber)) return Lumber;
            if (resource.GetType() == typeof(Food)) return Food;
            return null;
        }
    }
}