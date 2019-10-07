using System;
using UI.Manager;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public interface ISampleWindow : IView
    {
        event Action<PointerEventData> PointerClicked;
    }
}