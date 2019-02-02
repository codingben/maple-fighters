using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers;
using Scripts.Coroutines;
using Scripts.Services;
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
                EnterScene,
                exception =>
                {
                    Debug.LogError(
                        "EnterSceneInvoker::Start() -> An exception occurred during the operation. The connection with the server has been lost.");
                });
        }

        private void OnDestroy()
        {
            coroutinesExecutor.RemoveFromExternalExecutor();
            coroutinesExecutor.Dispose();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private async Task EnterScene(IYield yield)
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();
            await gameScenePeerLogic.EnterScene(yield);
        }
    }
}