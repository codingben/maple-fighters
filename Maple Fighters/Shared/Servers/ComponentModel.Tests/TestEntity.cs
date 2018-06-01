using ComponentModel.Common;

namespace ComponentModel.Tests
{
    public class TestEntity : ITestEntity
    {
        public IContainer<ITestEntity> Components { get; }

        public TestEntity()
        {
            Components = new Container<ITestEntity>(this);
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}