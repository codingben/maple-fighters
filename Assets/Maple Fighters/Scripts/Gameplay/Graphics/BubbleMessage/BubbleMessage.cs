using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
    public class BubbleMessage : MonoBehaviour
    {
        [SerializeField] private int time;
        [SerializeField] private Vector2 position;
        [SerializeField] private Text messageText;

        private void Awake()
        {
            transform.position = new Vector3(transform.position.x + position.x, transform.position.y + position.y);
        }

        public void Initialize(string message)
        {
            messageText.text = message;

            StartCoroutine(WaitForDestroy());
        }

        private IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}