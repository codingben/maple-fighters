using System;
using Xunit;

namespace MathematicsHelper.Tests
{
    public class Vector2Tests
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(100, 200)]
        private void Normalize(int x, int y)
        {
            // Arrange
            var vector = new Vector2(x, y);
            var result = new Vector2(0.4472136f, 0.8944272f);

            // Act
            vector = vector.Normalize();

            // Assert
            Assert.True(vector.Equals(result));
        }

        [Fact]
        private void Distance()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(2, 2);
            const float EXPECTED = 1.414214f;

            // Act
            var distance = Vector2.Distance(point1, point2);

            // Assert
            Assert.True(Math.Abs(distance - EXPECTED) < 0.001);
        }

        [Fact]
        private void Distance_Out_Result()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(2, 2);
            const float EXPECTED = 1.414214f;

            // Act
            Vector2.Distance(ref point1, ref point2, out var distance);

            // Assert
            Assert.True(Math.Abs(distance - EXPECTED) < 0.001);
        }

        [Fact]
        private void Max()
        {
            // Arrange
            var min = new Vector2(1, 1);
            var max = new Vector2(2, 2);

            // Act
            var point = Vector2.Max(min, max);

            // Assert
            Assert.True(point.Equals(max));
        }

        [Fact]
        private void Max_Out_Result()
        {
            // Arrange
            var min = new Vector2(1, 1);
            var max = new Vector2(2, 2);

            // Act
            Vector2.Max(ref min, ref max, out var point);

            // Assert
            Assert.True(point.Equals(max));
        }

        [Fact]
        private void Min()
        {
            // Arrange
            var min = new Vector2(1, 1);
            var max = new Vector2(2, 2);

            // Act
            var point = Vector2.Min(min, max);

            // Assert
            Assert.True(point.Equals(min));
        }

        [Fact]
        private void Min_Out_Result()
        {
            // Arrange
            var min = new Vector2(1, 1);
            var max = new Vector2(2, 2);

            // Act
            Vector2.Min(ref min, ref max, out var point);

            // Assert
            Assert.True(point.Equals(min));
        }

        [Fact]
        private void Equality_With_Operator_Returns_True()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(1, 1);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        private void Equality_With_Operator_Returns_False()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(2, 2);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        private void Addition_With_Operator()
        {
            // Arrange
            var point1 = new Vector2(0.5f, 0.5f);
            var point2 = new Vector2(0.5f, 0.5f);

            // Act
            var result = point2 + point1;

            // Assert
            Assert.Equal(Vector2.One, result);
        }

        [Fact]
        private void Subtract_With_Operator()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(2, 2);

            // Act
            var result = point2 - point1;

            // Assert
            Assert.Equal(Vector2.One, result);
        }

        [Fact]
        private void Multiply_With_Operator()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(1, 1);

            // Act
            var result = point1 * point2;

            // Assert
            Assert.Equal(Vector2.One, result);
        }

        [Fact]
        private void Multiply_With_ScaleFactor_Operator()
        {
            // Arrange
            var point1 = new Vector2(1, 1);

            // Act
            var result1 = (point1 * 1);
            var result2 = (1 * point1);

            // Assert
            Assert.Equal(Vector2.One, result1);
            Assert.Equal(Vector2.One, result2);
        }

        [Fact]
        private void Divide_With_Operator()
        {
            // Arrange
            var point1 = new Vector2(1, 1);
            var point2 = new Vector2(1, 1);

            // Act
            var result1 = (point1 / point2);
            var result2 = (point1 / 1);

            // Assert
            Assert.Equal(Vector2.One, result1);
            Assert.Equal(Vector2.One, result2);
        }
    }
}