using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    public class EnterSceneOperationSender : MonoBehaviour
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Start()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.StartTask(
                method: EnterSceneAsync,
                onException: (e) =>
                    Debug.LogError("Failed to send enter scene operation."));
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        private async Task EnterSceneAsync(IYield yield)
        {
            var gameService = FindObjectOfType<GameService>();
            if (gameService != null)
            {
                await gameService.GameSceneApi.EnterSceneAsync(yield);
            }
        }
    }
}