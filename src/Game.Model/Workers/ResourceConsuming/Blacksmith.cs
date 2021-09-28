using Game.Model.Items.Tools;

namespace Game.Model.Workers.ResourceConsuming
{
    public class Blacksmith : ResourceConsumingWorker
    {
        public Blacksmith(int level, int craftingSpeedReduction) : base(level, craftingSpeedReduction, typeof(Hammer))
        {
        }
    }
}