using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Coroutines;
using Scripts.Gameplay;
using Scripts.UI;
using Scripts.UI.Core;
using Game.Common;
using Scripts.Services;
using UnityEngine;

namespace Scripts.World
{
    public class PortalController : MonoBehaviour
    {
        private bool isTeleporting;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Awake()
        {
            coroutinesExecutor.ExecuteExternally();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.RemoveFromExternalExecutor();
            coroutinesExecutor.Dispose();
        }

        public void StartInteraction()
        {
            if (isTeleporting)
            {
                return;
            }

            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
            screenFade?.Show(Teleport);
        }

        public void StopInteraction()
        {
            if (isTeleporting)
            {
                return;
            }

            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
            screenFade?.Hide();

            coroutinesExecutor.Dispose();
        }

        private void Teleport()
        {
            isTeleporting = true;

            coroutinesExecutor.StartTask(ChangeScene);
        }

        private async Task ChangeScene(IYield yield)
        {
            var networkIdentity = GetComponent<NetworkIdentity>();
            var gameService = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            var responseParameters = await gameService.ChangeScene(yield, new ChangeSceneRequestParameters(networkIdentity.Id));
            var map = responseParameters.Map;
            if (map == 0)
            {
                LogUtils.Log(MessageBuilder.Trace("You can not teleport to scene index 0."));
                return;
            }

            GameScenesController.Instance.LoadScene(map);
        }
    }
}