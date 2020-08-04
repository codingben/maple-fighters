using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageListener : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Start()
        {
            gameApi = FindObjectOfType<GameApi>();
            gameApi.BubbleMessageReceived += OnBubbleMessageReceived;
        }

        private void OnDisable()
        {
            gameApi.BubbleMessageReceived -= OnBubbleMessageReceived;
        }

        private void OnBubbleMessageReceived(BubbleNotificationMessage message)
        {
            var id = message.NotifierId;

            if (EntityContainer.GetInstance().GetRemoteEntity(id, out var entity))
            {
                var owner = entity?.GameObject.transform;
                var text = message.Message;
                var time = message.Time;

                BubbleMessage.Create(owner, text, time);
            }
        }
    }
}