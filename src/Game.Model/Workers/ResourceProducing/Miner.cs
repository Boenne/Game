﻿using Game.Model.Items.Tools;

namespace Game.Model.Workers.ResourceProducing
{
    public class Miner : ResourceProducingWorker
    {
        public Miner(int level, int output) : base(level, output, typeof(Pickaxe))
        {
        }
    }
}