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
            InitializeChatController(characterSpawnDetails.Character.Name);
        }

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(GetCharacterGameObject().transform);
        }

        private void InitializePlayerController()
        {
            var playerStateAnimator = 
                GetCharacterSpriteGameObject().AddComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController =
                    GetCharacterGameObject().GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.PlayerStateChanged +=
                        playerStateAnimator.OnPlayerStateChanged;
                }
            }
        }

        private void InitializeChatController(string characterName)
        {
            var chatController = FindObjectOfType<ChatController>();
            if (chatController != null)
            {
                chatController.SetCharacterName(characterName);
            }
        }
    }
}