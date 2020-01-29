using System;

namespace Common.ComponentModel
{
    public static class ExtensionMethods
    {
        public static IExposedComponents ProvideExposed(this IComponents components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            return (ComponentsProvider)components;
        }
    }
}