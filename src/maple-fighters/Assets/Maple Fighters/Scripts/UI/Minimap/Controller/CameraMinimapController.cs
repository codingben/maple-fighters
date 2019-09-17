using System.Collections;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Controllers
{
    public class CameraMinimapController : MonoBehaviour
    {
        private const string MiniCameraTag = "Minimap Camera";

        [SerializeField]
        private MarkSelection[] markSelections;

        private new Camera camera;
        private IMinimapView minimapView;
        
        private void Awake()
        {
            var minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<Camera>();
            }

            CreateAndSubscribeToMinimapWindow();
        }

        private void CreateAndSubscribeToMinimapWindow()
        {
            minimapView =
                UIElementsCreator.GetInstance().Create<MinimapWindow>();
            minimapView.MarkSelectionChanged += OnMarkSelectionChanged;
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