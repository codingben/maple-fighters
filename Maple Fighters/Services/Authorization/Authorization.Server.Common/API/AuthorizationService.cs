using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerApplication.Common.Components;

namespace Authorization.Server.Common
{
    public class AuthorizationService : ServiceBase<ServerOperations, EmptyEventCode>, IAuthorizationServiceAPI
    {
        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.Authorization, MessageBuilder.Trace("Could not find an connection info for the Authorization service."));

            var ip = (string)Config.Global.Authorization.IP;
            var port = (int)Config.Global.Authorization.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}