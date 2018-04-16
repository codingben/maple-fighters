using UnityEngine;

namespace InterestManagement.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(horizontal * speed, vertical * speed) * Time.deltaTime);
        }
    }
}