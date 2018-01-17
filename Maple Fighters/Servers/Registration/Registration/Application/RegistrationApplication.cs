using Database.Common.Components;
using Registration.Application.Components;
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

            AddCommonComponents();

            Server.Entity.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.AddComponent(new DatabaseUserCreator());
            Server.Entity.AddComponent(new DatabaseUserEmailVerifier());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new RegistrationPeerLogic());
        }
    }
}