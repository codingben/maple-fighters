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
            var otherDummyComponent = dummyOwner.ExposedComponents.AddAndMock<IOtherDummyComponent>();

            // Act
            dummyOwner.ExposedComponents.Add(new DummyComponent());

            // Assert
            otherDummyComponent.Received().Create();
        }

        [Fact]
        public void OnRemoved_Called()
        {
            // Arrange
            IDummyOwner dummyOwner = new DummyOwner();
            var otherDummyComponent = dummyOwner.ExposedComponents.AddAndMock<IOtherDummyComponent>();

            dummyOwner.ExposedComponents.Add(new DummyComponent());

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

    public interface IDummyOwner
    {
        void Kill();

        IExposedComponents ExposedComponents { get; }
    }

    public class DummyOwner : IDummyOwner
    {
        public IExposedComponents ExposedComponents =>
            Components.ProvideExposed<IDummyOwner>();

        protected IComponents Components { get; }

        public DummyOwner()
        {
            Components = new OwnerComponentsProvider<IDummyOwner>(this);
        }

        public void Kill()
        {
            Components.Remove<DummyComponent>();
        }
    }
}