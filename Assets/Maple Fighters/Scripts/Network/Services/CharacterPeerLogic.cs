using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Coroutines;

namespace Scripts.Services
{
    public sealed class CharacterPeerLogic : PeerLogicBase, ICharacterPeerLogicAPI
    {
        public UnityEvent<GetCharactersResponseParameters> ReceivedCharacters { get; } = new UnityEvent<GetCharactersResponseParameters>();
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        protected override void OnAwake()
        {
            base.OnAwake();

            coroutinesExecutor.ExecuteExternally();
            coroutinesExecutor.StartTask(GetCharacters);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ReceivedCharacters.RemoveAllListeners();
            coroutinesExecutor.RemoveFromExternalExecutor();
        }

        public async Task GetCharacters(IYield yield)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<EmptyParameters, GetCharactersResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacters, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            if (responseParameters.Characters == null)
            {
                LogUtils.Log(MessageBuilder.Trace("Failed to get characters."));
                return;
            }

            ReceivedCharacters?.Invoke(responseParameters);
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<CreateCharacterRequestParameters, CreateCharacterResponseParameters>
                (yield, (byte)CharacterOperations.CreateCharacter, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>
                (yield, (byte)CharacterOperations.RemoveCharacter, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<CharacterValidationStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>
                (yield, (byte)CharacterOperations.ValidateCharacter, parameters, MessageSendOptions.DefaultReliable());
            if (responseParameters.Status == CharacterValidationStatus.Ok)
            {
                ServiceContainer.GameService.SetPeerLogic<GameScenePeerLogic, GameOperations, GameEvents>(new GameScenePeerLogic());
            }
            return responseParameters.Status;
        }
    }
}