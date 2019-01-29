using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(PlayerStateSetter))]
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
            var characterNameSetter = characterSpriteGameObject
                .GetComponent<CharacterNameSetter>();
            if (characterNameSetter != null)
            {
                characterNameSetter.SetName(characterName);
                characterNameSetter.SetSortingOrder(orderInLayer);
            }
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer =
                characterSpriteGameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = orderInLayer;
            }
        }

        private void DisableCollision()
        {
            var collider = characterGameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = characterGameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerStateSetter()
        {
            var animator = characterSpriteGameObject.GetComponent<Animator>();
            if (animator != null)
            {
                var playerStateSetter = GetComponent<PlayerStateSetter>();
                playerStateSetter.Animator = animator;
            }
        }
    }
}