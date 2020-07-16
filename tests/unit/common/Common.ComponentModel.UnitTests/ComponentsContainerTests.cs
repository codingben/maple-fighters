using System;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentsContainerTests
    {
        [Fact]
        public void Get_Should_Return_Singleton_Component()
        {
            // Arrange
            IComponents components = new ComponentsContainer();

            var component = new SingletonComponent();
            components.Add(component);

            // Act
            var someComponent = components.Get<ISingletonComponent>();

            // Assert
            someComponent.ShouldBeSameAs(component);
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

    public class NoAttributeComponent : IComponent
    {
        public void Awake(IComponents components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}