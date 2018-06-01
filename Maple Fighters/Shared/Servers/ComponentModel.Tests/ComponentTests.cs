using CommonTools.Log;
using ComponentModel.Common;
using NSubstitute;
using Xunit;

namespace ComponentModel.Tests
{
    public class ComponentTests
    {
        private readonly Container container = new Container();

        [Fact]
        private void AddComponent_Invoke_OnAwake()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var component = new TestComponent(logger);

            // Act
            container.AddComponent(component);

            // Assert
            logger.Received().Log(Arg.Any<string>());
        }

        [Fact]
        private void RemoveComponent_Invoke_OnDestroy()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var component = new TestComponent(logger);
            container.AddComponent(component);

            // Act
            container.RemoveComponent<TestComponent>();

            // Assert
            logger.Received().Log(Arg.Any<string>());
        }
    }
}