using NSubstitute;

namespace Common.ComponentModel.Tests
{
    public static class ExtensionMethods
    {
        public static T AddAndMock<T>(this IComponentsContainer container) 
            where T : class
        {
            var component = Substitute.For<T>();
            return container.Add(component);
        }

        public static T AddAndMock<T>(this IExposableComponentsContainer container) 
            where T : class
        {
            var component = Substitute.For<T>();
            return container.Add(component);
        }
    }
}