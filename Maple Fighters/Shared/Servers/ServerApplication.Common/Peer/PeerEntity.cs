using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public class PeerEntity : IPeerEntity
    {
        public IContainer<IPeerEntity> Container { get; }

        public PeerEntity()
        {
            Container = new Container<IPeerEntity>(this);
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
        }
    }
}