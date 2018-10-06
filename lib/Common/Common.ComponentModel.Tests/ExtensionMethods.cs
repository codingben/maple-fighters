using NSubstitute;

namespace Common.ComponentModel.Tests
{
    public static class ExtensionMethods
    {
        public static T AddAndMock<T>(this IComponents components) 
            where T : class
        {
            var component = Substitute.For<T>();
            return components.Add(component);
        }

        public static T AddAndMock<T>(this IExposedComponents components) 
            where T : class
        {
            var component = Substitute.For<T>();
            return components.Add(component);
        }
    }
}