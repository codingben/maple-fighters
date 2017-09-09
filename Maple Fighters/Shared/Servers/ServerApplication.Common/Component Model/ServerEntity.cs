using ServerApplication.Common.ApplicationBase;

namespace ServerApplication.Common.ComponentModel
{
    public class ServerEntity : IServerEntity
    {
        public IContainer<IServerEntity> Container { get; }

        public ServerEntity()
        {
            Container = new Container<IServerEntity>(this);
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
        }
    }
}