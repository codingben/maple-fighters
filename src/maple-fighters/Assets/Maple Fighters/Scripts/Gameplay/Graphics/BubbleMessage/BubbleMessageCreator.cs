using Scripts.Gameplay.Map;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageCreator : Singleton<BubbleMessageCreator>
    {
        private const string ResourcePath = "Game/Graphics/BubbleMessage";

        public void Create(Transform parent, string message, int time)
        {
            var bubbleMessageGameObject = CreateBubbleGameObject(parent);
            var bubbleMessage =
                bubbleMessageGameObject.GetComponent<BubbleMessage>();
            bubbleMessage.SetMessage(message);
            bubbleMessage.WaitAndDestroy(time);
        }

        private GameObject CreateBubbleGameObject(Transform parent)
        {
            var bubbleMessageObject = Resources.Load<GameObject>(ResourcePath);
            var bubbleMessageGameObject = Object.Instantiate(
                bubbleMessageObject,
                parent.position,
                Quaternion.identity,
                parent);
            bubbleMessageGameObject.name =
                bubbleMessageGameObject.name.RemoveCloneFromName();

            return bubbleMessageGameObject;
        }
    }
}