using System;
using UI;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    /// <summary>
    /// The interface of the <see cref="SampleWindow"/>.
    /// </summary>
    public interface ISampleWindow : IView
    {
        event Action<PointerEventData> PointerClicked;
    }
}