using Scripts.Gameplay.Camera;
using Scripts.Utils.Shared;
using UnityEngine;
using CharacterInformation = Shared.Game.Common.CharacterInformation;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : MonoBehaviour
    {
        private const string GAME_OBJECTS_PATH = "Game/{0}";
        private const int CHARACTER_INDEX = 0;

        [Header("Network")]
        [SerializeField] private bool isLocal;
        [Header("Sprite")]
        [SerializeField] private int orderInLayer;

        private GameObject character;
        private GameObject characterController;

        public void Create(CharacterInformation characterInformation)
        {
            var characterName = characterInformation.CharacterName;
            var characterClass = characterInformation.CharacterClass;

            var gameObject = Resources.Load<GameObject>(string.Format(GAME_OBJECTS_PATH, characterClass));
            var characterGameObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);
            characterGameObject.transform.localPosition = gameObject.transform.localPosition;
            characterGameObject.transform.SetAsFirstSibling();

            character = characterGameObject.transform.GetChild(CHARACTER_INDEX).gameObject;
            characterController = characterGameObject;

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
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimator>();
            playerStateAnimatorNetwork.IsLocal = isLocal;
        }

        private void InitializePlayerController()
        {
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimator>();
            var characterName = character.GetComponent<CharacterName>();

            var playerController = characterController.GetComponent<PlayerController>();
            playerController.ChangedDirection += characterName.OnChangedDirection;
            playerController.PlayerStateChanged = playerStateAnimatorNetwork.OnPlayerStateChanged;
        }

        private void InitializeCharacterInformationProvider(CharacterInformation characterInformation)
        {
            var characterInformationProvider = GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(characterInformation);
        }

        private void InitializeLocallyOnly()
        {
            SetCamerasTarget();
            SetCharacterToPositionSender();

            InitializePlayerController();
            InitializePlayerStateAnimatorNetwork();
        }

        private void SetCamerasTarget()
        {
            var cameraControllerProvider = characterController.GetComponent<CameraControllerProvider>();
            cameraControllerProvider.SetCamerasTarget();
        }

        private void InitializeRemoteOnly()
        {
            DisableCollision();
            RemoveAllCharacterControllerComponents();

            InitializePlayerStateSetter();
            InitializeCharacterNameDirectionSetter();
        }

        private void DisableCollision()
        {
            characterController.GetComponent<Collider2D>().isTrigger = true;
        }

        private void SetCharacterToPositionSender()
        {
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetPlayerController(characterController.transform);
        }

        private void RemoveAllCharacterControllerComponents()
        {
            var components = characterController.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }

        private void InitializePlayerStateSetter()
        {
            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimator>();
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