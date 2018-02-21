using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerApplication.Common.Components;

namespace Characters.Server.Common
{
    public class CharactersService : ServiceBase<ServerOperations, EmptyEventCode>, ICharactersServiceAPI
    {
        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.CharactersService, MessageBuilder.Trace("Could not find an connection info for the Characters service."));

            var ip = (string)Config.Global.CharactersService.IP;
            var port = (int)Config.Global.CharactersService.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}