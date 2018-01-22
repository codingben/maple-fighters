using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorRemotely : CharacterCreatorBase
    {
        public override void Create(CharacterInformation characterInformation)
        {
            base.Create(characterInformation);

            DisableCollision();
            RemoveAllCharacterControllerComponents();

            InitializePlayerStateSetter();
            InitializeCharacterNameDirectionSetter();
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent = characterSprite.GetComponent<CharacterName>();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(OrderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = characterSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = OrderInLayer;
        }

        private void InitializeCharacterInformationProvider(CharacterInformation characterInformation)
        {
            var characterInformationProvider = GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(characterInformation);
        }

        private void DisableCollision()
        {
            character.GetComponent<Collider2D>().isTrigger = true;
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = character.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerStateSetter()
        {
            var playerStateSetter = GetComponent<PlayerStateSetter>();
            playerStateSetter.Animator = characterSprite.GetComponent<Animator>();
        }

        private void InitializeCharacterNameDirectionSetter()
        {
            var characterNameComponent = characterSprite.GetComponent<CharacterName>();
            var positionSettter = GetComponent<PositionSetter>();
            positionSettter.DirectionChanged += characterNameComponent.OnDirectionChanged;
        }
    }
}