using Common.ComponentModel.Generic;

namespace Common.ComponentModel
{
    public static class ExtensionMethods
    {
        public static IExposedComponentsProvider ProvideExposed(
            this IComponentsProvider components)
        {
            return (ComponentsProvider)components;
        }

        public static IExposedComponentsProvider ProvideExposed<TOwner>(
            this IComponentsProvider components) where TOwner : class
        {
            return (OwnerComponentsProvider<TOwner>)components;
        }
    }
}