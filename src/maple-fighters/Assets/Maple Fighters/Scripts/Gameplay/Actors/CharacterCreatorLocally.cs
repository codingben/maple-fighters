using Game.Common;
using Scripts.UI.Controllers;
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

            // TODO: Use event bus system
            var chatController = FindObjectOfType<ChatController>();
            chatController.SetCharacterName(characterSpawnDetails.Character.Name);
        }

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(characterGameObject.transform);
        }

        private void InitializePlayerController()
        {
            var playerStateAnimator = 
                characterSpriteGameObject.AddComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController =
                    characterGameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.PlayerStateChanged +=
                        playerStateAnimator.OnPlayerStateChanged;
                }
            }
        }
    }
}