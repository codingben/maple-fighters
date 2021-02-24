using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleWindow : UIElement, ISampleWindow, IPointerClickHandler
    {
        public event Action<PointerEventData> PointerClicked;

        private void Awake()
        {
            Shown += OnShown;
            Hidden += OnHidden;
        }

        private void OnDestroy()
        {
            Shown -= OnShown;
            Hidden -= OnHidden;
        }

        private void OnShown()
        {
            Debug.Log("SampleWindow::OnShown()");
        }

        private void OnHidden()
        {
            Debug.Log("SampleWindow::OnHidden()");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClicked?.Invoke(eventData);
        }
    }
}