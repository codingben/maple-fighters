using InterestManagement;
using InterestManagement.Components;
using InterestManagement.Components.Interfaces;
using Xunit;

namespace Game.InterestManagement.Tests
{
    public class NearbySubscribersCollectionTests
    {
        [Fact]
        private void AddSubscriber_Returns_True()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());

            // Act
            var isSubscriberd = nearbySubscribersCollection.AddSubscriber(new SceneObject(1, string.Empty, TransformDetails.Empty()));

            // Assert
            Assert.True(isSubscriberd);
        }

        [Fact]
        private void AddSubscriber_Returns_False()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            nearbySubscribersCollection.AddSubscriber(sceneObject);

            // Act
            var isSubscriberd = nearbySubscribersCollection.AddSubscriber(sceneObject);

            // Assert
            Assert.False(isSubscriberd);
        }

        [Fact]
        private void RemoveSubscriber_Returns_True()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            nearbySubscribersCollection.AddSubscriber(sceneObject);

            // Act
            var isSubscriberd = nearbySubscribersCollection.RemoveSubscriber(sceneObject);

            // Assert
            Assert.True(isSubscriberd);
        }

        [Fact]
        private void RemoveSubscriber_Returns_False()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());

            // Act
            var isSubscriberd = nearbySubscribersCollection.RemoveSubscriber(sceneObject);

            // Assert
            Assert.False(isSubscriberd);
        }

        [Fact]
        private void AddSubscribers_Returns_True()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            var sceneObjects = new ISceneObject[] 
            { 
                new SceneObject(1, string.Empty, TransformDetails.Empty()), 
                new SceneObject(2, string.Empty, TransformDetails.Empty())
            };

            // Act
            var isSubscriberd = nearbySubscribersCollection.AddSubscribers(sceneObjects);

            // Assert
            Assert.True(isSubscriberd);
        }

        [Fact]
        private void AddSubscribers_Returns_False()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            nearbySubscribersCollection.AddSubscriber(sceneObject);

            var sceneObjects = new ISceneObject[]  { sceneObject };

            // Act
            var isSubscriberd = nearbySubscribersCollection.AddSubscribers(sceneObjects);

            // Assert
            Assert.False(isSubscriberd);
        }

        [Fact]
        private void RemoveSubscribers_Returns_True()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            nearbySubscribersCollection.AddSubscriber(sceneObject);

            var sceneObjects = new ISceneObject[] { sceneObject };

            // Act
            var isSubscriberd = nearbySubscribersCollection.RemoveSubscribers(sceneObjects);

            // Assert
            Assert.True(isSubscriberd);
        }

        [Fact]
        private void RemoveSubscribers_Returns_False()
        {
            // Arrange
            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            var nearbySubscribersCollection = sceneObject.Components.AddComponent(new NearbySubscribersCollection());
            var sceneObjects = new ISceneObject[] { sceneObject };

            // Act
            var isSubscriberd = nearbySubscribersCollection.RemoveSubscribers(sceneObjects);

            // Assert
            Assert.False(isSubscriberd);
        }
    }
}