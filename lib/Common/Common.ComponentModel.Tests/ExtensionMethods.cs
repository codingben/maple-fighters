using NSubstitute;

namespace Common.ComponentModel.Tests
{
    public static class ExtensionMethods
    {
        public static T AddAndMock<T>(this IComponents components) 
            where T : class
        {
            return (T)components.Add((IComponent)Substitute.For<T>());
        }

        public static T AddAndMock<T>(this IExposedComponents components) 
            where T : class
        {
            return (T)components.Add((IComponent)Substitute.For<T>());
        }
    }
}