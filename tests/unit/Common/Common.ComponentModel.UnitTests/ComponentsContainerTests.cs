using System;
using System.Linq;
using Common.ComponentModel.Exceptions;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentsContainerTests
    {
        [Fact]
        public void Add_Should_Not_Throw_Error()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();
            componentsContainer.Add(new SingletonComponent());

            // Act
            var component = componentsContainer.Get<SingletonComponent>();

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void Add_Should_Throw_Error_When_Added_Two_Same_Components()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();
            componentsContainer.Add(new SingletonComponent());

            // Act & Assert
            Should.Throw<ComponentAlreadyExistsException>(
                () => componentsContainer.Add(new SingletonComponent()));
        }

        [Fact]
        public void Add_Should_Throw_Error_When_No_Attribute()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentSettingsMissingException>(
                () => componentsContainer.Add(new NoAttributeComponent()));
        }

        [Fact]
        public void AddExposed_Throws_Error_When_Not_Exposed()
        {
            // Arrange
            IExposedComponents componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentNotExposedException>(
                () => componentsContainer.Add(new SingletonComponent()));
        }

        [Fact]
        public void Remove_Should_Find_Component()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();
            componentsContainer.Add(new SingletonComponent());

            // Act
            componentsContainer.Remove<SingletonComponent>();

            // Assert
            var someComponent = componentsContainer.Get<SingletonComponent>();
            someComponent.ShouldBeNull();
        }

        [Fact]
        public void Remove_Should_Throw_Error_When_Component_Not_Found()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();

            // Act & Assert
            Should.Throw<ComponentNotFoundException>(
                () => componentsContainer.Remove<SingletonComponent>());
        }

        [Fact]
        public void Find_Should_Return_Singleton_Component()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();

            var component = new SingletonComponent();
            componentsContainer.Add(component);

            // Act
            var someComponent = componentsContainer.Get<SingletonComponent>();

            // Assert
            someComponent.ShouldBeSameAs(component);
        }

        [Fact]
        public void After_Dispose_Should_Not_Return_Any_Components()
        {
            // Arrange
            IComponents componentsContainer = new ComponentsContainer();
            componentsContainer.Add(new SingletonComponent());

            // Act
            componentsContainer.Dispose();

            // Assert
            var someComponent = componentsContainer.Get<SingletonComponent>();
            someComponent.ShouldBeNull();
        }
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class SingletonComponent : IComponent
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