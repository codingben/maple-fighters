using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class CharacterSelectorApi : ApiBase<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        public async Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            return 
                await ServerPeerHandler
                    .SendOperationAsync<CreateCharacterRequestParameters, CreateCharacterResponseParameters>(
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
                    .SendOperationAsync<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>(
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
                    .SendOperationAsync<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>(
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
                    .SendOperationAsync<EmptyParameters, GetCharactersResponseParameters>(
                        yield,
                        CharacterOperations.GetCharacters,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());
        }
    }
}