using Game.Common;
using Scripts.Gameplay.Entity;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageListener : MonoBehaviour
    {
        private void Start()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.BubbleMessageReceived.AddListener(
                OnBubbleMessageReceived);
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.BubbleMessageReceived.RemoveListener(
                OnBubbleMessageReceived);
        }

        private void OnBubbleMessageReceived(
            BubbleMessageEventParameters parameters)
        {
            var id = parameters.RequesterId;
            var entity = EntityContainer.GetInstance().GetRemoteEntity(id);
            if (entity != null)
            {
                var owner = entity.GameObject.transform;
                var message = parameters.Message;
                var time = parameters.Time;
                BubbleMessageCreator.GetInstance().Create(owner, message, time);
            }
        }
    }
}