using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.World
{
    public class EnterSceneInvoker : MonoBehaviour
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Start()
        {
            if (ServiceContainer.GameService.IsConnected())
            {
                coroutinesExecutor.StartTask(
                    method: EnterSceneAsync,
                    onException: (e) =>
                        Debug.LogError("EnterSceneInvoker::Start() -> An exception occurred during the operation. The connection with the server has been lost."));
            }
            else
            {
                Debug.LogWarning(
                    "EnterSceneInvoker::Start() -> There is no connection with the game server.");
            }
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();
        }

        private async Task EnterSceneAsync(IYield yield)
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                await gameSceneApi.EnterSceneAsync(yield);
            }
        }
    }
}