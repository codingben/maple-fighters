using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Network.Services;
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
            coroutinesExecutor.StartTask(
                method: EnterSceneAsync,
                onException: (e) =>
                    Debug.LogError("Failed to send enter scene operation."));
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
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                await gameSceneApi.EnterSceneAsync(yield);
            }
        }
    }
}