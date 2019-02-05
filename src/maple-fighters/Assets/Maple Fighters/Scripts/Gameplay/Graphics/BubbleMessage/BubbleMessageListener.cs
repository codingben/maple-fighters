using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
    public class BubbleMessageListener : MonoBehaviour
    {
        private void Awake()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.BubbleMessageReceived.AddListener(
                    OnBubbleMessageReceived);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.BubbleMessageReceived.RemoveListener(
                    OnBubbleMessageReceived);
            }
        }

        private void OnBubbleMessageReceived(
            BubbleMessageEventParameters parameters)
        {
            var id = parameters.RequesterId;
            var sceneObject = SceneObjectsContainer.GetInstance()
                .GetRemoteSceneObject(id);
            if (sceneObject != null)
            {
                var owner = sceneObject.GameObject.transform;
                var message = parameters.Message;
                var time = parameters.Time;
                BubbleMessageCreator.GetInstance().Create(owner, message, time);
            }
        }
    }
}