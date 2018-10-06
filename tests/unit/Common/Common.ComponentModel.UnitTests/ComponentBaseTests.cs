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
            IComponents components = new ComponentsProvider();
            var otherDummyComponent =
                components.AddAndMock<IOtherDummyComponent>();

            // Act
            components.Add(new DummyComponent());

            // Assert
            otherDummyComponent.Received().Create();
        }

        [Fact]
        public void OnRemoved_Called()
        {
            // Arrange
            IComponents components = new ComponentsProvider();
            var otherDummyComponent =
                components.AddAndMock<IOtherDummyComponent>();

            components.Add(new DummyComponent());

            // Act
            components.Remove<DummyComponent>();

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

            var otherDummyComponent = Components.Get<IOtherDummyComponent>()
                .AssertNotNull();
            otherDummyComponent.Create();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            var otherDummyComponent = Components.Get<IOtherDummyComponent>()
                .AssertNotNull();
            otherDummyComponent.Destroy();
        }
    }
}