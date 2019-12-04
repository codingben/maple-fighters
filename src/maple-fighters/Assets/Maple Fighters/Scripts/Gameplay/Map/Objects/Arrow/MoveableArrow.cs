using System.Collections;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MoveableArrow : MonoBehaviour
    {
        [SerializeField]
        private float moveTime;

        [SerializeField]
        private float moveSpeed;

        private new UnityEngine.Camera camera;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            var minimapCamera =
                GameObject.FindGameObjectWithTag(GameTags.MinimapCameraTag);
            camera = minimapCamera?.GetComponent<UnityEngine.Camera>();
            
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(MoveableArrowCoroutine());
        }

        private void Update()
        {
            if (camera != null)
            {
                spriteRenderer.enabled = 
                    Utils.IsInLayerMask(gameObject.layer, camera.cullingMask);
            }
        }

        private IEnumerator MoveableArrowCoroutine()
        {
            while (true)
            {
                if (camera != null)
                {
                    while (Utils.IsInLayerMask(gameObject.layer, camera.cullingMask))
                    {
                        yield return StartCoroutine(MoveArrowUp());
                        yield return StartCoroutine(MoveArrowDown());
                    }
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