using CommonTools.Log;
using NSubstitute;
using Xunit;

namespace ComponentModel.Tests
{
    public class OwnerContainerTests
    {
        [Fact]
        private void AddComponent_Returns_Component()
        {
            // Arrange
            var owner = new TestEntity();

            // Act
            var component = owner.Components.AddComponent(new OwnerTestComponent(Substitute.For<ILogger>()));

            // Assert
            Assert.NotNull(component);
        }

        [Fact]
        private void RemoveComponent()
        {
            // Arrange
            var owner = new TestEntity();
            owner.Components.AddComponent(new OwnerTestComponent(Substitute.For<ILogger>()));

            // Act
            owner.Components.RemoveComponent<OwnerTestComponent>();

            // Assert
            Assert.True(owner.Components.Count() == 0);
        }

        [Fact]
        private void GetComponent_Returns_Component()
        {
            // Arrange
            var owner = new TestEntity();
            owner.Components.AddComponent(new OwnerTestComponent(Substitute.For<ILogger>()));

            // Act
            var component = owner.Components.GetComponent<ITestComponent>();

            // Assert
            Assert.NotNull(component);
        }

        [Fact]
        private void RemoveAllComponents()
        {
            // Arrange
            var owner = new TestEntity();
            owner.Components.AddComponent(new OwnerTestComponent(Substitute.For<ILogger>()));

            // Act
            owner.Components.Dispose();

            // Assert
            Assert.True(owner.Components.Count() == 0);
        }
    }
}