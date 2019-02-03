using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Containers;

namespace Scripts.Services
{
    public class CharacterSelectorApi : ICharacterSelectorApi
    {
        public ServerPeerHandler<CharacterOperations, EmptyEventCode> ServerPeer
        {
            get;
        }

        public CharacterSelectorApi()
        {
            ServerPeer =
                new ServerPeerHandler<CharacterOperations, EmptyEventCode>();
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            return 
                await ServerPeer
                    .SendOperation<CreateCharacterRequestParameters, CreateCharacterResponseParameters>(
                        yield,
                        CharacterOperations.CreateCharacter, 
                        parameters, 
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            return 
                await ServerPeer
                    .SendOperation<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>(
                        yield,
                        CharacterOperations.RemoveCharacter, 
                        parameters, 
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield, 
            ValidateCharacterRequestParameters parameters)
        {
            var responseParameters = 
                await ServerPeer
                    .SendOperation<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>(
                        yield,
                        CharacterOperations.ValidateCharacter,
                        parameters,
                        MessageSendOptions.DefaultReliable());

            if (responseParameters.Status == CharacterValidationStatus.Ok)
            {
                ServiceContainer.GameService
                    .SetPeerLogic<GameScenePeerLogic, GameOperations, GameEvents>();
            }

            return responseParameters;
        }

        public async Task<GetCharactersResponseParameters> GetCharactersAsync(
            IYield yield)
        {
            return
                await ServerPeer
                    .SendOperation<EmptyParameters, GetCharactersResponseParameters>(
                        yield,
                        CharacterOperations.GetCharacters,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());
        }
    }
}