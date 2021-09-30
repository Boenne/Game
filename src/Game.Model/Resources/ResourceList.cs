using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model.Resources
{
    public class ResourceList : Dictionary<Type, int>
    {
        public ResourceList(params Resource[] resources)
        {
            foreach (var resource in resources)
            {
                var type = resource.GetType();
                if (!type.IsSubclassOf(typeof(Resource)))
                    throw new ArgumentOutOfRangeException();
                this[type] = resource.Quantity;
            }
        }

        public static bool operator <(ResourceList lhs, ResourceList rhs)
        {
            return !(lhs > rhs);
        }

        public static bool operator >(ResourceList lhs, ResourceList rhs)
        {
            foreach (var rhsKey in rhs.Keys)
            {
                if (!lhs.ContainsKey(rhsKey)) return false;
                if (lhs[rhsKey] < rhs[rhsKey]) return false;
            }
            return true;
        }

        public static ResourceList operator -(ResourceList lhs, ResourceList rhs)
        {
            if (lhs < rhs)
                throw new ArgumentOutOfRangeException();

            var resourceList = new ResourceList();
            foreach (var rhsKey in rhs.Keys)
            {
                resourceList.Add(rhsKey, lhs[rhsKey] - rhs[rhsKey]);
            }

            return resourceList;
        }

        public static ResourceList operator +(ResourceList lhs, ResourceList rhs)
        {
            if (rhs.Keys.Any(x => !lhs.ContainsKey(x)))
                throw new ArgumentOutOfRangeException();

            var resourceList = new ResourceList();
            foreach (var rhsKey in rhs.Keys)
            {
                resourceList.Add(rhsKey, lhs[rhsKey] + rhs[rhsKey]);
            }

            return resourceList;
        }

        public ResourceList Copy()
        {
            var resourceList = new ResourceList();
            foreach (var key in Keys)
            {
                resourceList[key] = this[key];
            }

            return resourceList;
        }

        public static ResourceList CreateDefault()
        {
            return new ResourceList
            {
                {typeof(Food), 0},
                {typeof(Stone), 0},
                {typeof(Lumber), 0},
                {typeof(Copper), 0}
            };
        }
    }
}