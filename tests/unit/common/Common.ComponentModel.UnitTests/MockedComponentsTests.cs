using NSubstitute;
using Xunit;

namespace Common.ComponentModel.UnitTests
{
    public class MockedComponentsTests
    {
        [Fact]
        private void Mocked_DummyCharacter_Received_Move()
        {
            // Arrange
            IGameObject gameObject = new GameObject();

            var dummyCharacter = gameObject.Components.Get<IDummyCharacter>();
            var transform = gameObject.Components.Get<ITransform>();

            // Act
            transform.SetPosition();

            // Assert
            dummyCharacter.Received().Move();
        }
    }

    public interface ISceneObject
    {
        int Id { get; }
    }

    public interface IGameObject
    {
        IComponents Components { get; }
    }

    public class GameObject : IGameObject, ISceneObject
    {
        public int Id { get; }

        public IComponents Components { get; }

        public GameObject()
        {
            var transform = Substitute.For<Transform>();
            var dummyCharacter = Substitute.For<DummyCharacter>();
            var collection = new IComponent[] { transform, dummyCharacter };

            Components = new ComponentCollection(collection);
        }
    }

    public interface ITransform
    {
        void SetPosition();
    }

    public class Transform : ComponentBase, ITransform
    {
        public void SetPosition()
        {
            var dummyCharacter = Components.Get<IDummyCharacter>();
            dummyCharacter.Move();
        }
    }

    public interface IDummyCharacter
    {
        void Move();
    }

    public class DummyCharacter : ComponentBase, IDummyCharacter
    {
        public void Move()
        {
            // Left blank intentionally
        }
    }
}