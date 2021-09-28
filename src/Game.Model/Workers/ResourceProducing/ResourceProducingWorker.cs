using System;

namespace Game.Model.Workers.ResourceProducing
{
    public class ResourceProducingWorker : Worker
    {
        public ResourceProducingWorker(int level, int output, Type toolType) : base(level, toolType)
        {
            Output = output;
        }

        public int Output { get; }

        public int TotalOutput()
        {
            return Output + (HasTool() ? Tool.Modifier : 0);
        }
    }
}