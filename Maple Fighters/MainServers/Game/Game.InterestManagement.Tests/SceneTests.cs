using CommonTools.Log;
using InterestManagement;
using InterestManagement.Components;
using MathematicsHelper;
using Xunit;

namespace Game.InterestManagement.Tests
{
    public class SceneTests
    {
        public SceneTests()
        {
            LogUtils.Logger = new Logger();
        }

        [Fact]
        private void AddSceneObject_Returns_SceneObject()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);

            // Act
            var sceneObject = scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));

            // Assert
            Assert.NotEqual(null, sceneObject);
        }

        [Fact]
        private void AddSceneObject_Returns_Null()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            scene.AddSceneObject(dummySceneObject);

            // Act
            var sceneObject = scene.AddSceneObject(dummySceneObject);

            // Assert
            Assert.Equal(null, sceneObject);
        }

        [Fact]
        private void RemoveSceneObject_Returns_True()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            var dummySceneObject = scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));

            // Act
            var isRemoved = scene.RemoveSceneObject(dummySceneObject);

            // Assert
            Assert.True(isRemoved);
        }

        [Fact]
        private void RemoveSceneObject_Returns_False()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());

            // Act
            var isRemoved = scene.RemoveSceneObject(dummySceneObject);

            // Assert
            Assert.False(isRemoved);
        }

        [Fact]
        private void RemoveAllSceneObjects_Returns_Zero_SceneObjects()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));
            scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));

            // Act
            scene.Dispose();

            // Assert
            Assert.True(scene.GetAllSceneObjects().Count == 0);
        }

        [Fact]
        private void GetSceneObject_Returns_SceneObject()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));
            scene.AddSceneObject(new SceneObject(1, string.Empty, TransformDetails.Empty()));

            // Act
            var sceneObject = scene.GetSceneObject(0);

            // Assert
            Assert.NotEqual(null, sceneObject);
        }

        [Fact]
        private void GetSceneObject_Returns_Null()
        {
            // Arrange
            var scene = new Scene(Vector2.One, Vector2.One);
            scene.AddSceneObject(new SceneObject(0, string.Empty, TransformDetails.Empty()));
            scene.AddSceneObject(new SceneObject(1, string.Empty, TransformDetails.Empty()));

            // Act
            var sceneObject = scene.GetSceneObject(2);

            // Assert
            Assert.Equal(null, sceneObject);
        }
    }
}