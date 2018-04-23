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
    public class CharacterService : ServiceBase, ICharacterServiceAPI
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
            outboundServerPeerLogic = OutboundServerPeer.CreateOutboundServerPeerLogic<CharacterOperations, EmptyEventCode>();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with CharacterService service."));
        }

        public void ChangeCharacterMap(ChangeCharacterMapParameters parameters)
        {
            outboundServerPeerLogic?.SendOperation((byte)CharacterOperations.ChangeCharacterMap, parameters);
        }

        public Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParametersEx parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new CreateCharacterResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<CreateCharacterRequestParametersEx, CreateCharacterResponseParameters>
                (yield, (byte)CharacterOperations.CreateCharacter, parameters);
        }

        public Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParametersEx parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new RemoveCharacterResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<RemoveCharacterRequestParametersEx, RemoveCharacterResponseParameters>
                (yield, (byte)CharacterOperations.RemoveCharacter, parameters);
        }

        public Task<GetCharactersResponseParameters> GetCharacters(IYield yield, GetCharactersRequestParameters parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new GetCharactersResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<GetCharactersRequestParameters, GetCharactersResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacters, parameters);
        }

        public Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParametersEx parameters)
        {
            if (outboundServerPeerLogic == null)
            {
                return Task.FromResult(new GetCharacterResponseParameters());
            }

            return outboundServerPeerLogic.SendOperation<GetCharacterRequestParametersEx, GetCharacterResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacter, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.CharacterService, MessageBuilder.Trace("Could not find a connection info for the Character service."));

            var ip = (string)Config.Global.CharacterService.IP;
            var port = (int)Config.Global.CharacterService.Port;
            return new PeerConnectionInformation(ip, port);
        }

        private string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.CharacterService, MessageBuilder.Trace("Could not find a configuration for the Character service."));

            var secretKey = (string)Config.Global.CharacterService.SecretKey;
            return secretKey;
        }
    }
}