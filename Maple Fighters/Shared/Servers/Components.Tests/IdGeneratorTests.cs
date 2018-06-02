using ComponentModel.Common;
using Components.Common;
using Components.Common.Interfaces;
using Xunit;

namespace Components.Tests
{
    public class IdGeneratorTests
    {
        private readonly IContainer container = new Container();

        public IdGeneratorTests()
        {
            container.AddComponent(new IdGenerator());
        }

        [Theory]
        [InlineData(1)]
        private void GenerateId(int id)
        {
            // Arrange
            var idGenerator = container.GetComponent<IIdGenerator>();

            // Act
            var currentId = idGenerator.GenerateId();

            // Assert
            Assert.Equal(id, currentId);
        }
    }
}