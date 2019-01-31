using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Containers;

namespace Scripts.Services
{
    public sealed class CharacterPeerLogic : PeerLogicBase, ICharacterPeerLogicAPI
    {
        public async Task<GetCharactersResponseParameters> GetCharacters(
            IYield yield)
        {
            return 
                await ServerPeerHandler
                    .SendOperation<EmptyParameters, GetCharactersResponseParameters>(
                        yield, 
                        CharacterOperations.GetCharacters, 
                        new EmptyParameters(), 
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacter(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            return 
                await ServerPeerHandler
                    .SendOperation<CreateCharacterRequestParameters, CreateCharacterResponseParameters>(
                        yield, 
                        CharacterOperations.CreateCharacter, 
                        parameters, 
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacter(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            return 
                await ServerPeerHandler
                    .SendOperation<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>(
                        yield, 
                        CharacterOperations.RemoveCharacter, 
                        parameters, 
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<ValidateCharacterResponseParameters> ValidateCharacter(
            IYield yield, 
            ValidateCharacterRequestParameters parameters)
        {
            var responseParameters = 
                await ServerPeerHandler
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
    }
}