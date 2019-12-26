using System;
using Common.ComponentModel.Generic;

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

        public static IExposedComponents ProvideExposed<TOwner>(this IComponents components) 
            where TOwner : class
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            return (OwnerComponentsProvider<TOwner>)components;
        }
    }
}