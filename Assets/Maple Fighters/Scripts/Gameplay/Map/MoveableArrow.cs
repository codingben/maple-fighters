using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    using Camera = UnityEngine.Camera;

    public class MoveableArrow : MonoBehaviour
    {
        private const string MINI_CAMERA_TAG = "Minimap Camera";

        [SerializeField] private float moveTime;
        [SerializeField] private float moveSpeed;

        private Camera minimapCamera;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            minimapCamera = GameObject.FindGameObjectWithTag(MINI_CAMERA_TAG).GetComponent<Camera>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(MoveableArrowCoroutine());
        }

        private void Update()
        {
            spriteRenderer.enabled = Utils.IsInLayerMask(gameObject.layer, minimapCamera.cullingMask);
        }

        private IEnumerator MoveableArrowCoroutine()
        {
            while (true)
            {
                while (Utils.IsInLayerMask(gameObject.layer, minimapCamera.cullingMask))
                {
                    yield return StartCoroutine(MoveArrowUp());
                    yield return StartCoroutine(MoveArrowDown());
                }
                yield return null;
            }
        }

        private IEnumerator MoveArrowUp()
        {
            var curTime = Time.time;

            do
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                yield return null;
            } while (Time.time < curTime + moveTime);
        }

        private IEnumerator MoveArrowDown()
        {
            var curTime = Time.time;

            do
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                yield return null;
            } while (Time.time < curTime + moveTime);
        }
    }
}