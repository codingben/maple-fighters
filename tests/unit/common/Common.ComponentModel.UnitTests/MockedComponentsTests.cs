using Common.ComponentModel.Tests;
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

            var dummyCharacter =
                gameObject.ExposedComponents.AddAndMock<DummyCharacter>();
            var transform = gameObject.ExposedComponents.Get<ITransform>();

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
        IExposedComponents ExposedComponents { get; }
    }

    public class GameObject : IGameObject, ISceneObject
    {
        public int Id { get; }

        public IExposedComponents ExposedComponents =>
            Components.ProvideExposed();

        protected IComponents Components { get; }

        public GameObject()
        {
            Components = new ComponentsContainer();
            Components.Add(new Transform());
        }
    }

    public interface ITransform
    {
        void SetPosition();
    }

    [ComponentSettings(ExposedState.Exposable)]
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

    [ComponentSettings(ExposedState.Exposable)]
    public class DummyCharacter : ComponentBase, IDummyCharacter
    {
        public void Move()
        {
            // Left blank intentionally
        }
    }
}