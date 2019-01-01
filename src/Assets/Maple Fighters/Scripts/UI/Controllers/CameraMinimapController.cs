using System.Collections;
using CommonTools.Log;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    public class CameraMinimapController : MonoSingleton<CameraMinimapController>
    {
        private const string MiniCameraTag = "Minimap Camera";

        [SerializeField] private MarkSelection[] markSelections;

        private int curMarkLayer;
        private Camera minimapCamera;
        private MinimapWindow minimapWindow;

        private void Start()
        {
            minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag).GetComponent<Camera>();
            minimapWindow = UserInterfaceContainer.GetInstance().Add<MinimapWindow>();

            SubscribeToEvents();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (minimapCamera == null)
            {
                minimapCamera = GameObject.FindGameObjectWithTag(MiniCameraTag).GetComponent<Camera>();
                minimapCamera.cullingMask = markSelections[curMarkLayer].MarkLayerMask;
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
                UserInterfaceContainer.GetInstance()?.Remove(minimapWindow);
            }
        }

        private void OnMarkSelectionChanged(int selection)
        {
            if (selection >= markSelections.Length)
            {
                LogUtils.Log("You have selected a mark which is out of range of a mark selections.", LogMessageType.Error);
                return;
            }

            curMarkLayer = selection;
            minimapCamera.cullingMask = markSelections[selection].MarkLayerMask;

            StartCoroutine(SetSelectedGameObjectToNull());
        }

        private IEnumerator SetSelectedGameObjectToNull()
        {
            yield return new WaitForEndOfFrame();

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}