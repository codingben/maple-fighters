using Scripts.Gameplay.Camera;
using Scripts.Utils.Shared;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorLocally : CharacterCreatorBase
    {
        public override void Create(CharacterInformation characterInformation)
        {
            base.Create(characterInformation);

            SetCamerasTarget();
            SetCharacterToPositionSender();
            SetDirectionOnCreation();

            InitializePlayerController();
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent = characterSprite.GetComponent<CharacterName>();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(OrderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = characterSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = OrderInLayer;
        }

        private void SetCamerasTarget()
        {
            var cameraControllerProvider = character.GetComponent<CameraControllerProvider>();
            cameraControllerProvider.SetCamerasTarget();
        }

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetPlayerController(character.transform);
        }

        private void SetDirectionOnCreation()
        {
            const float SCALE = 1;

            var transform = character.transform;

            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
            }

            SetCharacterNameDirection();
        }

        private void SetCharacterNameDirection()
        {
            var characterNameComponent = characterSprite.GetComponent<CharacterName>();
            characterNameComponent.OnDirectionChanged(direction);
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = characterSprite.AddComponent<PlayerStateAnimator>();
            var characterName = characterSprite.GetComponent<CharacterName>();

            var playerController = character.GetComponent<PlayerController>();
            playerController.DirectionChanged += characterName.OnDirectionChanged;
            playerController.PlayerStateChanged += playerStateAnimatorNetwork.OnPlayerStateChanged;
        }
    }
}