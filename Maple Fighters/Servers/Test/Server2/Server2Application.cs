using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Server2
{
    public class Server2Application : ApplicationBase
    {
        public Server2Application(IFiberProvider fiberProvider, IServerConnector serverConnector)
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            switch (clientPeer.ConnectionInformation.Port)
            {
                case 5060: // UDP // TODO: Do not hard code
                {
                    WrapClientPeer(clientPeer, new ClientPeerLogic());
                    break;
                }
                case 4535: // TCP // TODO: Do not hard code
                {
                    WrapClientPeer(clientPeer, new ServerPeerLogic());
                    break;
                }
                default:
                {
                    LogUtils.Log($"No handler found for peer: {clientPeer.ConnectionInformation.Ip}:{clientPeer.ConnectionInformation.Port}");
                    break;
                }
            }
        }
    }
}