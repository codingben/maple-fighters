using System.Collections;
using Scripts.World;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(FadeEffect))]
    public class BubbleMessage : MonoBehaviour
    {
        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private Text messageText;

        private void Awake()
        {
            transform.position = 
                new Vector3(
                    transform.position.x + position.x,
                    transform.position.y + position.y);
        }

        public void SetMessage(string message)
        {
            messageText.text = message;
        }

        public void WaitAndDestroy(int time)
        {
            StartCoroutine(WaitSomeTimeBeforeDestroy(time));
        }

        private IEnumerator WaitSomeTimeBeforeDestroy(int time)
        {
            yield return new WaitForSeconds(time);

            var fadeEffect = GetComponent<FadeEffect>();
            fadeEffect.UnFade(() => Destroy(gameObject));
        }
    }
}