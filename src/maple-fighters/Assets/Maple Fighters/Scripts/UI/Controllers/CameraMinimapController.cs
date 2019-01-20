using System.Collections;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    public class CameraMinimapController : MonoSingleton<CameraMinimapController>
    {
        private const string MiniCameraTag = "Minimap Camera";

        [SerializeField]
        private MarkSelection[] markSelections;

        private int selectionIndex;
        private new Camera camera;

        private MinimapWindow minimapWindow;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag);
            if (minimapCamera != null)
            {
                camera = minimapCamera.GetComponent<Camera>();
            }

            minimapWindow =
                UIElementsCreator.GetInstance().Create<MinimapWindow>();

            // TODO: Find a better solution, lol...
            DontDestroyOnLoad(minimapWindow.transform.root);
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (camera == null)
            {
                var minimapCamera =
                    GameObject.FindGameObjectWithTag(MiniCameraTag);
                if (minimapCamera != null)
                {
                    camera = minimapCamera.GetComponent<Camera>();

                    if (camera != null)
                    {
                        camera.cullingMask =
                            markSelections[selectionIndex].MarkLayerMask;
                    }
                }
            }
        }

        private void SubscribeToEvents()
        {
            if (minimapWindow != null)
            {
                minimapWindow.MarkSelectionChanged += OnMarkSelectionChanged;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void UnsubscribeFromEvents()
        {
            if (minimapWindow != null)
            {
                minimapWindow.MarkSelectionChanged -= OnMarkSelectionChanged;
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            UnsubscribeFromEvents();

            if (minimapWindow != null)
            {
                Destroy(minimapWindow.gameObject);
            }
        }

        private void OnMarkSelectionChanged(int index)
        {
            if (index < markSelections.Length)
            {
                selectionIndex = index;

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