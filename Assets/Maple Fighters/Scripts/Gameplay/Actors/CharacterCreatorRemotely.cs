using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorRemotely : CharacterCreatorBase
    {
        public override void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            base.Create(characterSpawnDetails);

            DisableCollision();
            RemoveAllCharacterControllerComponents();

            InitializePlayerStateSetter();
            InitializeSpriteRenderer();
            InitializeCharacterName(characterSpawnDetails.Character.Name);
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent = 
                characterSpriteGameObject.GetComponent<CharacterName>();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(orderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = 
                characterSpriteGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = orderInLayer;
        }

        private void DisableCollision()
        {
            characterGameObject.GetComponent<Collider2D>().isTrigger = true;
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = 
                characterGameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerStateSetter()
        {
            var playerStateSetter = GetComponent<PlayerStateSetter>();
            playerStateSetter.Animator =
                characterSpriteGameObject.GetComponent<Animator>();
        }
    }
}