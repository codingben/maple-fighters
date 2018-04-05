using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Game.Common;
using JsonConfig;
using ServerCommunication.Common;

namespace Character.Server.Common
{
    public class CharacterService : ServiceBase<CharacterOperations, EmptyEventCode>, ICharacterServiceAPI
    {
        protected override void OnAuthenticated()
        {
            base.OnAuthenticated();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with Character service."));
        }

        public Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParametersEx parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<CreateCharacterRequestParametersEx, CreateCharacterResponseParameters>
                (yield, (byte)CharacterOperations.CreateCharacter, parameters);
        }

        public Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParametersEx parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<RemoveCharacterRequestParametersEx, RemoveCharacterResponseParameters>
                (yield, (byte)CharacterOperations.RemoveCharacter, parameters);
        }

        public Task<GetCharactersResponseParameters> GetCharacters(IYield yield, GetCharactersRequestParameters parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<GetCharactersRequestParameters, GetCharactersResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacters, parameters);
        }

        public Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParametersEx parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<GetCharacterRequestParametersEx, GetCharacterResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacter, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.CharacterService, MessageBuilder.Trace("Could not find a connection info for the Character service."));

            var ip = (string)Config.Global.CharacterService.IP;
            var port = (int)Config.Global.CharacterService.Port;
            return new PeerConnectionInformation(ip, port);
        }

        protected override string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.CharacterService, MessageBuilder.Trace("Could not find a configuration for the Character service."));

            var secretKey = (string)Config.Global.CharacterService.SecretKey;
            return secretKey;
        }
    }
}