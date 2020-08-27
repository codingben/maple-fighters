using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    public class ClientInterestAreaVisualGraphicsTracker : MonoBehaviour
    {
        [SerializeField]
        private float speed = 7.5f;

        private Transform target;

        private void Update()
        {
            if (target != null)
            {
                Move();
            }
        }

        private void Move()
        {
            transform.position = Vector3.Lerp(
                transform.position, target.position, speed * Time.deltaTime);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}