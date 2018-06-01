using CommonTools.Log;
using NSubstitute;
using Xunit;

namespace ComponentModel.Tests
{
    public class OwnerComponentTests
    {
        private readonly ITestEntity testEntity = new TestEntity();

        [Fact]
        private void AddComponent_Invoke_OnAwake()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var component = new OwnerTestComponent(logger);

            // Act
            testEntity.Components.AddComponent(component);

            // Assert
            logger.Received().Log(Arg.Any<string>());
        }

        [Fact]
        private void RemoveComponent_Invoke_OnDestroy()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var component = new OwnerTestComponent(logger);
            testEntity.Components.AddComponent(component);

            // Act
            testEntity.Components.RemoveComponent<OwnerTestComponent>();

            // Assert
            logger.Received().Log(Arg.Any<string>());
        }
    }
}