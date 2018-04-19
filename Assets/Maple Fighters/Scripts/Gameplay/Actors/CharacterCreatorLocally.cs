using Scripts.Utils.Shared;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorLocally : CharacterCreatorBase
    {
        public override void Create(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            base.Create(characterSpawnDetails);

            SetCharacterToPositionSender();
            ChangeCharacterDirection();

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

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.Character = character.transform;
        }

        private void ChangeCharacterDirection()
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
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = characterSprite.AddComponent<PlayerStateAnimator>();
            var playerController = character.GetComponent<PlayerController>();
            playerController.PlayerStateChanged += playerStateAnimatorNetwork.OnPlayerStateChanged;
        }
    }
}