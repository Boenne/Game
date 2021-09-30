using Game.Model.Resources;

namespace Game.Model.Workers
{
    public class Carrier : Identifiable
    {
        public Carrier(int maxResourceLimit)
        {
            MaxResourceLimit = maxResourceLimit;
        }

        public int MaxResourceLimit { get; }
        public ResourceList Resources { get; private set; }

        public void Load(ResourceList resourceList)
        {
            Resources = resourceList;
        }

        public ResourceList Unload()
        {
            var resources = Resources.Copy();
            Resources = null;
            return resources;
        }
    }
}