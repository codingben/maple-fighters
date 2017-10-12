using CommonTools.Log;
using Scripts.Utils.Shared;
using TMPro;
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

        public void Create(CharacterInformation characterInformation)
        {
            var name = characterInformation.CharacterName;
            var type = characterInformation.CharacterClass;

            var gameObject = Resources.Load<GameObject>(string.Format(GAME_OBJECTS_PATH, type));
            var character = Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);
            character.transform.SetAsFirstSibling();

            var characterNameComponent = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().AssertNotNull();
            characterNameComponent.text = name;

            var spriteRenderer = character.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = orderInLayer;

            var playerStateAnimatorNetwork = character.GetComponent<PlayerStateAnimatorNetwork>();
            playerStateAnimatorNetwork.IsLocal = isLocal;

            var playerController = GetComponent<PlayerController>();
            playerController.PlayerStateChanged = playerStateAnimatorNetwork.OnPlayerStateChanged;

            if (!isLocal)
            {
                var playerStateSetter = gameObject.GetComponent<PlayerStateSetter>();
                playerStateSetter.PlayerAnimator = playerStateAnimatorNetwork;
            }
        }
    }
}