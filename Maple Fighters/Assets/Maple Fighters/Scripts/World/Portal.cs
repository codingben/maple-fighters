using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers.Service;
using Scripts.Coroutines;
using Scripts.UI;
using Shared.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.World
{
    public class Portal : MonoBehaviour
    {
        public Action<Transform> StartInteraction;
        public Action StopInteraction;

        [Header("General")]
        [SerializeField] private Maps newMap;
        [SerializeField] private int sceneIndex;
        [Header("Player Position")]
        [SerializeField] private Vector3 newPosition;

        private Transform playerGameObject;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor().ExecuteExternally();

            StartInteraction = OnInteractionStarted;
            StopInteraction = OnInteractionStopped;
        }

        private void OnInteractionStarted(Transform gameObject)
        {
            if (!playerGameObject)
            {
                playerGameObject = gameObject;
            }

            ScreenFade.Instance.Fade(1, 10, Teleport);
        }

        private void OnInteractionStopped()
        {
            ScreenFade.Instance.UnFade(1);
        }

        private void Teleport()
        {
            coroutinesExecutor.StartTask(ChangeScene);
        }

        private async Task ChangeScene(IYield yield)
        {
            var parameters = new ChangeSceneRequestParameters(newMap);
            await ServiceContainer.GameService.ChangeScene(yield, parameters);

            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);

            playerGameObject.transform.position = newPosition;
        }
    }
}