using CommonTools.Log;
using Game.Common;
using Scripts.Utils.Shared;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(PositionSender))]
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
                characterSpriteGameObject.GetComponent<CharacterName>()
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

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>().AssertNotNull();
            positionSender.SetCharacter(characterGameObject.transform);
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = 
                characterSpriteGameObject.AddComponent<PlayerStateAnimator>();

            var playerController =
                characterGameObject.GetComponent<PlayerController>()
                    .AssertNotNull();
            playerController.PlayerStateChanged +=
                playerStateAnimatorNetwork.OnPlayerStateChanged;
        }
    }
}