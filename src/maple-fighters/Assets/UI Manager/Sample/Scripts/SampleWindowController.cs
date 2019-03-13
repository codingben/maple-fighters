using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleWindowController : MonoBehaviour
    {
        private SampleWindow sampleWindow;

        private void Awake()
        {
            CreateAndSubscribeToSampleWindow();
        }

        private void Start()
        {
            ShowSampleWindow();
        }

        private void OnDestroy()
        {
            UnsubscribeFromSampleWindow();
        }

        private void CreateAndSubscribeToSampleWindow()
        {
            sampleWindow =
                UIElementsCreator.GetInstance().Create<SampleWindow>();
            sampleWindow.PointerClicked += OnPointerClicked;
        }

        private void UnsubscribeFromSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.PointerClicked -= OnPointerClicked;
            }
        }

        private void OnPointerClicked(PointerEventData eventData)
        {
            HideSampleWindow();
        }

        private void ShowSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.Show();
            }
        }

        private void HideSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.Hide();
            }
        }
    }
}