using System;
using Common.ComponentModel.Generic;

namespace Common.ComponentModel
{
    public static class ExtensionMethods
    {
        public static IExposedComponentsProvider ProvideExposed(
            this IComponentsProvider components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            return (ComponentsProvider)components;
        }

        public static IExposedComponentsProvider ProvideExposed<TOwner>(
            this IComponentsProvider components) 
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