using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerCommunication.Common;

namespace Authorization.Server.Common
{
    public class AuthorizationService : ServiceBase, IAuthorizationServiceAPI
    {
        private IOutboundServerPeerLogic outboundServerPeerLogic;

        protected override void OnConnectionEstablished()
        {
            base.OnConnectionEstablished();

            var secretKey = GetSecretKey().AssertNotNull(MessageBuilder.Trace("Secret key not found."));
            outboundServerPeerLogic = OutboundServerPeer.CreateCommonServerAuthenticationPeerLogic(secretKey, OnAuthenticated);
        }

        protected override void OnConnectionClosed(DisconnectReason disconnectReason)
        {
            base.OnConnectionClosed(disconnectReason);

            outboundServerPeerLogic.Dispose();
        }

        private void OnAuthenticated()
        {
            outboundServerPeerLogic.Dispose();
            outboundServerPeerLogic = OutboundServerPeer.CreateOutboundServerPeerLogic<AuthorizationOperations, EmptyEventCode>();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with AuthorizationService service."));
        }

        public void RemoveAuthorization(RemoveAuthorizationRequestParameters parameters)
        {
            outboundServerPeerLogic?.SendOperation((byte)AuthorizationOperations.RemoveAuthorization, parameters);
        }

        public Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new AuthorizeAccessTokenResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
                (yield, (byte)AuthorizationOperations.AccessTokenAuthorization, parameters);
        }

        public Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new AuthorizeUserResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
                (yield, (byte)AuthorizationOperations.UserAuthorization, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.AuthorizationService, MessageBuilder.Trace("Could not find a connection info for the Authorization service."));

            var ip = (string)Config.Global.AuthorizationService.IP;
            var port = (int)Config.Global.AuthorizationService.Port;
            return new PeerConnectionInformation(ip, port);
        }

        private string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.AuthorizationService, MessageBuilder.Trace("Could not find a configuration for the Authorization service."));

            var secretKey = (string)Config.Global.AuthorizationService.SecretKey;
            return secretKey;
        }
    }
}