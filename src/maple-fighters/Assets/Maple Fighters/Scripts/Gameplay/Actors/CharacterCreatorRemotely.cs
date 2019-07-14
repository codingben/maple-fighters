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
        }

        private void DisableCollision()
        {
            var collider = 
                GetCharacterGameObject().GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = 
                GetCharacterGameObject().GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerAnimatorProvider()
        {
            var animator = 
                GetCharacterSpriteGameObject().GetComponent<Animator>();
            if (animator != null)
            {
                var playerAnimatorProvider = 
                    GetComponent<PlayerAnimatorProvider>();
                playerAnimatorProvider.Initialize(animator);
            }
        }
    }
}