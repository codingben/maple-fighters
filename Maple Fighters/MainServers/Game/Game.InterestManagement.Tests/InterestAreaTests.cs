using System.Linq;
using InterestManagement;
using InterestManagement.Components;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;
using NSubstitute;
using Xunit;

namespace Game.InterestManagement.Tests
{
    public class InterestAreaTests
    {
        [Fact]
        private void SetSize_GetScene_Received()
        {
            // Arrange
            var presenceSceneProvider = Substitute.For<PresenceSceneProvider>(new Scene(Vector2.One, Vector2.One));

            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            sceneObject.Components.RemoveComponent<PresenceSceneProvider>();
            sceneObject.Components.AddComponent(presenceSceneProvider);

            var interestArea = sceneObject.Components.AddComponent(new InterestArea(Vector2.Zero, Vector2.Zero));

            // Act
            interestArea.SetSize();

            // Assert
            presenceSceneProvider.Received().GetScene();
        }

        [Fact]
        private void DetectOverlapsWithRegions_IsIntersect_Returns_True()
        {
            // Arrange
            var scene = new Scene(sceneSize: Vector2.One, regionSize: Vector2.One);
            var presenceSceneProvider = Substitute.For<PresenceSceneProvider>(scene);

            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            sceneObject.Components.RemoveComponent<PresenceSceneProvider>();
            sceneObject.Components.AddComponent(presenceSceneProvider);
            sceneObject.Components.AddComponent(new InterestArea(position: Vector2.Zero, areaSize: Vector2.One));

            // Act
            var transform = sceneObject.Components.GetComponent<IPositionTransform>();
            transform.SetPosition(Vector2.Zero);

            // Assert
            var isSubscribed = scene.GetAllRegions().Cast<IRegion>().Any(region => region.HasSubscription(sceneObject));
            Assert.True(isSubscribed);
        }

        [Fact]
        private void DetectOverlapsWithRegions_IsIntersect_Returns_False()
        {
            // Arrange
            var scene = new Scene(sceneSize: Vector2.One, regionSize: Vector2.One);
            var presenceSceneProvider = Substitute.For<PresenceSceneProvider>(scene);

            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            sceneObject.Components.RemoveComponent<PresenceSceneProvider>();
            sceneObject.Components.AddComponent(presenceSceneProvider);
            sceneObject.Components.AddComponent(new InterestArea(position: Vector2.Zero, areaSize: Vector2.One));

            // Act
            var transform = sceneObject.Components.GetComponent<IPositionTransform>();
            transform.SetPosition(new Vector2(10, 10));

            // Assert
            var isSubscribed = scene.GetAllRegions().Cast<IRegion>().Any(region => region.HasSubscription(sceneObject));
            Assert.False(isSubscribed);
        }

        [Fact]
        private void GetSubscribedPublishers_Returns_Regions()
        {
            // Arrange
            var scene = new Scene(sceneSize: Vector2.One, regionSize: Vector2.One);
            var presenceSceneProvider = Substitute.For<PresenceSceneProvider>(scene);

            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            sceneObject.Components.RemoveComponent<PresenceSceneProvider>();
            sceneObject.Components.AddComponent(presenceSceneProvider);

            var interestArea = sceneObject.Components.AddComponent(new InterestArea(position: Vector2.Zero, areaSize: Vector2.One));

            // Act
            var transform = sceneObject.Components.GetComponent<IPositionTransform>();
            transform.SetPosition(Vector2.Zero);

            // Assert
            var isSubscribed = interestArea.GetSubscribedPublishers().Any();
            Assert.True(isSubscribed);
        }

        [Fact]
        private void GetSubscribedPublishers_Returns_Null()
        {
            // Arrange
            var scene = new Scene(sceneSize: Vector2.One, regionSize: Vector2.One);
            var presenceSceneProvider = Substitute.For<PresenceSceneProvider>(scene);

            var sceneObject = new SceneObject(0, string.Empty, TransformDetails.Empty());
            sceneObject.Components.RemoveComponent<PresenceSceneProvider>();
            sceneObject.Components.AddComponent(presenceSceneProvider);

            var interestArea = sceneObject.Components.AddComponent(new InterestArea(position: Vector2.Zero, areaSize: Vector2.One));

            // Act
            var transform = sceneObject.Components.GetComponent<IPositionTransform>();
            transform.SetPosition(new Vector2(10, 10));

            // Assert
            var isSubscribed = interestArea.GetSubscribedPublishers().Any();
            Assert.False(isSubscribed);
        }
    }
}