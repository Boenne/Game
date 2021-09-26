﻿using Game.Model.Workers;

namespace Game.Model.Buildings.ResourceProducing
{
    public interface IResourceProducingBuilding
    {
        void LoadCarrier(Carrier carrier);
        void CarrierArrivedAtStorage(Carrier carrier);
    }
}