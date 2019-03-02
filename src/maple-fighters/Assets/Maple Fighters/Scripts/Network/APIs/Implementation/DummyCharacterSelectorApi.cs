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
                new CharacterParameters(
                    name: "benzuk",
                    characterType: CharacterClasses.Arrow,
                    characterIndex: CharacterIndex.Second,
                    lastMap: Maps.Map_1)
            };

            return Task.FromResult(
                new GetCharactersResponseParameters(characters));
        }
    }
}