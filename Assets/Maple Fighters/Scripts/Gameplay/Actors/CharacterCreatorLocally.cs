using Game.Common;
using Scripts.Utils.Shared;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorLocally : CharacterCreatorBase
    {
        public override void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            base.Create(characterSpawnDetails);

            SetCharacterToPositionSender();
            InitializePlayerController();
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

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(characterGameObject.transform);
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = 
                characterSpriteGameObject.AddComponent<PlayerStateAnimator>();

            var playerController =
                characterGameObject.GetComponent<PlayerController>();
            playerController.PlayerStateChanged +=
                playerStateAnimatorNetwork.OnPlayerStateChanged;
        }
    }
}