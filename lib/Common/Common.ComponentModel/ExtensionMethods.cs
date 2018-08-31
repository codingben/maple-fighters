using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel
{
    public static class ExtensionMethods
    {
        public static void AddIfNotExists<TComponent>(
            this IList<object> collection,
            TComponent component) where TComponent : class
        {
            if (collection.OfType<TComponent>().Any())
            {
                throw new ComponentModelException(
                    $"Component {typeof(TComponent).Name} already exists!");
            }

            collection.Add(component);
        }
    }
}