using Game.Model.Items.Tools;

namespace Game.Model.Workers.ResourceProducing
{
    public class Farmer : ResourceProducingWorker
    {
        public Farmer(int level, int output) : base(level, output, typeof(Rake))
        {
        }
    }
}