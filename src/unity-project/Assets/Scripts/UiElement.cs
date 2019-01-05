using System;
using UnityEngine;

namespace UserInterface
{
    public class UiElement : MonoBehaviour
    {
        /// <summary>
        /// The event invoked when <see cref="Show"/> called.
        /// </summary>
        public event Action Shown;

        /// <summary>
        /// The event invoked when <see cref="Hide"/> called.
        /// </summary>
        public event Action Hidden;

        public void Show()
        {
            Shown?.Invoke();
        }

        public void Hide()
        {
            Hidden?.Invoke();
        }
    }
}