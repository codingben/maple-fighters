using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(PlayerAnimatorProvider))]
    public class CharacterCreatorRemotely : CharacterCreatorBase
    {
        public override void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            base.Create(characterSpawnDetails);

            DisableCollision();
            RemoveAllCharacterControllerComponents();

            InitializePlayerAnimatorProvider();
            InitializeSpriteRenderer();
            InitializeCharacterName(characterSpawnDetails.Character.Name);
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameSetter = CharacterSpriteGameObject
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
                CharacterSpriteGameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = orderInLayer;
            }
        }

        private void DisableCollision()
        {
            var collider = CharacterGameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = 
                CharacterGameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerAnimatorProvider()
        {
            var animator = CharacterSpriteGameObject.GetComponent<Animator>();
            if (animator != null)
            {
                var playerAnimatorProvider = 
                    GetComponent<PlayerAnimatorProvider>();
                playerAnimatorProvider.Initialize(animator);
            }
        }
    }
}