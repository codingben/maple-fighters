using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class DummyCharacterSelectorApi : ApiBase<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        public Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetCharactersResponseParameters> GetCharactersAsync(
            IYield yield)
        {
            var characters = new[]
            {
                new CharacterParameters(),
                new CharacterParameters(),
                new CharacterParameters()
            };

            characters[0].Name = "Dummy";
            characters[0].HasCharacter = false;
            characters[0].Index = CharacterIndex.First;

            characters[1].Name = "Dummy";
            characters[1].HasCharacter = false;
            characters[1].Index = CharacterIndex.Second;

            characters[2].Name = "Dummy";
            characters[2].HasCharacter = false;
            characters[2].Index = CharacterIndex.Third;

            return Task.FromResult(
                new GetCharactersResponseParameters(characters));
        }
    }
}