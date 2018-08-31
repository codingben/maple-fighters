using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentCollectionsTests
    {
        [Fact]
        public void TryAdd_Should_Not_Throw_Error()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new SingletonComponent());

            // Act
            var component =
                componentCollections.Find<SingletonComponent>(
                    ExposedState.Unexposable);

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void TryAdd_Should_Throw_Error_When_Added_Two_Same_Components()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new SingletonComponent());

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentCollections.TryAdd(new SingletonComponent()));
        }

        [Fact]
        public void TryAdd_Should_Throw_Error_When_No_Attribute()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentCollections.TryAdd(new NoAttributeComponent()));
        }

        [Fact]
        public void TryAddExposed_Throws_Error_When_Not_Exposed()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentCollections.TryAddExposedOnly(new SingletonComponent()));
        }

        [Fact]
        public void Remove_Should_Find_Component()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new SingletonComponent());

            // Act
            componentCollections.Remove<SingletonComponent>();

            // Assert
            var someComponent =
                componentCollections.Find<SingletonComponent>(
                    ExposedState.Unexposable);

            someComponent.ShouldBeNull();
        }

        [Fact]
        public void Remove_Should_Throw_Error_When_Component_Not_Found()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentCollections.Remove<SingletonComponent>());
        }

        [Fact]
        public void Find_Should_Return_Singleton_Component()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();

            var component = new SingletonComponent();
            componentCollections.TryAdd(component);

            // Act
            var someComponent =
                componentCollections.Find<SingletonComponent>(
                    ExposedState.Unexposable);

            // Assert
            someComponent.ShouldBeSameAs(component);
        }

        [Fact]
        public void Find_Should_Return_PerThread_Component()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new PerThreadComponent());

            // Act & Assert
            Should.Throw<NotImplementedException>(
                () => componentCollections.Find<PerThreadComponent>(
                    ExposedState.Unexposable));
        }

        [Fact]
        public void Find_Should_Return_PerCall_Component()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();

            var component = new PerCallComponent();
            componentCollections.TryAdd(component);

            // Act
            var someComponent =
                componentCollections.Find<PerCallComponent>(
                    ExposedState.Unexposable);

            // Assert
            someComponent.ShouldNotBeSameAs(component);
        }

        [Fact]
        public void GetAll_Should_Returns_All_Components()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new SingletonComponent());
            componentCollections.TryAddExposedOnly(new PerThreadComponent());
            componentCollections.TryAdd(new PerCallComponent());

            // Act & Assert
            componentCollections.GetAll().Count().ShouldBeLessThanOrEqualTo(3);
        }

        [Fact]
        public void After_Dispose_Should_Not_Return_Any_Components()
        {
            // Arrange
            IComponentCollections componentCollections = new ComponentCollections();
            componentCollections.TryAdd(new SingletonComponent());
            componentCollections.TryAddExposedOnly(new PerThreadComponent());
            componentCollections.TryAdd(new PerCallComponent());

            // Act
            componentCollections.Dispose();

            // Assert
            componentCollections.GetAll().Count().ShouldBeLessThanOrEqualTo(0);
        }
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class SingletonComponent : IComponent
    {
        public void Awake(IComponentsContainer components)
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
        public void Awake(IComponentsContainer components)
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
        public void Awake(IComponentsContainer components)
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
        public void Awake(IComponentsContainer components)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}