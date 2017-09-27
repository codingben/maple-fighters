using Database.Common.Components;
using Registration.Application.PeerLogic;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Registration.Application
{
    public class RegistrationApplication : ApplicationBase
    {
        public RegistrationApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            Server.Entity.Container.AddComponent(new DatabaseConnectionProvider());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new RegistrationPeerLogic());
        }
    }
}