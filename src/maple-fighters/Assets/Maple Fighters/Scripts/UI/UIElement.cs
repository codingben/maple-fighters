﻿using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Base implementation of the <see cref="IView"/> interface (e.g. SampleWindow : UIElement, ISampleWindow).
    /// </summary>
    public class UIElement : MonoBehaviour, IView
    {
        /// <summary>
        /// The event invoked when <see cref="Show"/> called.
        /// </summary>
        public event Action Shown;

        /// <summary>
        /// The event invoked when <see cref="Hide"/> called.
        /// </summary>
        public event Action Hidden;

        public bool IsShown
        {
            get;
            private set;
        }

        public void Show()
        {
            IsShown = true;

            Shown?.Invoke();
        }

        public void Hide()
        {
            IsShown = false;

            Hidden?.Invoke();
        }
    }
}