using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Coroutines;
using Scripts.Gameplay;
using Scripts.Services;
using Scripts.UI;
using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.World
{
    public class PortalTeleportation : MonoBehaviour
    {
        private bool isTeleporting;
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.ExecuteExternally();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.RemoveFromExternalExecutor();
        }

        public void StartInteraction()
        {
            if (isTeleporting)
            {
                return;
            }

            var screenFade = 
                UserInterfaceContainer.GetInstance().Get<ScreenFade>()
                    .AssertNotNull();
            screenFade.Show(Teleport);
        }

        public void StopInteraction()
        {
            if (isTeleporting)
            {
                return;
            }

            var screenFade = 
                UserInterfaceContainer.GetInstance().Get<ScreenFade>()
                    .AssertNotNull();
            screenFade.Hide();
        }

        private void Teleport()
        {
            isTeleporting = true;

            coroutinesExecutor.StartTask(
                ChangeScene,
                onException: exception =>
                    ServiceConnectionProviderUtils.OnOperationFailed());
        }

        private async Task ChangeScene(IYield yield)
        {
            var sceneObject = GetComponent<ISceneObject>();
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();

            var responseParameters = 
                await gameScenePeerLogic.ChangeScene(
                    yield,
                    new ChangeSceneRequestParameters(sceneObject.Id));

            var map = responseParameters.Map;
            if (map != 0)
            {
                GameScenesController.GetInstance().LoadScene(map);
            }
            else
            {
                LogUtils.Log(MessageBuilder.Trace("You can not teleport to scene index 0."));
            }
        }
    }
}