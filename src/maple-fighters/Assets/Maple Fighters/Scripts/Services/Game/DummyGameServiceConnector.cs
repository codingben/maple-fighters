using System.Threading.Tasks;
using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class DummyGameServiceConnector : MonoBehaviour
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.StartTask(Connect);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        private async Task Connect(IYield yield)
        {
            var gameService = FindObjectOfType<GameService>();
            if (gameService != null)
            {
                await gameService.ConnectAsync(yield);
            }

            Destroy(gameObject);
        }
    }
}