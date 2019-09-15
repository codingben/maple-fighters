using System.Collections;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Mobs
{
    public class BlueSnail : MonoBehaviour
    {
        [Header("Attack"), SerializeField]
        private Vector2 hitAmount;

        private void Awake()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PlayerAttacked.AddListener(OnPlayerAttacked);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PlayerAttacked.RemoveListener(OnPlayerAttacked);
            }
        }

        private void OnPlayerAttacked(PlayerAttackedEventParameters parameters)
        {
            var point = 
                new Vector3(parameters.ContactPointX, parameters.ContactPointY);

            var spawnedCharacter = GetSpawnedCharacter();
            if (spawnedCharacter != null)
            {
                var direction = new Vector2(
                    x: ((spawnedCharacter.transform.position - point).normalized.x > 0
                            ? 1
                            : -1) * hitAmount.x,
                    y: hitAmount.y);

                StartCoroutine(BounceTheLocalPlayer(spawnedCharacter, direction));
            }
        }

        private IEnumerator BounceTheLocalPlayer(
            GameObject character, 
            Vector3 direction)
        {
            var playerController = character.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (playerController.PlayerState
                    != PlayerState.Attacked)
                {
                    playerController.ChangePlayerState(
                        PlayerState.Attacked);

                    yield return new WaitForSeconds(0.1f);

                    playerController.Bounce(direction);
                }
            }
        }

        private GameObject GetSpawnedCharacter()
        {
            GameObject spawnedCharacter = null;

            var player =
                SceneObjectsContainer.GetInstance().GetLocalSceneObject()
                    .GameObject;
            if (player != null)
            {
                const int CharacterIndex = 0;

                var transform = player.transform.GetChild(CharacterIndex);
                spawnedCharacter = transform.gameObject;
            }

            return spawnedCharacter;
        }
    }
}