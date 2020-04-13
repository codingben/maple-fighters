using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class BubbleMessageCreator : MonoBehaviour
    {
        [SerializeField]
        private Transform owner;

        [SerializeField]
        private string message;

        [SerializeField]
        private int time;

        private void Start()
        {
            if (owner == null)
            {
                owner = transform;
            }

            BubbleMessage.Create(owner, message, time);
        }
    }
}