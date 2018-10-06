using Common.ComponentModel.Exceptions;
using Common.ComponentModel.Tests;
using Common.UnitTestsBase;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentsProviderTests
    {
        [Fact]
        public void Add_Returns_UnexposableComponent()
        {
            // Arrange
            IComponents components = new ComponentsProvider();

            // Act
            var component = components.Add(new UnexposableComponent());

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void AddAndExpose_Returns_ExposableComponent()
        {
            // Arrange
            IExposedComponents components = new ComponentsProvider();

            // Act
            var component = components.Add(new ExposableComponent());

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void Add_Throws_Exception_When_Two_Same_Components_Added()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new UnexposableComponent());

            // Act & Assert
            Should.Throw<ComponentAlreadyExistsException<UnexposableComponent>>(
                () => components.Add(new UnexposableComponent()));
        }

        [Fact]
        public void AddAndExpose_Throws_Exception_When_Two_Same_Components_Added()
        {
            // Arrange
            IExposedComponents components = new ComponentsProvider();
            components.Add(new ExposableComponent());

            // Act & Assert
            Should.Throw<ComponentAlreadyExistsException<ExposableComponent>>(
                () => components.Add(new ExposableComponent()));
        }

        [Fact]
        public void Remove_Exposable_Component()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new ExposableComponent());

            // Act
            components.Remove<ExposableComponent>();

            // Assert
            components.Get<IExposableComponent>().ShouldBeNull();
        }

        [Fact]
        public void Remove_Unexposable_Component()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new UnexposableComponent());

            // Act
            components.Remove<UnexposableComponent>();

            // Assert
            components.Get<IUnexposableComponent>().ShouldBeNull();
        }

        [Fact]
        public void Get_Returns_Exposable_Component()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new ExposableComponent());

            // Act
            var exposableComponent = components.Get<IExposableComponent>();

            // Assert
            exposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Returns_Exposable_Only_Component()
        {
            // Arrange
            IExposedComponents components = new ComponentsProvider();
            components.Add(new ExposableComponent());

            // Act
            var exposableComponent = components.Get<IExposableComponent>();

            // Assert
            exposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Returns_Unexposable_Component()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new UnexposableComponent());

            // Act
            var unexposableComponent = components.Get<IUnexposableComponent>();

            // Assert
            unexposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Throws_Error_When_Not_Interface()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            components.Add(new UnexposableComponent());

            // Act && Assert
            Should.Throw<InterfaceExpectedException<UnexposableComponent>>(() => 
                components.Get<UnexposableComponent>());
        }

        [Fact]
        public void Dispose_Components()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            var otherTestableComponent = components.AddAndMock<IOtherTestableComponent>();
            components.Add(new TestableComponent());

            // Act
            components.Dispose();

            // Assert
            otherTestableComponent.Received().Test();
        }
    }

    public interface IUnexposableComponent
    {
        // Left blank intentionally
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class UnexposableComponent : ComponentBase, IUnexposableComponent
    {
        // Left blank intentionally
    }

    public interface IExposableComponent
    {
        // Left blank intentionally
    }

    [ComponentSettings(ExposedState.Exposable)]
    public class ExposableComponent : ComponentBase, IExposableComponent
    {
        // Left blank intentionally
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class TestableComponent : ComponentBase
    {
        protected override void OnRemoved()
        {
            base.OnRemoved();

            var otherTestableComponent = Components.Get<IOtherTestableComponent>().AssertNotNull();
            otherTestableComponent.Test();
        }
    }

    public interface IOtherTestableComponent
    {
        void Test();
    }
}