using ComponentModel.Common;

namespace ServerApplication.Common.ApplicationBase
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
            Container.Dispose();
        }
    }
}