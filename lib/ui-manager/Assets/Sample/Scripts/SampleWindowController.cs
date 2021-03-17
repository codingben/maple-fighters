using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    /// <summary>
    /// The creator of the <see cref="SampleWindow"/>.
    /// </summary>
    public class SampleWindowController : MonoBehaviour
    {
        private ISampleWindow sampleWindow;

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
            sampleWindow = UICreator
                .GetInstance()
                .Create<SampleWindow>();

            if (sampleWindow != null)
            {
                sampleWindow.PointerClicked += OnPointerClicked;
            }
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
            sampleWindow?.Show();
        }

        private void HideSampleWindow()
        {
            sampleWindow?.Hide();
        }
    }
}