using Common.ComponentModel.Generic;
using Common.ComponentModel.Tests;
using Common.UnitTestsBase;
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

            var dummyCharacter = gameObject.ExposedComponents.AddAndMock<IDummyCharacter>();
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
        IExposedComponentsProvider ExposedComponents { get; }
    }

    public class GameObject : IGameObject, ISceneObject
    {
        public int Id { get; }

        public IExposedComponentsProvider ExposedComponents =>
            Components.ProvideExposed<ISceneObject>();

        protected IComponentsProvider Components { get; }

        public GameObject()
        {
            Components = new OwnerComponentsProvider<ISceneObject>(this);
            Components.Add(new Transform());
        }
    }

    public interface ITransform
    {
        void SetPosition();
    }

    [ComponentSettings(ExposedState.Exposable)]
    public class Transform : ComponentBase<ISceneObject>, ITransform
    {
        public void SetPosition()
        {
            var dummyCharacter = Components.Get<IDummyCharacter>().AssertNotNull();
            dummyCharacter.Move();
        }
    }

    public interface IDummyCharacter
    {
        void Move();
    }
}