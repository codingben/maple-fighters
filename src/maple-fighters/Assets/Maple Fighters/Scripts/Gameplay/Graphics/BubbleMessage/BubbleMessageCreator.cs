using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageCreator
    {
        public static void Create(Transform parent, string message, int time)
        {
            var bubbleMessageObject = 
                Resources.Load<GameObject>(Paths.Resources.BubbleMessagePath);
            var bubbleMessageGameObject = Object.Instantiate(
                bubbleMessageObject,
                parent.position,
                Quaternion.identity,
                parent);
            if (bubbleMessageGameObject != null)
            {
                var bubbleMessage =
                    bubbleMessageGameObject.GetComponent<BubbleMessage>();
                bubbleMessage.SetMessage(message);
                bubbleMessage.WaitAndDestroy(time);
            }
        }
    }
}