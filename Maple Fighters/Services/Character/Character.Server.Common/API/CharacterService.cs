using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerApplication.Common.Components;

namespace Character.Server.Common
{
    public class CharacterService : ServiceBase<ServerOperations, EmptyEventCode>, ICharacterServiceAPI
    {
        public Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParameters parameters)
        {
            return OutboundServerPeerLogic.SendOperation<GetCharacterRequestParameters, GetCharacterResponseParameters>
                (yield, (byte)ServerOperations.GetCharacter, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.CharactersService, MessageBuilder.Trace("Could not find an connection info for the Characters service."));

            var ip = (string)Config.Global.CharactersService.IP;
            var port = (int)Config.Global.CharactersService.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}