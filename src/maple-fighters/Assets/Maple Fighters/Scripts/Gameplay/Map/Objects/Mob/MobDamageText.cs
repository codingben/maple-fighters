using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(Text))]
    public class MobDamageText : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void Start()
        {
            StartCoroutine(ColorEffect());
        }

        public void SetText(string value)
        {
            text.text = value;
        }

        private void Update()
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        private IEnumerator ColorEffect()
        {
            var color = new Color(0, 0, 0, 1);

            while (true)
            {
                if (text.color.a <= 0)
                {
                    Destroy(gameObject);
                    yield break;
                }

                text.color -= color * colorSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}