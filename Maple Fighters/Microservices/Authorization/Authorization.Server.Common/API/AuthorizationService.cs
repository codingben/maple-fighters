using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerCommunication.Common;

namespace Authorization.Server.Common
{
    public class AuthorizationService : ServiceBase<AuthorizationOperations, EmptyEventCode>, IAuthorizationServiceAPI
    {
        protected override void OnAuthenticated()
        {
            base.OnAuthenticated();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with Authorization service."));
        }

        public void RemoveAuthorization(RemoveAuthorizationRequestParameters parameters)
        {
            OutboundServerPeerLogic?.SendOperation((byte)AuthorizationOperations.RemoveAuthorization, parameters);
        }

        public Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
                (yield, (byte)AuthorizationOperations.AccessTokenAuthorization, parameters);
        }

        public Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
                (yield, (byte)AuthorizationOperations.UserAuthorization, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.AuthorizationService, MessageBuilder.Trace("Could not find a connection info for the Authorization service."));

            var ip = (string)Config.Global.AuthorizationService.IP;
            var port = (int)Config.Global.AuthorizationService.Port;
            return new PeerConnectionInformation(ip, port);
        }

        protected override string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.AuthorizationService, MessageBuilder.Trace("Could not find a configuration for the Authorization service."));

            var secretKey = (string)Config.Global.AuthorizationService.SecretKey;
            return secretKey;
        }
    }
}