using NSubstitute;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentCollectionTests
    {
        [Fact]
        public void Get_Should_Return_Singleton_Component()
        {
            // Arrange
            var singletonComponent = Substitute.For<SingletonComponent>();
            var collection = new IComponent[] { singletonComponent };
            var components = new ComponentCollection(collection);

            // Act
            var someComponent = components.Get<ISingletonComponent>();

            // Assert
            someComponent.ShouldBeSameAs(singletonComponent);
        }
    }

    public interface ISingletonComponent
    {
        // Left blank intentionally
    }

    public class SingletonComponent : IComponent, ISingletonComponent
    {
        public void Awake(IComponents components)
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}