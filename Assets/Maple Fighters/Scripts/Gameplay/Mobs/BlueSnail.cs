using System.Collections;
using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Services;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlueSnail : MonoBehaviour
    {
        [Header("Attack"), SerializeField]
        private Vector2 hitAmount;

        private void Awake()
        {
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.PlayerAttacked.AddListener(OnPlayerAttacked);
        }

        private void OnDestroy()
        {
            var gameScenePeerLogic =
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.PlayerAttacked.RemoveListener(OnPlayerAttacked);
        }

        private void OnPlayerAttacked(PlayerAttackedEventParameters parameters)
        {
            var point = 
                new Vector2(parameters.ContactPointX, parameters.ContactPointY);

            StartCoroutine(BounceTheLocalPlayer(point));
        }

        private IEnumerator BounceTheLocalPlayer(Vector3 contactPoint)
        {
            var player = 
                SceneObjectsContainer.GetInstance().GetLocalSceneObject()
                    .GameObject;
            if (player != null)
            {
                const int CharacterIndex = 0;

                var characterGameObject =
                    player.transform.GetChild(CharacterIndex);
                if (characterGameObject != null)
                {
                    var playerController = 
                        characterGameObject.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        if (playerController.PlayerState
                            != PlayerState.Attacked)
                        {
                            playerController.ChangePlayerState(
                                PlayerState.Attacked);

                            yield return new WaitForSeconds(0.1f);

                            playerController.Bounce(
                                new Vector2(
                                    x: ((player.transform.position - contactPoint)
                                        .normalized.x > 0 ? 1 : -1) * hitAmount.x,
                                    y: hitAmount.y));
                        }
                    }
                }
            }
        }
    }
}