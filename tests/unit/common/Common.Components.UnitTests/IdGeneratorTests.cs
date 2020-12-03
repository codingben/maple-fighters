using Common.ComponentModel;
using Shouldly;
using Xunit;

namespace Common.Components.UnitTests
{
    public class IdGeneratorTests
    {
        private readonly IComponents components;

        public IdGeneratorTests()
        {
            var collection = new[] { new IdGenerator() };

            components = new ComponentCollection(collection);
        }

        [Theory]
        [InlineData(1)]
        private void GenerateId(int id)
        {
            // Arrange
            var idGenerator = components.Get<IIdGenerator>();

            // Act
            var generateId = idGenerator.GenerateId();

            // Assert
            id.Equals(generateId).ShouldBeTrue();
        }
    }
}