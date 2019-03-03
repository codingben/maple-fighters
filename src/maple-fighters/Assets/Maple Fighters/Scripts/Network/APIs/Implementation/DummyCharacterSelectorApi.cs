using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class DummyCharacterSelectorApi : ApiBase<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        private Dictionary<CharacterIndex, CharacterParameters> characters;

        public DummyCharacterSelectorApi()
        {
            characters = new Dictionary<CharacterIndex, CharacterParameters>
            {
                {
                    CharacterIndex.First,
                    new CharacterParameters
                    {
                        Name = "Dummy",
                        HasCharacter = false,
                        Index = CharacterIndex.First
                    }
                },
                {
                    CharacterIndex.Second,
                    new CharacterParameters
                    {
                        Name = "benzuk",
                        HasCharacter = true,
                        Index = CharacterIndex.Second,
                        CharacterType = CharacterClasses.Arrow
                    }
                },
                {
                    CharacterIndex.Third,
                    new CharacterParameters
                    {
                        Name = "Dummy",
                        HasCharacter = false,
                        Index = CharacterIndex.Third
                    }
                }
            };
        }

        public Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            var index = parameters.Index;
            var name = parameters.Name;
            var characterClass = parameters.CharacterClass;

            characters[index] =
                new CharacterParameters(name, characterClass, index)
                {
                    HasCharacter = true
                };

            return Task.FromResult(
                new CreateCharacterResponseParameters(
                    CharacterCreationStatus.Succeed));
        }

        public Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            var index = parameters.CharacterIndex;
            var characterIndex = (CharacterIndex)index;

            characters[characterIndex] = new CharacterParameters
            {
                Name = $"Dummy {index}",
                HasCharacter = false,
                Index = characterIndex
            };

            return Task.FromResult(
                new RemoveCharacterResponseParameters(
                    RemoveCharacterStatus.Succeed));
        }

        public Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters)
        {
            return Task.FromResult(
                new ValidateCharacterResponseParameters(
                    CharacterValidationStatus.Ok,
                    Maps.Map_1));
        }

        public Task<GetCharactersResponseParameters> GetCharactersAsync(
            IYield yield)
        {
            var characterParameterses = characters.Values.ToArray();

            return Task.FromResult(
                new GetCharactersResponseParameters(characterParameterses));
        }
    }
}