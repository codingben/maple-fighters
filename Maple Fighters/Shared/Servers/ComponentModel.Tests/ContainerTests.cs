using CommonTools.Log;
using ComponentModel.Common;
using NSubstitute;
using Xunit;

namespace ComponentModel.Tests
{
    public class ContainerTests
    {
        [Fact]
        private void AddComponent_Returns_Component()
        {
            // Arrange
            var container = new Container();

            // Act
            var component = container.AddComponent(new TestComponent(Substitute.For<ILogger>()));

            // Assert
            Assert.NotNull(component);
        }

        [Fact]
        private void RemoveComponent()
        {
            // Arrange
            var container = new Container();
            container.AddComponent(new TestComponent(Substitute.For<ILogger>()));

            // Act
            container.RemoveComponent<TestComponent>();

            // Assert
            Assert.True(container.Count() == 0);
        }

        [Fact]
        private void GetComponent_Returns_Component()
        {
            // Arrange
            var container = new Container();
            container.AddComponent(new TestComponent(Substitute.For<ILogger>()));

            // Act
            var component = container.GetComponent<ITestComponent>();

            // Assert
            Assert.NotNull(component);
        }

        [Fact]
        private void RemoveAllComponents()
        {
            // Arrange
            var container = new Container();
            container.AddComponent(new TestComponent(Substitute.For<ILogger>()));

            // Act
            container.Dispose();

            // Assert
            Assert.True(container.Count() == 0);
        }
    }
}