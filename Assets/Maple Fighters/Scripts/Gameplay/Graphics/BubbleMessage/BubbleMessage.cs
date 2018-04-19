using System.Collections;
using Scripts.World;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
    public class BubbleMessage : MonoBehaviour
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Text messageText;

        private void Awake()
        {
            transform.position = new Vector3(transform.position.x + position.x, transform.position.y + position.y);
        }

        public void Initialize(string message, int time)
        {
            messageText.text = message;

            StartCoroutine(WaitForDestroy(time));
        }

        private IEnumerator WaitForDestroy(int time)
        {
            yield return new WaitForSeconds(time);

            var fadeEffect = GetComponent<FadeEffect>();
            fadeEffect?.UnFade(() => Destroy(gameObject));
        }
    }
}