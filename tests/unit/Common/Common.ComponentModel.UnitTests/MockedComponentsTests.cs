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
            ISceneObject sceneObject = new SceneObject();

            var dummyCharacter = sceneObject.ExposableComponents.AddAndMock<IDummyCharacter>();
            var transform = sceneObject.ExposableComponents.Get<ITransform>();

            // Act
            transform.SetPosition();

            // Assert
            dummyCharacter.Received().Move();
        }
    }

    public interface ISceneObject
    {
        IExposableComponentsContainer ExposableComponents { get; }
    }

    public interface IGameObject
    {
        int Id { get; }
    }

    public class SceneObject : ISceneObject, IGameObject
    {
        public int Id { get; }
        public IExposableComponentsContainer ExposableComponents =>
            (ComponentsContainer<IGameObject>)Components;

        protected IComponentsContainer Components { get; }

        public SceneObject()
        {
            Components = new ComponentsContainer<IGameObject>(this);
            Components.Add(new Transform());
        }
    }

    public interface ITransform
    {
        void SetPosition();
    }

    [ComponentSettings(ExposedState.Exposable)]
    public class Transform : ComponentBase<IGameObject>, ITransform
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