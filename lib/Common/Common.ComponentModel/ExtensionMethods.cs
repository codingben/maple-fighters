using System;
using Common.ComponentModel.Core;

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

            return (ComponentsContainer)components;
        }
    }
}