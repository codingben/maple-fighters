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
        private MinimapWindow minimapWindow;
        
        private void Awake()
        {
            var minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<Camera>();
            }

            minimapWindow =
                UIElementsCreator.GetInstance().Create<MinimapWindow>();
            minimapWindow.MarkSelectionChanged += OnMarkSelectionChanged;
        }

        private void OnDestroy()
        {
            if (minimapWindow != null)
            {
                minimapWindow.MarkSelectionChanged -= OnMarkSelectionChanged;

                Destroy(minimapWindow.gameObject);
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