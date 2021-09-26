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
        public Resource Resource { get; private set; }

        public void Load(Resource resource)
        {
            Resource = resource;
            if (resource.Quantity > MaxResourceLimit)
                Resource.Quantity = MaxResourceLimit;
        }

        public int Unload()
        {
            var quantity = Resource.Quantity;
            Resource = null;
            return quantity;
        }
    }
}