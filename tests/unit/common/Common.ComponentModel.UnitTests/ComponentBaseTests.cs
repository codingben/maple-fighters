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
            var otherDummyComponent = Substitute.For<OtherDummyComponent>();
            var collection = new IComponent[] { otherDummyComponent };

            // Act
            var components = new ComponentCollection(collection);

            // Assert
            otherDummyComponent.Received().Create();
        }

        [Fact]
        public void OnRemoved_Called()
        {
            // Arrange
            var otherDummyComponent = Substitute.For<OtherDummyComponent>();
            var collection = new IComponent[] { otherDummyComponent };
            var components = new ComponentCollection(collection);

            // Act
            components.Dispose();

            // Assert
            otherDummyComponent.Received().Destroy();
        }
    }

    public interface IOtherDummyComponent
    {
        void Create();

        void Destroy();
    }

    public class OtherDummyComponent : ComponentBase, IOtherDummyComponent
    {
        public void Create()
        {
            // Left blank intentionally
        }

        public void Destroy()
        {
            // Left blank intentionally
        }
    }

    public class DummyComponent : ComponentBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            var otherDummyComponent = Components.Get<IOtherDummyComponent>();
            otherDummyComponent.Create();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            var otherDummyComponent = Components.Get<IOtherDummyComponent>();
            otherDummyComponent.Destroy();
        }
    }
}