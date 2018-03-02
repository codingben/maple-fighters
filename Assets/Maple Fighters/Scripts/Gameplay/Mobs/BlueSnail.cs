using System.Collections;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Game.Common;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlueSnail : MonoBehaviour
    {
        private void Awake()
        {
            ServiceContainer.GameService.PlayerAttacked.AddListener((parameters) => 
            {
                StartCoroutine(HitPlayer(new Vector2(parameters.ContactPointX, parameters.ContactPointY)));
            });
        }

        private IEnumerator HitPlayer(Vector3 contactPoint)
        {
            var player = SceneObjectsContainer.Instance.GetLocalSceneObject().GetGameObject();

            var playerController = player?.transform.GetChild(0).GetComponent<PlayerController>();
            if (playerController == null || playerController.PlayerState == PlayerState.Attacked)
            {
                yield break;
            }

            playerController.PlayerState = PlayerState.Attacked;

            yield return new WaitForSeconds(0.1f);

            var direction = (player.transform.position - contactPoint).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2((direction.x > 0 ? 1 : -1) * 3, 3.0f), ForceMode2D.Impulse);
        }
    }
}