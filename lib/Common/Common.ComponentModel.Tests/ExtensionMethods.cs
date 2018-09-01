using NSubstitute;

namespace Common.ComponentModel.Tests
{
    public static class ExtensionMethods
    {
        public static T AddAndMock<T>(this IComponentsProvider components) 
            where T : class
        {
            var component = Substitute.For<T>();
            return components.Add(component);
        }

        public static T AddAndMock<T>(this IExposedComponentsProvider components) 
            where T : class
        {
            var component = Substitute.For<T>();
            return components.Add(component);
        }
    }
}