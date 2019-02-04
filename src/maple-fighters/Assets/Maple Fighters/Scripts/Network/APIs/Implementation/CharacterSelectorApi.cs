using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;

namespace Scripts.Services
{
    public class CharacterSelectorApi : ApiBase<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        public async Task<CreateCharacterResponseParameters> CreateCharacterAsync(
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

        public async Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
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

        public async Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield, 
            ValidateCharacterRequestParameters parameters)
        {
            return
                await ServerPeerHandler
                    .SendOperation<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>(
                        yield,
                        CharacterOperations.ValidateCharacter,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }

        public async Task<GetCharactersResponseParameters> GetCharactersAsync(
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
    }
}