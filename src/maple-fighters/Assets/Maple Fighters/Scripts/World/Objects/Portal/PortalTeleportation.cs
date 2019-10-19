using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Gameplay.GameEntity;
using Scripts.Services.Game;

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

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        public void Teleport()
        {
            if (coroutinesExecutor == null)
            {
                coroutinesExecutor.StartTask(
                    method: ChangeScene,
                    onException: (e) => 
                        Debug.LogError("Failed to send change scene operation."));
            }
        }

        private async Task ChangeScene(IYield yield)
        {
            var gameService = GameService.GetInstance();
            if (gameService != null)
            {
                var parameters = 
                    await gameService.GameSceneApi.ChangeSceneAsync(
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