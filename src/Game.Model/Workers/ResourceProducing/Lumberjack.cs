using Game.Model.Items.Tools;

namespace Game.Model.Workers.ResourceProducing
{
    public class Lumberjack : ResourceProducingWorker
    {
        public Lumberjack(int level, int output) : base(level, output, typeof(Pickaxe))
        {
        }
    }
}