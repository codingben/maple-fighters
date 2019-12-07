using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Map.Operations
{
    public class EnterSceneOperationSender : MonoBehaviour
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Start()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.StartTask(EnterSceneAsync);
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
                var api = gameService.GameSceneApi;
                if (api != null)
                {
                    await gameService.GameSceneApi.EnterSceneAsync(yield);
                }
            }
        }
    }
}