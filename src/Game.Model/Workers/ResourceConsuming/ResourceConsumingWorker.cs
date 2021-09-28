using System;

namespace Game.Model.Workers.ResourceConsuming
{
    public class ResourceConsumingWorker : Worker
    {
        public ResourceConsumingWorker(int level, int craftingSpeedReduction, Type toolType) : base(level, toolType)
        {
            CraftingSpeedReduction = craftingSpeedReduction;
        }

        public int CraftingSpeedReduction { get; }

        public int TotalCraftingSpeedRecution()
        {
            return CraftingSpeedReduction + (HasTool() ? Tool.Modifier : 0);
        }
    }
}