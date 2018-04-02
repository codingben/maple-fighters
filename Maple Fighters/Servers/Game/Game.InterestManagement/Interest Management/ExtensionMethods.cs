using System.Collections.Generic;
using System.Linq;
using InterestManagement.Components.Interfaces;

namespace InterestManagement
{
    public static class ExtensionMethods
    {
        public static IEnumerable<ISceneObject> GetSubscribersFromAllPublishers(this IRegion[,] regions)
        {
            return regions.Cast<IRegion>().SelectMany(region => region.GetAllSubscribers()).ToList();
        }
    }
}