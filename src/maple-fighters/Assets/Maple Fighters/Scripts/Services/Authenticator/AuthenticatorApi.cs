using CommonCommunicationInterfaces;
using CommunicationHelper;
using Network.Scripts;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorApi : NetworkApi<AuthenticatorOperations, EmptyEventCode>
    {
        protected AuthenticatorApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }
    }
}