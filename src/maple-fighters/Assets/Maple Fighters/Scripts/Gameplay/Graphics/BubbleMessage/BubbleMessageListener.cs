using Game.Common;
using Scripts.Gameplay.Entity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageListener : MonoBehaviour
    {
        private GameService gameService;

        private void Start()
        {
            gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi?.BubbleMessageReceived.AddListener(OnBubbleMessageReceived);
        }

        private void OnDisable()
        {
            gameService?.GameSceneApi?.BubbleMessageReceived.RemoveListener(OnBubbleMessageReceived);
        }

        private void OnBubbleMessageReceived(BubbleMessageEventParameters parameters)
        {
            var entityId = parameters.RequesterId;
            var entity = EntityContainer.GetInstance().GetRemoteEntity(entityId)
                ?.GameObject;
            if (entity != null)
            {
                var owner = entity.transform;
                var message = parameters.Message;
                var time = parameters.Time;

                BubbleMessage.Create(owner, message, time);
            }
        }
    }
}