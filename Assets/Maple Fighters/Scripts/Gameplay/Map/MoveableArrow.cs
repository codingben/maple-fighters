using System.Collections;
using CommonTools.Log;
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

        private new Camera camera;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            var minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<Camera>().AssertNotNull();
            }

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
                camera.cullingMask);
        }

        private IEnumerator MoveableArrowCoroutine()
        {
            while (true)
            {
                while (Utils.IsInLayerMask(
                    gameObject.layer,
                    camera.cullingMask))
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