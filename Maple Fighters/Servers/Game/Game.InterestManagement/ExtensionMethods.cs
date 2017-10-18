using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public static class ExtensionMethods
    {
        public static IEnumerable<ISceneObject> GetSubscribersFromAllPublishers(this IRegion[,] regions)
        {
            return regions.Cast<IRegion>().SelectMany(region => region.GetAllSubscribers()).ToList();
        }
    }
}