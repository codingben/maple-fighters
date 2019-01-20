using CommonTools.Log;
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
                characterSpriteGameObject.GetComponent<CharacterNameSetter>()
                    .AssertNotNull();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(orderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = 
                characterSpriteGameObject.GetComponent<SpriteRenderer>()
                    .AssertNotNull();
            spriteRenderer.sortingOrder = orderInLayer;
        }

        private void DisableCollision()
        {
            var collider = characterGameObject.GetComponent<Collider2D>()
                .AssertNotNull();
            collider.isTrigger = true;
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = 
                characterGameObject.GetComponents<MonoBehaviour>()
                    .AssertNotNull();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerStateSetter()
        {
            var animator = 
                characterSpriteGameObject.GetComponent<Animator>()
                    .AssertNotNull();
            var playerStateSetter =
                GetComponent<PlayerStateSetter>().AssertNotNull();
            playerStateSetter.Animator = animator;
        }
    }
}