using System;

namespace Game.Model.Resources
{
    public class ResourceSite : Identifiable
    {
        public ResourceSite(Type resourceType, int availableResources)
        {
            if (!resourceType.IsSubclassOf(typeof(Resource))) throw new ArgumentOutOfRangeException(nameof(resourceType));
            ResourceType = resourceType;
            AvailableResources = availableResources;
        }

        public Type ResourceType { get; }
        public int AvailableResources { get; }
    }
}