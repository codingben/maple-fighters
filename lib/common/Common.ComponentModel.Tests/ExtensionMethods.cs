using NSubstitute;

namespace Common.ComponentModel.Tests
{
    public static class ExtensionMethods
    {
        public static T AddAndMock<T>(this IComponents components) 
            where T : class, IComponent
        {
            return components.Add(Substitute.For<T>());
        }

        public static T AddAndMock<T>(this IExposedComponents components) 
            where T : class, IComponent
        {
            return components.Add(Substitute.For<T>());
        }
    }
}