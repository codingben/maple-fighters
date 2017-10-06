using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers.Service;
using Scripts.Coroutines;
using Scripts.UI;
using Scripts.UI.Core;
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

            UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull().Show(Teleport);
        }

        private void OnInteractionStopped()
        {
            UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull().Hide();
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