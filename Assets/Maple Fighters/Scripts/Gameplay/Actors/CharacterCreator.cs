using Scripts.Gameplay.Camera;
using Scripts.Utils.Shared;
using UnityEngine;
using CharacterInformation = Shared.Game.Common.CharacterInformation;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : MonoBehaviour
    {
        private const string GAME_OBJECTS_PATH = "Game/{0}";

        [Header("Network")]
        [SerializeField] private bool isLocal;
        [Header("Sprite")]
        [SerializeField] private int orderInLayer;

        private GameObject character;

        public void Create(CharacterInformation characterInformation)
        {
            var characterName = characterInformation.CharacterName;
            var characterClass = characterInformation.CharacterClass;

            var gameObject = Resources.Load<GameObject>(string.Format(GAME_OBJECTS_PATH, characterClass));
            var characterGameObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);
            characterGameObject.transform.localPosition = gameObject.transform.localPosition;
            characterGameObject.transform.SetAsFirstSibling();

            character = characterGameObject;

            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterInformation);

            if (isLocal)
            {
                InitializeLocallyOnly();
            }
            else
            {
                InitializeRemoteOnly();
            }
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent = character.GetComponent<CharacterName>();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(orderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = character.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = orderInLayer;
        }

        private void InitializePlayerStateAnimatorNetwork()
        {
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimatorNetwork>();
            playerStateAnimatorNetwork.IsLocal = isLocal;
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimatorNetwork>();
            var characterName = character.GetComponent<CharacterName>();
            var playerController = GetComponent<PlayerController>();
            playerController.PlayerStateChanged = playerStateAnimatorNetwork.OnPlayerStateChanged;
            playerController.ChangedDirection += characterName.OnChangedDirection;
        }

        private void InitializeCharacterInformationProvider(CharacterInformation characterInformation)
        {
            var characterInformationProvider = GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(characterInformation);
        }

        private void InitializeLocallyOnly()
        {
            SetCameraTarget();

            InitializePlayerController();
            InitializePlayerStateAnimatorNetwork();
        }

        private void SetCameraTarget()
        {
            var cameraControllerProvider = GetComponent<CameraControllerProvider>();
            cameraControllerProvider.SetCameraTarget();
        }

        private void InitializeRemoteOnly()
        {
            InitializePlayerStateSetter();
            InitializeCharacterNameDirectionSetter();
        }

        private void InitializePlayerStateSetter()
        {
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimatorNetwork>();
            var playerStateSetter = GetComponent<PlayerStateSetter>();
            playerStateSetter.PlayerAnimator = playerStateAnimatorNetwork;
        }

        private void InitializeCharacterNameDirectionSetter()
        {
            var characterNameComponent = character.GetComponent<CharacterName>();
            var positionSettter = GetComponent<PositionSetter>();
            positionSettter.DirectionChanged += characterNameComponent.OnChangedDirection;
        }
    }
}