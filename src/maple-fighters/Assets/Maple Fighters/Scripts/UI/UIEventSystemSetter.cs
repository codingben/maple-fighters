using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Utils
{
    public class UIEventSystemSetter : MonoBehaviour
    {
        [SerializeField]
        private bool sendNavigationEvents;

        private void Start()
        {
            EventSystem.current.sendNavigationEvents = sendNavigationEvents;
        }
    }
}