using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorRemotely : CharacterCreatorBase
    {
        public override void Create(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            base.Create(characterSpawnDetails);

            DisableCollision();
            RemoveAllCharacterControllerComponents();

            InitializePlayerStateSetter();
            InitializeSpriteRenderer();
            InitializeCharacterNameDirectionSetter();
            InitializeCharacterName(characterSpawnDetails.Character.Name);
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