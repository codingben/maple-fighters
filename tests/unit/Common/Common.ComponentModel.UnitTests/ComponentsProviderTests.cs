using Common.ComponentModel.Core;
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
            IComponentsProvider componentsProvider = new ComponentsProvider();

            // Act
            var component = componentsProvider.Add(new UnexposableComponent());

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void AddAndExpose_Returns_ExposableComponent()
        {
            // Arrange
            IExposedComponentsProvider componentsProvider = new ComponentsProvider();

            // Act
            var component = componentsProvider.Add(new ExposableComponent());

            // Assert
            component.ShouldNotBeNull();
        }

        [Fact]
        public void Add_Throws_Exception_When_Two_Same_Components_Added()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new UnexposableComponent());

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsProvider.Add(new UnexposableComponent()));
        }

        [Fact]
        public void AddAndExpose_Throws_Exception_When_Two_Same_Components_Added()
        {
            // Arrange
            IExposedComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new ExposableComponent());

            // Act & Assert
            Should.Throw<ComponentModelException>(
                () => componentsProvider.Add(new ExposableComponent()));
        }

        [Fact]
        public void Remove_Exposable_Component()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new ExposableComponent());

            // Act
            componentsProvider.Remove<ExposableComponent>();

            // Assert
            componentsProvider.Get<IExposableComponent>().ShouldBeNull();
        }

        [Fact]
        public void Remove_Unexposable_Component()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new UnexposableComponent());

            // Act
            componentsProvider.Remove<UnexposableComponent>();

            // Assert
            componentsProvider.Get<IUnexposableComponent>().ShouldBeNull();
        }

        [Fact]
        public void Get_Returns_Exposable_Component()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new ExposableComponent());

            // Act
            var exposableComponent = componentsProvider.Get<IExposableComponent>();

            // Assert
            exposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Returns_Exposable_Only_Component()
        {
            // Arrange
            IExposedComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new ExposableComponent());

            // Act
            var exposableComponent = componentsProvider.Get<IExposableComponent>();

            // Assert
            exposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Returns_Unexposable_Component()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new UnexposableComponent());

            // Act
            var unexposableComponent = componentsProvider.Get<IUnexposableComponent>();

            // Assert
            unexposableComponent.ShouldNotBeNull();
        }

        [Fact]
        public void Get_Throws_Error_When_Not_Interface()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new UnexposableComponent());

            // Act && Assert
            Should.Throw<ComponentModelException>(() => 
                componentsProvider.Get<UnexposableComponent>());
        }

        [Fact]
        public void Dispose_Components()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            var otherTestableComponent = componentsProvider.AddAndMock<IOtherTestableComponent>();
            componentsProvider.Add(new TestableComponent());

            // Act
            componentsProvider.Dispose();

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