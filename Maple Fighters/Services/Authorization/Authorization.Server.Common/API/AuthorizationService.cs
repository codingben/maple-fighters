using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerApplication.Common.Components;

namespace Authorization.Server.Common
{
    public class AuthorizationService : ServiceBase<ServerOperations, EmptyEventCode>, IAuthorizationServiceAPI
    {
        public Task<CreateAuthorizationResponseParameters> CreateAuthorization(IYield yield, CreateAuthorizationRequestParameters parameters)
        {
            return OutboundServerPeerLogic.SendOperation<CreateAuthorizationRequestParameters, CreateAuthorizationResponseParameters>
                (yield, (byte)ServerOperations.CreateAuthorization, parameters);
        }

        public Task<EmptyParameters> RemoveAuthorization(IYield yield, RemoveAuthorizationRequestParameters parameters)
        {
            return OutboundServerPeerLogic.SendOperation<RemoveAuthorizationRequestParameters, EmptyParameters>
                (yield, (byte)ServerOperations.RemoveAuthorization, parameters);
        }

        public Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters)
        {
            return OutboundServerPeerLogic.SendOperation<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
                (yield, (byte)ServerOperations.AccessTokenAuthorization, parameters);
        }

        public Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters)
        {
            return OutboundServerPeerLogic.SendOperation<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
                (yield, (byte)ServerOperations.UserAuthorization, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.AuthorizationService, MessageBuilder.Trace("Could not find an connection info for the Authorization service."));

            var ip = (string)Config.Global.AuthorizationService.IP;
            var port = (int)Config.Global.AuthorizationService.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}