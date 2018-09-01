using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentsContainerTests
    {
        [Fact]
        public void TryAdd_Should_Not_Throw_Error()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new SingletonComponent());

            // Act
            var component =
                componentsContainer.Find<SingletonComponent>();

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void TryAdd_Should_Throw_Error_When_Added_Two_Same_Components()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new SingletonComponent());

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsContainer.TryAdd(new SingletonComponent()));
        }

        [Fact]
        public void TryAdd_Should_Throw_Error_When_No_Attribute()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsContainer.TryAdd(new NoAttributeComponent()));
        }

        [Fact]
        public void TryAddExposed_Throws_Error_When_Not_Exposed()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsContainer.TryAddExposedOnly(new SingletonComponent()));
        }

        [Fact]
        public void Remove_Should_Find_Component()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new SingletonComponent());

            // Act
            componentsContainer.Remove<SingletonComponent>();

            // Assert
            var someComponent =
                componentsContainer.Find<SingletonComponent>();

            someComponent.ShouldBeNull();
        }

        [Fact]
        public void Remove_Should_Throw_Error_When_Component_Not_Found()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsContainer.Remove<SingletonComponent>());
        }

        [Fact]
        public void Find_Should_Return_Singleton_Component()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();

            var component = new SingletonComponent();
            componentsContainer.TryAdd(component);

            // Act
            var someComponent =
                componentsContainer.Find<SingletonComponent>();

            // Assert
            someComponent.ShouldBeSameAs(component);
        }

        [Fact]
        public void Find_Should_Return_PerThread_Component()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new PerThreadComponent());

            // Act & Assert
            Should.Throw<NotImplementedException>(
                () => componentsContainer.Find<PerThreadComponent>());
        }

        [Fact]
        public void Find_Should_Return_PerCall_Component()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();

            var component = new PerCallComponent();
            componentsContainer.TryAdd(component);

            // Act
            var someComponent =
                componentsContainer.Find<PerCallComponent>();

            // Assert
            someComponent.ShouldNotBeSameAs(component);
        }

        [Fact]
        public void GetAll_Should_Returns_All_Components()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new SingletonComponent());
            componentsContainer.TryAddExposedOnly(new PerThreadComponent());
            componentsContainer.TryAdd(new PerCallComponent());

            // Act & Assert
            componentsContainer.GetAll().Count().ShouldBeLessThanOrEqualTo(3);
        }

        [Fact]
        public void After_Dispose_Should_Not_Return_Any_Components()
        {
            // Arrange
            IComponentsContainer componentsContainer = new ComponentsContainer();
            componentsContainer.TryAdd(new SingletonComponent());
            componentsContainer.TryAddExposedOnly(new PerThreadComponent());
            componentsContainer.TryAdd(new PerCallComponent());

            // Act
            componentsContainer.Dispose();

            // Assert
            componentsContainer.GetAll().Count().ShouldBeLessThanOrEqualTo(0);
        }
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class SingletonComponent : IComponent
    {
        public void Awake(IComponentsProvider components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [ComponentSettings(ExposedState.Exposable, LifeTime.PerThread)]
    public class PerThreadComponent : IComponent
    {
        public void Awake(IComponentsProvider components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [ComponentSettings(ExposedState.Unexposable, LifeTime.PerCall)]
    public class PerCallComponent : IComponent
    {
        public void Awake(IComponentsProvider components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class NoAttributeComponent : IComponent
    {
        public void Awake(IComponentsProvider components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}