using CommonTools.Log;
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

            var chatController = FindObjectOfType<ChatController>();
            chatController.SetCharacterName(characterSpawnDetails.Character.Name);
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