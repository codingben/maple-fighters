using System;
using UI;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public interface ISampleWindow : IView
    {
        event Action<PointerEventData> PointerClicked;
    }
}