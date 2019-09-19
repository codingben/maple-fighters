using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class SendNavigationEventsSetter : MonoBehaviour
    {
        [SerializeField]
        private bool sendNavigationEvents;

        private void Start()
        {
            EventSystem.current.sendNavigationEvents = sendNavigationEvents;
        }
    }
}