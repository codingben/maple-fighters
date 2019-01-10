using UnityEngine;
using UserInterface;

namespace Sample.Scripts
{
    public class SampleWindow : UIElement
    {
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
    }
}