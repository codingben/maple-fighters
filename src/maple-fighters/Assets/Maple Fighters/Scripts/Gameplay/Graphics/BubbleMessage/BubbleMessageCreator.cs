using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [ExecuteInEditMode]
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
            BubbleMessage.Create(owner, message, time);
        }

        private void Update()
        {
            if (owner == null)
            {
                owner = transform;
            }
        }
    }
}