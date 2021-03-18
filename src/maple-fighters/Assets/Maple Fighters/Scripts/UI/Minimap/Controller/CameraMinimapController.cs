using System.Collections;
using Scripts.Constants;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Minimap
{
    public class CameraMinimapController : MonoBehaviour
    {
        [SerializeField]
        private UIMarkSelection[] markSelections;

        private new Camera camera;
        private IMinimapView minimapView;

        private void Awake()
        {
            var minimapCamera =
                GameObject.FindGameObjectWithTag(GameTags.MinimapCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<Camera>();
            }

            CreateAndSubscribeToMinimapWindow();
        }

        private void CreateAndSubscribeToMinimapWindow()
        {
            minimapView = UICreator
                .GetInstance()
                .Create<MinimapWindow>();

            if (minimapView != null)
            {
                minimapView.MarkSelectionChanged += OnMarkSelectionChanged;
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromMinimapWindow();
        }

        private void UnsubscribeFromMinimapWindow()
        {
            if (minimapView != null)
            {
                minimapView.MarkSelectionChanged -= OnMarkSelectionChanged;
            }
        }

        private void OnMarkSelectionChanged(int index)
        {
            if (index < markSelections.Length)
            {
                if (camera != null)
                {
                    camera.cullingMask =
                        markSelections[index].MarkLayerMask;
                }

                StartCoroutine(SetSelectedGameObjectToNull());
            }
        }

        private IEnumerator SetSelectedGameObjectToNull()
        {
            yield return new WaitForEndOfFrame();

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}