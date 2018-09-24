using Common.ComponentModel;
using Shouldly;
using Xunit;

namespace Common.Components.UnitTests
{
    public class RandomNumberGeneratorTests
    {
        private readonly IComponentsProvider componentsProvider;

        public RandomNumberGeneratorTests()
        {
            componentsProvider = new ComponentsProvider();
            componentsProvider.Add(new RandomNumberGenerator());
        }

        [Fact]
        private void GenerateRandomNumber()
        {
            // Arrange
            var randomNumberGenerator = componentsProvider.Get<IRandomNumberGenerator>();

            // Act
            var number = randomNumberGenerator.GenerateRandomNumber();

            // Assert
            number.Equals(0).ShouldBeFalse();
        }

        [Theory]
        [InlineData(100, 200)]
        [InlineData(1000, 2000)]
        private void GenerateRandomNumber_WithParameters(int min, int max)
        {
            // Arrange
            var randomNumberGenerator = componentsProvider.Get<IRandomNumberGenerator>();

            // Act
            var number = randomNumberGenerator.GenerateRandomNumber(min, max);

            // Assert
            Assert.InRange(number, min, max);
        }
    }
}