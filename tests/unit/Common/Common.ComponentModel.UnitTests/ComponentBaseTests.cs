using Common.ComponentModel.Tests;
using Common.UnitTestsBase;
using NSubstitute;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class ComponentBaseTests
    {
        [Fact]
        public void OnAwake_Called()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            var otherDummyComponent = componentsProvider.AddAndMock<IOtherDummyComponent>();

            // Act
            componentsProvider.Add(new DummyComponent());

            // Assert
            otherDummyComponent.Received().Create();
        }

        [Fact]
        public void OnRemoved_Called()
        {
            // Arrange
            IComponentsProvider componentsProvider = new ComponentsProvider();
            var otherDummyComponent = componentsProvider.AddAndMock<IOtherDummyComponent>();

            componentsProvider.Add(new DummyComponent());

            // Act
            componentsProvider.Remove<DummyComponent>();

            // Assert
            otherDummyComponent.Received().Destroy();
        }
    }

    public interface IOtherDummyComponent
    {
        void Create();

        void Destroy();
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class DummyComponent : ComponentBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            var otherDummyComponent = Components.Get<IOtherDummyComponent>().AssertNotNull();
            otherDummyComponent.Create();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            var otherDummyComponent = Components.Get<IOtherDummyComponent>().AssertNotNull();
            otherDummyComponent.Destroy();
        }
    }
}