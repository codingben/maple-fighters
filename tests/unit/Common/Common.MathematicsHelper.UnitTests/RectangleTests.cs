using Shouldly;
using Xunit;

namespace Common.MathematicsHelper.UnitTests
{
    public class RectangleTests
    {
        [Theory]
        [InlineData(0, 0, 10, 5)]
        [InlineData(1, 0, 10, 5)]
        [InlineData(0, 1, 10, 5)]
        private void Intersects_Returns_True(
            float x,
            float y,
            float width,
            float height)
        {
            // Arrange
            var rectangle1 = new Rectangle(0, 0, 10, 5);
            var rectangle2 = new Rectangle(x, y, width, height);

            // Act
            var result = rectangle1.Intersects(rectangle2);

            // Assert
            result.ShouldBeTrue();
        }

        [Theory]
        [InlineData(0, 0, 10, 5)]
        [InlineData(1, 0, 10, 5)]
        [InlineData(0, 1, 10, 5)]
        private void Intersects_Returns_False(
            float x,
            float y,
            float width,
            float height)
        {
            // Arrange
            var rectangle1 = new Rectangle(x + width, y + height, 10, 5);
            var rectangle2 = new Rectangle(x, y, width, height);

            // Act
            var result = rectangle1.Intersects(rectangle2);

            // Assert
            result.ShouldBeFalse();
        }
    }
}