using Common.ComponentModel;
using Common.UnitTestsBase;
using Shouldly;
using Xunit;

namespace Common.Components.UnitTests
{
    public class IdGeneratorTests
    {
        private readonly IComponents components;

        public IdGeneratorTests()
        {
            components = new ComponentsProvider();
            components.Add(new IdGenerator());
        }

        [Theory]
        [InlineData(1)]
        private void GenerateId(int id)
        {
            // Arrange
            var idGenerator = components.Get<IIdGenerator>().AssertNotNull();

            // Act
            var generateId = idGenerator.GenerateId();

            // Assert
            id.Equals(generateId).ShouldBeTrue();
        }
    }
}