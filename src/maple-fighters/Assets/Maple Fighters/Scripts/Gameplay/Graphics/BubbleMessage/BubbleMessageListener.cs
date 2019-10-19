using Game.Common;
using Scripts.Gameplay.GameEntity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageListener : MonoBehaviour
    {
        private IGameService gameService;

        private void Start()
        {
            gameService = GameService.GetInstance();
            gameService?.GameSceneApi.BubbleMessageReceived.AddListener(OnBubbleMessageReceived);
        }

        private void OnDestroy()
        {
            gameService = GameService.GetInstance();
            gameService?.GameSceneApi.BubbleMessageReceived.RemoveListener(OnBubbleMessageReceived);
        }

        private void OnBubbleMessageReceived(BubbleMessageEventParameters parameters)
        {
            var id = parameters.RequesterId;
            var entity = EntityContainer.GetInstance().GetRemoteEntity(id)
                ?.GameObject;
            if (entity != null)
            {
                var owner = entity.transform;
                var message = parameters.Message;
                var time = parameters.Time;
                BubbleMessageCreator.Create(owner, message, time);
            }
        }
    }
}