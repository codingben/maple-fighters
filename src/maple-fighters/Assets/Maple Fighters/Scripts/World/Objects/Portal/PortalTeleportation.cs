using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Coroutines;
using Scripts.Gameplay.GameEntity;
using Scripts.Network.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.World.Objects
{
    [RequireComponent(typeof(Entity))]
    public class PortalTeleportation : MonoBehaviour
    {
        private int entityId;
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            var entity = GetComponent<IEntity>();
            entityId = entity.Id;
        }

        public void Teleport()
        {
            if (coroutinesExecutor == null)
            {
                coroutinesExecutor = new ExternalCoroutinesExecutor();
                coroutinesExecutor.ExecuteExternally();
                coroutinesExecutor.StartTask(
                    method: ChangeScene,
                    onException: (e) => 
                        Debug.LogError("Failed to send change scene operation."));
            }
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.RemoveFromExternalExecutor();
        }

        private async Task ChangeScene(IYield yield)
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                var parameters = 
                    await gameSceneApi.ChangeSceneAsync(
                        yield,
                        new ChangeSceneRequestParameters(entityId));

                var map = parameters.Map;
                if (map != 0)
                {
                    SceneManager.LoadScene(map.ToString());
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