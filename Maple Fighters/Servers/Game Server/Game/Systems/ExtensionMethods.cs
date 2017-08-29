using System.Collections.Generic;
using System.Linq;
using Game.Entities;
using Game.InterestManagement;

namespace Game.Systems
{
    public static class ExtensionMethods
    {
        public static IEnumerable<IEntity> ConvertRegionsFromMatrix(this IRegion[,] regions)
        {
            return regions.Cast<IRegion>().SelectMany(region => region.GetAllSubscribers()).ToList();
        }
    }
}