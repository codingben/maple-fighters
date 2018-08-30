using Common.ComponentModel.Tests;
using Common.UnitTestsBase;
using NSubstitute;
using Xunit;

namespace Common.ComponentModel.Generic.UnitTests
{
    public class ComponentBaseTests
    {
        [Fact]
        public void OnAwake_Called()
        {
            // Arrange
            IDummyOwner dummyOwner = new DummyOwner();
            var otherDummyComponent = dummyOwner.ExposableComponents.AddAndMock<IOtherDummyComponent>();

            // Act
            dummyOwner.ExposableComponents.Add(new DummyComponent());

            // Assert
            otherDummyComponent.Received().Create();
        }

        [Fact]
        public void OnRemoved_Called()
        {
            // Arrange
            IDummyOwner dummyOwner = new DummyOwner();
            var otherDummyComponent = dummyOwner.ExposableComponents.AddAndMock<IOtherDummyComponent>();

            dummyOwner.ExposableComponents.Add(new DummyComponent());

            // Act
            dummyOwner.Kill();

            // Assert
            otherDummyComponent.Received().Destroy();
        }
    }

    public interface IOtherDummyComponent
    {
        void Create();

        void Destroy();
    }

    [ComponentSettings(ExposedState.Exposable)]
    public class DummyComponent : ComponentBase<IDummyOwner>
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

    public interface IDummyOwner
    {
        void Kill();

        IExposableComponentsContainer ExposableComponents { get; }
    }

    public class DummyOwner : IDummyOwner
    {
        public IExposableComponentsContainer ExposableComponents =>
            (ComponentsContainer<IDummyOwner>)Components;

        protected IComponentsContainer Components { get; }

        public DummyOwner()
        {
            Components = new ComponentsContainer<IDummyOwner>(this);
        }

        public void Kill()
        {
            Components.Remove<DummyComponent>();
        }
    }
}