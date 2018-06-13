using CommonTools.Log;
using InterestManagement;
using InterestManagement.Components;
using MathematicsHelper;
using Xunit;

namespace Game.InterestManagement.Tests
{
    public class RegionTests
    {
        public RegionTests()
        {
            LogUtils.Logger = new Logger();
        }

        [Fact]
        private void AddSubscription_Returns_True()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            dummySceneObject.Components.AddComponent(new NearbySubscribersCollection());

            // Act
            var isSubscribed = region.AddSubscription(dummySceneObject);

            // Assert
            Assert.True(isSubscribed);
        }

        [Fact]
        private void AddSubscription_Returns_False()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            dummySceneObject.Components.AddComponent(new NearbySubscribersCollection());
            region.AddSubscription(dummySceneObject);

            // Act
            var isSubscribed = region.AddSubscription(dummySceneObject);

            // Assert
            Assert.False(isSubscribed);
        }

        [Fact]
        private void RemoveSubscription_Returns_True()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            dummySceneObject.Components.AddComponent(new NearbySubscribersCollection());
            region.AddSubscription(dummySceneObject);

            // Act
            var isSubscribed = region.RemoveSubscription(dummySceneObject);

            // Assert
            Assert.True(isSubscribed);
        }

        [Fact]
        private void RemoveSubscription_Returns_False()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());

            // Act
            var isSubscribed = region.RemoveSubscription(dummySceneObject);

            // Assert
            Assert.False(isSubscribed);
        }

        [Fact]
        private void RemoveSubscriptionForAllSubscribers_Returns_True()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            dummySceneObject.Components.AddComponent(new NearbySubscribersCollection());
            region.AddSubscription(dummySceneObject);

            // Act
            var isSubscribed = region.RemoveSubscriptionForAllSubscribers(dummySceneObject);

            // Assert
            Assert.True(isSubscribed);
        }

        [Fact]
        private void RemoveSubscriptionForAllSubscribers_Returns_False()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());

            // Act
            var isSubscribed = region.RemoveSubscriptionForAllSubscribers(dummySceneObject);

            // Assert
            Assert.False(isSubscribed);
        }

        [Fact]
        private void HasSubscription_Returns_True()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            region.AddSubscription(dummySceneObject);

            // Act
            var isSubscribed = region.HasSubscription(dummySceneObject);

            // Assert
            Assert.True(isSubscribed);
        }

        [Fact]
        private void HasSubscription_Returns_False()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            var dummySceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());

            // Act
            var isSubscribed = region.HasSubscription(dummySceneObject);

            // Assert
            Assert.False(isSubscribed);
        }

        [Fact]
        private void GetSubscribersExcept_Returns_SceneObject()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            region.AddSubscription(new SceneObject(0, string.Empty, TransformDetails.Empty()));
            region.AddSubscription(new SceneObject(1, string.Empty, TransformDetails.Empty()));

            // Act
            var subscribers = region.GetSubscribersExcept(0);

            // Assert
            Assert.True(subscribers.Length == 1);
        }

        [Fact]
        private void GetSubscribersExcept_Returns_Null()
        {
            // Arrange
            var region = new Region(Rectangle.Empty);
            region.AddSubscription(new SceneObject(0, string.Empty, TransformDetails.Empty()));

            // Act
            var subscribers = region.GetSubscribersExcept(0);

            // Assert
            Assert.True(subscribers.Length == 0);
        }
    }
}