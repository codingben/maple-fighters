using ComponentModel.Common;
using Components.Common;
using Components.Common.Interfaces;
using Xunit;

namespace Components.Tests
{
    public class RandomNumberGeneratorTests
    {
        private readonly IContainer container = new Container();

        public RandomNumberGeneratorTests()
        {
            container.AddComponent(new RandomNumberGenerator());
        }

        [Fact]
        private void GenerateRandomNumber()
        {
            // Arrange
            var randomNumberGenerator = container.GetComponent<IRandomNumberGenerator>();

            // Act
            var number = randomNumberGenerator.GenerateRandomNumber();

            // Assert
            Assert.NotEqual(0, number);
        }

        [Theory]
        [InlineData(100, 200)]
        [InlineData(1000, 2000)]
        private void GenerateRandomNumber_WithParameters(int min, int max)
        {
            // Arrange
            var randomNumberGenerator = container.GetComponent<IRandomNumberGenerator>();

            // Act
            var number = randomNumberGenerator.GenerateRandomNumber(min, max);

            // Assert
            Assert.InRange(number, min, max);
        }
    }
}