using System;
using Shouldly;
using Xunit;

namespace Common.MathematicsHelper.UnitTests
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
            vector.Equals(result).ShouldBeTrue();
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
            (Math.Abs(distance - EXPECTED) < 0.001).ShouldBeTrue();
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
            (Math.Abs(distance - EXPECTED) < 0.001).ShouldBeTrue();
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
            point.Equals(max).ShouldBeTrue();
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
            point.Equals(max).ShouldBeTrue();
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
            point.Equals(min).ShouldBeTrue();
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
            point.Equals(min).ShouldBeTrue();
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
            result.ShouldBeTrue();
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
            result.ShouldBeTrue();
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
            result.ShouldBe(Vector2.One);
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
            result.ShouldBe(Vector2.One);
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
            result.ShouldBe(Vector2.One);
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
            result1.ShouldBe(Vector2.One);
            result2.ShouldBe(Vector2.One);
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
            result1.ShouldBe(Vector2.One);
            result2.ShouldBe(Vector2.One);
        }
    }
}