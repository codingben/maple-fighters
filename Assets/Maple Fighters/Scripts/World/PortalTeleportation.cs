using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Gameplay;
using Scripts.UI;
using Scripts.UI.Core;
using Game.Common;
using Scripts.Services;

namespace Scripts.World
{
    public class PortalTeleportation : PortalControllerBase
    {
        private bool isTeleporting;

        public override void StartInteraction()
        {
            base.StartInteraction();

            if (!isTeleporting)
            {
                var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
                screenFade?.Show(Teleport);
            }
        }

        public override void StopInteraction()
        {
            base.StopInteraction();

            if (!isTeleporting)
            {
                var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
                screenFade?.Hide();
            }
        }

        private void Teleport()
        {
            isTeleporting = true;

            CoroutinesExecutor.StartTask(ChangeScene, onException: exception => ServiceConnectionProviderUtils.OnOperationFailed());
        }

        private async Task ChangeScene(IYield yield)
        {
            var sceneObject = GetComponent<ISceneObject>();
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            var responseParameters = await gameScenePeerLogic.ChangeScene(yield, new ChangeSceneRequestParameters(sceneObject.Id));
            var map = responseParameters.Map;
            if (map == 0)
            {
                LogUtils.Log(MessageBuilder.Trace("You can not teleport to scene index 0."));
                return;
            }

            GameScenesController.Instance.LoadScene(map);

            Dispose();
        }
    }
}