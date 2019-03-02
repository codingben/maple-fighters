using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Containers;
using Scripts.Network.APIs;
using Scripts.UI.Models;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(IOnCharacterReceivedListener))]
    public class CharacterViewInteractor : MonoBehaviour
    {
        private ICharacterSelectorApi characterSelectorApi;
        private IOnCharacterReceivedListener onCharacterReceivedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            characterSelectorApi =
                ServiceContainer.GameService.GetCharacterSelectorApi();
            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        public void GetCharacters()
        {
            coroutinesExecutor?.StartTask(GetCharactersAsync);
        }

        private async Task GetCharactersAsync(IYield yield)
        {
            var parameters =
                await characterSelectorApi.GetCharactersAsync(yield);

            var characters = parameters.Characters;
            foreach (var character in characters)
            {
                var characterDetails = new CharacterDetails(
                    character.Name,
                    ConvertToUiCharacterIndex(character.Index),
                    ConvertToUiCharacterClass(character.CharacterType),
                    ConvertToUiMapIndex(character.LastMap),
                    character.HasCharacter);

                onCharacterReceivedListener.OnCharacterReceived(
                    characterDetails);
            }
        }

        private UICharacterIndex ConvertToUiCharacterIndex(
            CharacterIndex characterIndex)
        {
            var uiCharacterIndex = UICharacterIndex.Zero;

            switch (characterIndex)
            {
                case CharacterIndex.Zero:
                {
                    uiCharacterIndex = UICharacterIndex.Zero;
                    break;
                }

                case CharacterIndex.First:
                {
                    uiCharacterIndex = UICharacterIndex.First;
                    break;
                }

                case CharacterIndex.Second:
                {
                    uiCharacterIndex = UICharacterIndex.Second;
                    break;
                }

                case CharacterIndex.Third:
                {
                    uiCharacterIndex = UICharacterIndex.Third;
                    break;
                }
            }

            return uiCharacterIndex;
        }

        private UICharacterClass ConvertToUiCharacterClass(
            CharacterClasses characterClasses)
        {
            var uiCharacterClass = UICharacterClass.Knight;

            switch (characterClasses)
            {
                case CharacterClasses.Knight:
                {
                    uiCharacterClass = UICharacterClass.Knight;
                    break;
                }

                case CharacterClasses.Arrow:
                {
                    uiCharacterClass = UICharacterClass.Arrow;
                    break;
                }

                case CharacterClasses.Wizard:
                {
                    uiCharacterClass = UICharacterClass.Wizard;
                    break;
                }
            }

            return uiCharacterClass;
        }

        private UIMapIndex ConvertToUiMapIndex(Maps map)
        {
            var uiMapIndex = UIMapIndex.Map_1;

            switch (map)
            {
                case Maps.Map_1:
                {
                    uiMapIndex = UIMapIndex.Map_1;
                    break;
                }

                case Maps.Map_2:
                {
                    uiMapIndex = UIMapIndex.Map_2;
                    break;
                }
            }

            return uiMapIndex;
        }
    }
}