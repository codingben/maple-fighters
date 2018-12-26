using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    using Camera = UnityEngine.Camera;

    [RequireComponent(typeof(SpriteRenderer))]
    public class MoveableArrow : MonoBehaviour
    {
        private const string MiniCameraTag = "Minimap Camera";

        [SerializeField]
        private float moveTime;

        [SerializeField]
        private float moveSpeed;

        private Camera minimapCamera;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag)
                ?.GetComponent<Camera>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(MoveableArrowCoroutine());
        }

        private void Update()
        {
            spriteRenderer.enabled = Utils.IsInLayerMask(
                gameObject.layer,
                minimapCamera.cullingMask);
        }

        private IEnumerator MoveableArrowCoroutine()
        {
            while (true)
            {
                while (Utils.IsInLayerMask(
                    gameObject.layer,
                    minimapCamera.cullingMask))
                {
                    yield return StartCoroutine(MoveArrowUp());
                    yield return StartCoroutine(MoveArrowDown());
                }

                yield return null;
            }
        }

        private IEnumerator MoveArrowUp()
        {
            var currentTime = Time.time;

            do
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                yield return null;
            }
            while (Time.time < currentTime + moveTime);
        }

        private IEnumerator MoveArrowDown()
        {
            var currentTime = Time.time;

            do
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                yield return null;
            }
            while (Time.time < currentTime + moveTime);
        }
    }
}