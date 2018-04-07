using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using PeerLogic.Common;
using PeerLogic.Common.Components;

namespace ServerCommunication.Common
{
    public class UnauthenticatedServerPeerLogic<TNewPeerLogic> : PeerLogicBase<AuthenticationOperations, EmptyEventCode>
        where TNewPeerLogic : IPeerLogicBase, new()
    {
        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForAuthenticationOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout());
        }

        private void AddHandlerForAuthenticationOperation()
        {
            var secretKey = GetSecretKey();
            OperationHandlerRegister.SetHandler(AuthenticationOperations.Authenticate, new AuthenticationOperationHandler(secretKey, OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized()
        {
            ClientPeerWrapper.SetPeerLogic(new TNewPeerLogic());
        }

        private void OnNonAuthorized()
        {
            var peerId = ClientPeerWrapper.PeerId;
            var ip = ClientPeerWrapper.Peer.ConnectionInformation.Ip;
            var port = ClientPeerWrapper.Peer.ConnectionInformation.Port;

            LogUtils.Log(MessageBuilder.Trace($"An authentication for peer {ip}:{port} with id {peerId} has been failed."));

            ClientPeerWrapper.Peer.Disconnect();
        }

        private string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.SecretKey, MessageBuilder.Trace("Could not find a configured secret key."));

            var secretKey = (string)Config.Global.SecretKey.Key;
            return secretKey;
        }
    }
}