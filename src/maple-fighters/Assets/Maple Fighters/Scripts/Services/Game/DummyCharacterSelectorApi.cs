using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Network.Scripts;

namespace Scripts.Services.Game
{
    internal class DummyCharacterSelectorApi : NetworkApi<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        private readonly Dictionary<CharacterIndex, CharacterParameters> characters;

        internal DummyCharacterSelectorApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
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

        public async Task<CreateCharacterResponseParameters> CreateCharacterAsync(
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

            return 
                await Task.FromResult(
                    new CreateCharacterResponseParameters(
                        CharacterCreationStatus.Succeed));
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
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

            return 
                await Task.FromResult(
                    new RemoveCharacterResponseParameters(
                        RemoveCharacterStatus.Succeed));
        }

        public async Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters)
        {
            return
                await Task.FromResult(
                    new ValidateCharacterResponseParameters(
                        CharacterValidationStatus.Ok,
                        Maps.Map_1));
        }

        public async Task<GetCharactersResponseParameters> GetCharactersAsync(IYield yield)
        {
            return 
                await Task.FromResult(
                    new GetCharactersResponseParameters(
                        characters.Values.ToArray()));
        }
    }
}