using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Containers;
using Scripts.Coroutines;
using Scripts.Gameplay;
using Scripts.Services;
using UnityEngine;

namespace Scripts.World
{
    [RequireComponent(typeof(ISceneObject))]
    public class PortalTeleportation : MonoBehaviour
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;

        public void Teleport()
        {
            if (coroutinesExecutor == null)
            {
                coroutinesExecutor = new ExternalCoroutinesExecutor();
                coroutinesExecutor.ExecuteExternally();
                coroutinesExecutor.StartTask(
                    ChangeScene,
                    onException: exception =>
                        ServiceConnectionProviderUtils.OperationFailed());
            }
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.RemoveFromExternalExecutor();
        }

        private async Task ChangeScene(IYield yield)
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();
            if (gameScenePeerLogic != null)
            {
                var sceneObject = GetComponent<ISceneObject>();
                var parameters = 
                    await gameScenePeerLogic.ChangeScene(
                        yield,
                        new ChangeSceneRequestParameters(sceneObject.Id));
                var map = parameters.Map;
                if (map != 0)
                {
                    GameScenesController.GetInstance().LoadScene(map);
                }
                else
                {
                    Debug.Log(
                        $"Unable to teleport to the desired map index: {map}.");
                }
            }
        }
    }
}